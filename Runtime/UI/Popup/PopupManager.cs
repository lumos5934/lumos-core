using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace LumosLib
{
    public class PopupManager : MonoBehaviour, IPopupManager, IPreInitializable
    {
        #region >--------------------------------------------------- FIELD

        
        [PropertySpace(15f)]
        [Title("Parameter")]
        [SerializeField] private int _startOrder;
        [SerializeField] private int _orderInterval;
         
        [PropertySpace(15f)]
        [Title("Resource")]
        [SerializeField] private List<UIPopup> _popupPrefabs;
        
        
        private Dictionary<Type, UIPopup> _popupPool = new();
        private Dictionary<Type, UIPopup> _popupPrefabDict = new();
        private Stack<UIPopup> _popupStack = new();
        private Camera _camera;
       
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public async UniTask<bool> InitAsync()
        {
            _camera = GetComponentInChildren<Camera>();
            if (_camera == null)
                return await UniTask.FromResult(false);
            
            _camera.cullingMask = LayerMask.GetMask("UI");
            _camera.clearFlags = CameraClearFlags.Depth;
            
            
            foreach (var prefab in _popupPrefabs)
            {
                _popupPrefabDict[prefab.GetType()] = prefab;
            }

            UpdateCameraStack();
            
            SceneManager.sceneLoaded += ( scene, mode) =>
            {
                CloseAll();
                UpdateCameraStack();
            };
            
            GlobalService.Register<IPopupManager>(this);
            
            return await UniTask.FromResult(true);
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE
        
        
        public void Register(UIPopup popup)
        {
            var type = popup.GetType();
            _popupPool.TryAdd(type, popup);
        }

        public void Unregister(UIPopup popup)
        {
            var type = popup.GetType();
            _popupPool.Remove(type);
        }
        
        public T Get<T>() where T : UIPopup
        {
            var type = typeof(T);
            
            foreach (var popup in _popupStack)
            {
                if (popup.GetType() == type)
                {
                    return popup as T;
                }
            }
            
            return null;
        }
        
        
        public T Open<T>()  where T : UIPopup
        {
            var contains = Get<T>();
            if (contains != null)
                return contains;
            
            var type = typeof(T);
            
            if (!_popupPool.TryGetValue(type, out UIPopup popup))
            {
                _popupPrefabDict.TryGetValue(type, out UIPopup resource);
                if (resource == null)
                    return null;

                popup = Instantiate(resource);
                popup.Init();

                if (popup.IsGlobal)
                {
                    popup.transform.SetParent(transform);
                }
            }
            
            _popupStack.Push(popup);
            popup.SetOrder(_startOrder + _popupStack.Count * _orderInterval);
            popup.SetCamera(_camera);
            popup.Open();
            
            return popup as T;
        }
        
        public void Close()
        {
            if (_popupStack.Count == 0) 
                return;

            var popup = _popupStack.Pop();
            popup?.Close();
        }
        
        public void Close<T>() where T : UIPopup
        {
            var type = typeof(T);
            
            UIPopup targetPopup = null;

            var tempStackList = new List<UIPopup>(_popupStack);
            for (int i = 0; i < tempStackList.Count; i++)
            {
                if (tempStackList[i].GetType() == type)
                {
                    targetPopup = tempStackList[i];
                    tempStackList.RemoveAt(i); 
                    break;
                }
            }

            if (targetPopup == null) 
                return;
            
            
            targetPopup.Close(); 
            _popupStack.Clear();
        
            for (int i = tempStackList.Count - 1; i >= 0; i--)
            {
                var popup = tempStackList[i];
                _popupStack.Push(popup);
            
                popup.SetOrder(_startOrder + _popupStack.Count * _orderInterval);
            }
        }
        

        public void CloseAll()
        {
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                popup?.Close();
            }
        }


        private void UpdateCameraStack()
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var mainCamData = mainCam.GetUniversalAdditionalCameraData();
                var popupCamData = _camera.GetUniversalAdditionalCameraData();
                
                if (!mainCamData.cameraStack.Contains(_camera))
                {
                    mainCamData.cameraStack.Add(_camera);
                    
                    _camera.targetTexture = mainCam.targetTexture;
                    _camera.targetDisplay = mainCam.targetDisplay;
                    _camera.allowMSAA = mainCam.allowMSAA;
                
                    popupCamData.antialiasing = mainCamData.antialiasing;
                    popupCamData.antialiasingQuality = mainCamData.antialiasingQuality;
                }
            }
        }
        
        #endregion
#if UNITY_EDITOR
        #region >--------------------------------------------------- INSPECTOR
        
        
        [Button("Collect Popup Prefabs")]
        public void SetUIResources()
        {
            _popupPrefabs = new();
            
            var entries = ResourcesUtil.Find<UIPopup>(this, "", SearchOption.AllDirectories);

            foreach (var entry in entries)
            {
                var result = entry.GetResource<UIPopup>();
                if (result != null)
                {
                    _popupPrefabs.Add(result);
                }
            }
        }
        
        
        #endregion
#endif
    }
}