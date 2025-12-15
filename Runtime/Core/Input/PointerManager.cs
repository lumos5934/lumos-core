using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace LumosLib
{
    public class PointerManager : MonoBehaviour, IPreInitializer
    {
        #region >--------------------------------------------------- PROPERTIE

        
        public int PreInitOrder => (int)PreInitializeOrder.Pointer;
        public bool IsOverUI { get; private set; } 


        #endregion
        #region >--------------------------------------------------- FIELD

    
        private Vector2 _pointerPos;
        private Coroutine _pointerClickCoroutine;
        
        
        #endregion
        #region >--------------------------------------------------- EVENT

        
        public event UnityAction OnDown;
        public event UnityAction OnHold;
        public event UnityAction OnUp;
        
        
        #endregion
        #region >--------------------------------------------------- UNITY
        
        
        protected virtual void Awake()
        {
            GlobalService.Register(this);

            DontDestroyOnLoad(gameObject);
        }

        protected virtual void LateUpdate()
        {
            IsOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
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
            
            var pointerPosRef = Project.Config.PointerPosActionReference;
            if (pointerPosRef != null)
            {
                //pointerPosRef.action.performed += context =>  _pointerPos =  context.ReadValue<Vector2>();
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
        
        
        
        #endregion
        #region >--------------------------------------------------- POINTER


        private void StartedPointerDown(InputAction.CallbackContext context)
        {
            OnDown?.Invoke();
        
            _pointerClickCoroutine = StartCoroutine(PointerClickCoroutine());
        }
        
        private void CanceledPointerDown(InputAction.CallbackContext context)
        {
            OnUp?.Invoke();

            if (_pointerClickCoroutine != null)
            {
                StopCoroutine(_pointerClickCoroutine);
            }
        }

        private IEnumerator PointerClickCoroutine()
        {
            while (true)
            {
                yield return null;
                
                OnHold?.Invoke();
            }
        }
        
        
        #endregion
    }
}