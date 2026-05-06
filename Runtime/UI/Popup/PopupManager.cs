using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


namespace LLib
{
    public class PopupManager : MonoBehaviour, IPreInitializable
    {
        [InfoBox("Requirement : IResourceManager")]
        [SerializeField] private Canvas _dimmerCanvas;
        
        private Dictionary<Type, UIPopup> _popupPrefabDict = new();
        private IResourceManager _resourceMgr;
        private Dictionary<Type, UIPopup> _popupCache = new();
        private List<UIPopup> _openedPopups = new();
        private Camera _camera;

        
        private void Awake()
        {
            Services.Register(this);
        }
        
        
        public async UniTask<bool> InitAsync(PreInitContext ctx)
        {
            _resourceMgr = Services.Get<IResourceManager>();
            
            var resourceInit = _resourceMgr as IPreInitializable;
            if (resourceInit == null)
                return false;
            
            var result = await ctx.GetAsync(resourceInit);
            if (result == null) 
                return false;
            

            _camera = GetComponentInChildren<Camera>();
            if (_camera == null)
                return false;
            
            _camera.cullingMask = LayerMask.GetMask("UI");
            _camera.clearFlags = CameraClearFlags.Depth;


            if (_dimmerCanvas != null)
            {
                _dimmerCanvas.worldCamera = _camera;
                _dimmerCanvas.gameObject.SetActive(false);
            }
            
            
            var prefabs  = _resourceMgr.GetAll<UIPopup>("");
            
            foreach (var prefab in prefabs)
            {
                _popupPrefabDict[prefab.GetType()] = prefab;
            }

            UpdateCameraStack();
            
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                UpdateCameraStack();
            };

            return true;
        }
        
        
        internal void Add(UIPopup popup)
        {
            var type = popup.GetType();
            _popupCache.TryAdd(type, popup);
        }

        
        internal void Remove(UIPopup popup)
        {
            var type = popup.GetType();
            if (_popupCache.Remove(type))
            {
                UpdateOrders();
            }
        }
        
        
        private T Get<T>() where T : UIPopup
        {
            var type = typeof(T);
            
            foreach (var popup in _openedPopups)
            {
                if (popup.GetType() == type)
                {
                    return popup as T;
                }
            }
            
            return null;
        }

        public T Open<T>(Action<T> onBeforeOpen) where T : UIPopup
        {
            var opened = Get<T>();
            if (opened != null)
                return Open(opened, onBeforeOpen) as T;
            
            var type = typeof(T);
            
            if (!_popupCache.TryGetValue(type, out UIPopup popup))
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

            return Open(popup, onBeforeOpen) as T;
        }
        
        
        private UIPopup Open<T>(UIPopup popup, Action<T> onBeforeOpen) where T : UIPopup
        {
            if (_openedPopups.Contains(popup))
            {
                int index = _openedPopups.IndexOf(popup);
                if (index == _openedPopups.Count - 1)
                    return popup;

                _openedPopups.RemoveAt(index);
                _openedPopups.Add(popup);
            }
            else
            {
                _openedPopups.Add(popup);
                popup.SetCamera(_camera);
                popup.Open(onBeforeOpen);
            }
            
            UpdateOrders();
            
            return popup;
        }

        
        public void Close()
        {
            if (_openedPopups.Count == 0)
                return;

            int lastIndex = _openedPopups.Count - 1;
            var popup = _openedPopups[lastIndex];

            _openedPopups.RemoveAt(lastIndex);
            popup.Close();

            UpdateOrders();
        }

        
        public void Close<T>() where T : UIPopup
        {
            for (int i = _openedPopups.Count - 1; i >= 0; i--)
            {
                if (_openedPopups[i] is T popup)
                {
                    int index = _openedPopups.IndexOf(popup);
                    if (index < 0)
                        return;

                    _openedPopups.RemoveAt(index);
                    
                    popup.Close();
                    UpdateOrders();
                    return;
                }
            }
        }

        
        public void CloseAll()
        {
            foreach (var popup in _openedPopups)
            {
                popup.Close();
            }
            
            _openedPopups.Clear();
        }
        
        
        private void UpdateOrders()
        {
            int dimmerOrder = -1;
            
            for (int i = 0; i < _openedPopups.Count; i++)
            {
                int order = (i + 1) * 10;
                
                _openedPopups[i].SetOrder(order);
                
                if (_openedPopups[i].IsModal)
                {
                    dimmerOrder = order - 1;
                }
            }
            
            if (_dimmerCanvas != null)
            {
                if (dimmerOrder > -1)
                {
                    _dimmerCanvas.gameObject.SetActive(true);
                    _dimmerCanvas.sortingOrder = dimmerOrder;
                }
                else
                {
                    _dimmerCanvas.gameObject.SetActive(false);
                }
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
        
        

    }
}