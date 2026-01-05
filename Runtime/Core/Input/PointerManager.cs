using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace LumosLib
{
    public class PointerManager : MonoBehaviour, IPreInitializable, IPointerManager
    {
        #region >--------------------------------------------------- FIELD


        [SerializeField] private InputActionReference _posInputReference;
        [SerializeField] private InputActionReference _clickInputReference;

        private bool _isOverUI;
        private Vector2 _pos;
        private GameObject _scanObj;
        private Coroutine _clickCoroutine;
        
        [Title("REQUIREMENT")]
        [ShowInInspector, HideReferencePicker, ReadOnly, LabelText("IEventManager")] private IEventManager _eventManager;

        
        #endregion
        #region >--------------------------------------------------- UNITY


        private void LateUpdate()
        {
            _isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }


        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            _eventManager = GlobalService.Get<IEventManager>();
            if (_eventManager == null)
                return UniTask.FromResult(false);
            
            var pointerClickRef = _clickInputReference;
            if (pointerClickRef != null)
            {
                pointerClickRef.action.started += StartedPointerDown;
                pointerClickRef.action.canceled += CanceledPointerDown;
                pointerClickRef.action.actionMap.Enable(); 
            }
            
            var pointerPosRef = _posInputReference;
            if (pointerPosRef != null)
            {
                pointerPosRef.action.performed += SetPointerPos;
                pointerPosRef.action.actionMap.Enable(); 
            }
            
            GlobalService.Register<IPointerManager>(this);
            return UniTask.FromResult(true);
        }
  
        
        #endregion
        #region >--------------------------------------------------- SET


        public bool GetOverUI()
        {
            return _isOverUI;
        }
        
        public Vector2 GetPos()
        {
            return _pos;
        }

        public GameObject GetScanObject(bool ignoreUI = true)
        {
            if (GetOverUI() && !ignoreUI)  return null;

            var cam = Camera.main;
            if (cam == null) return null;
            
            Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
            }
            
            return hit.collider != null ? hit.collider.gameObject : null;
        }
        
        private void SetPointerPos(InputAction.CallbackContext context)
        {
            _pos = context.ReadValue<Vector2>();
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE

        
        private void StartedPointerDown(InputAction.CallbackContext context)
        {
            _clickCoroutine = StartCoroutine(PointerClickCoroutine());
            
            _eventManager.Publish(new PointerDownEvent());
        }
        
        private void CanceledPointerDown(InputAction.CallbackContext context)
        {
            if (_clickCoroutine != null)
            {
                StopCoroutine(_clickCoroutine);
            }

            _eventManager.Publish(new PointerUpEvent());
        }

        private IEnumerator PointerClickCoroutine()
        {
            while (true)
            {
                yield return null;
                
                _eventManager.Publish(new PointerHoldEvent());
            }
        }
        
        
        #endregion


    }
}