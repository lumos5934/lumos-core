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
                pointerClickRef.action.started += StartPointerClick;
                pointerClickRef.action.canceled += EndPointerClick;
                pointerClickRef.action.actionMap.Enable(); 
            }
            
            var pointerPosRef = Project.Config.PointerPosActionReference;
            if (pointerPosRef != null)
            {
                pointerPosRef.action.performed += context => _pointerPos = context.ReadValue<Vector2>();
                pointerPosRef.action.actionMap.Enable(); 
            }
            
            var pointerDeltaPosRef = Project.Config.PointerDeltaPosActionReference;
            if (pointerDeltaPosRef != null)
            {
                pointerDeltaPosRef.action.performed += context => _pointerPos = context.ReadValue<Vector2>();
                pointerDeltaPosRef.action.actionMap.Enable(); 
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


        private void StartPointerClick(InputAction.CallbackContext context)
        {
            OnDown?.Invoke(_pointerPos);

            _pointerClickCoroutine = StartCoroutine(PointerClickCoroutine());
        }
        
        private void EndPointerClick(InputAction.CallbackContext context)
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

        
        #endregion
    }
}