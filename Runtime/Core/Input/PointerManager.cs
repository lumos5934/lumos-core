using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace LumosLib
{
    public class PointerManager : MonoBehaviour, IPreInitializer
    {
        #region >--------------------------------------------------- PROPERTIE

        
        public int PreInitOrder => (int)PreInitializeOrder.Pointer;


        #endregion
        #region >--------------------------------------------------- FIELD

    
        private Vector2 _pointerPos;
        private Vector2 _pointerDeltaPos;
        private Coroutine _pointerClickCoroutine;
        
        
        #endregion
        #region >--------------------------------------------------- EVENT

        
        public event UnityAction<Vector2> OnDown;
        public event UnityAction<Vector2> OnHold;
        public event UnityAction<Vector2> OnUp;
        
        
        #endregion
        #region >--------------------------------------------------- UNITY
        
        
        protected virtual void Awake()
        {
            GlobalService.Register(this);

            DontDestroyOnLoad(gameObject);
        }


        #endregion
        #region >--------------------------------------------------- INIT
        
        
        
        public IEnumerator InitAsync()
        {
            var pointerClickRef = Project.Config.PointerClickActionReference;
            
            if (pointerClickRef != null)
            {
                pointerClickRef.action.started += StartedPointerDown;
                pointerClickRef.action.canceled += CanceledPointerDown;
                pointerClickRef.action.actionMap.Enable(); 
            }
            
            var pointerPosRef = Project.Config.PointerMoveActionReference;
            if (pointerPosRef != null)
            {
                pointerPosRef.action.performed += PerformedPointerMove;
                pointerPosRef.action.canceled += CanceledPointerMove;
                pointerPosRef.action.actionMap.Enable(); 
            }
            
            yield break;
        }
        
        
        #endregion
        #region >--------------------------------------------------- GET
        
        
        
        public Vector2 GetPos()
        {
            return _pointerPos;
        }
        
        public Vector2 GetDeltaPos()
        {
            return _pointerDeltaPos;
        }
        
        
        #endregion
        #region >--------------------------------------------------- POINTER


        private void StartedPointerDown(InputAction.CallbackContext context)
        {
            OnDown?.Invoke(_pointerPos);

            _pointerClickCoroutine = StartCoroutine(PointerClickCoroutine());
        }
        
        private void CanceledPointerDown(InputAction.CallbackContext context)
        {
            OnUp?.Invoke(_pointerPos);
            
            StopCoroutine(_pointerClickCoroutine);
        }

        private IEnumerator PointerClickCoroutine()
        {
            while (true)
            {
                yield return null;
                
                OnHold?.Invoke(_pointerPos);
            }
        }


        private void PerformedPointerMove(InputAction.CallbackContext context)
        {
            var pointerPos = context.ReadValue<Vector2>();
            _pointerDeltaPos = pointerPos - _pointerPos;
            _pointerPos = pointerPos;
        }

        private void CanceledPointerMove(InputAction.CallbackContext context)
        {
            _pointerDeltaPos = _pointerPos;
        }

        
        #endregion
    }
}