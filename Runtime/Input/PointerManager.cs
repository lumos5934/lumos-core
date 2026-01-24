using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace LumosLib
{
    public class PointerManager : MonoBehaviour, IPreInitializable, IPointerManager
    {
        #region >--------------------------------------------------- PROPERTIE


        public bool IsPressed { get; private set; }
        public bool IsOverUI => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        public Vector2 ScreenPosition { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        
        private Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = Camera.main;
                }
                return _camera;
            }
        }


        #endregion
        #region >--------------------------------------------------- FIELD

        
        [SerializeField] private InputActionReference _posInputReference;
        [SerializeField] private InputActionReference _clickInputReference;

        [Title("REQUIREMENT")]
        [ShowInInspector, HideReferencePicker, ReadOnly, LabelText("IEventManager")] private IEventManager _eventManager;

        private Camera _camera;
        private Vector2 _worldPosition;
        
        
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
                pointerClickRef.action.started += OnPointerBegan;
                pointerClickRef.action.canceled += OnPointerEnded;
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
        #region >--------------------------------------------------- GET & SET


        public GameObject GetHitObject()
        {
            if (IsOverUI)
                return null;
            
            var hit = Physics2D.Raycast(WorldPosition, Vector2.zero);

            return hit.collider != null ? hit.collider.gameObject : null;
        }
        
        private void SetPointerPos(InputAction.CallbackContext context)
        {
            ScreenPosition = context.ReadValue<Vector2>();
            
            if (Camera == null)
            {
                DebugUtil.LogWarning("Camera is null", "FAIL");
                WorldPosition = Vector2.zero;
            }
            else
            {
                WorldPosition = Camera.ScreenToWorldPoint(ScreenPosition);
            }
        }
        
        public void SetCamera(Camera cam)
        {
            _camera = cam;
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE

        
        private void OnPointerBegan(InputAction.CallbackContext context)
        {
            IsPressed = true;
            
            _eventManager.Publish(
                new PointerDownEvent(ScreenPosition, WorldPosition, GetHitObject()));
        }
        
        private void OnPointerEnded(InputAction.CallbackContext context)
        {
            IsPressed = false;

            _eventManager.Publish(
                new PointerUpEvent(ScreenPosition, WorldPosition, GetHitObject()));
        }
        
        
        #endregion
    }
}