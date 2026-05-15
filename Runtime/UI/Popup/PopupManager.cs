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
        [SerializeField] private int _startSortingOrder;
        [SerializeField] private Canvas _dimmerCanvas;
        
        private IResourceManager _resourceMgr;
        private Dictionary<Type, UIPopup> _popupPrefabDict = new();
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

            if (_popupCache.TryGetValue(type, out var containsPopup))
            {
                return containsPopup as T;
            }
            else
            {
                _popupPrefabDict.TryGetValue(type, out UIPopup resource);
                if (resource == null)
                    return null;

                var newPopup = Instantiate(resource);
                newPopup.Init();
                
                if (newPopup.IsGlobal)
                {
                    newPopup.transform.SetParent(transform);
                }
                
                return newPopup as T;
            }
        }

        
        public T Open<T>() where T : UIPopup
        {
            var popup = Get<T>();
            
            OnOpen(popup);
            
            popup.Open();
            
            return popup;
        }


        private void OnOpen(UIPopup popup)
        {
            if (_openedPopups.Contains(popup))
            {
                if (_openedPopups[^1] == popup)
                    return;

                _openedPopups.Remove(popup);
                _openedPopups.Add(popup);
            }
            else
            {
                popup.SetCamera(_camera);
            }
            
            _openedPopups.Add(popup);
            
            UpdateOrders();
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
            int order = 0;
            
            for (int i = 0; i < _openedPopups.Count; i++)
            {
                order = _startSortingOrder + (i + 1) * 10;
                
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