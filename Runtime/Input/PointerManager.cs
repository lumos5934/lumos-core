using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace LumosLib
{
    public class PointerManager : MonoBehaviour, IPreInitializable, IPointerManager
    {
        #region >--------------------------------------------------- PROPERTIE


        public bool IsPressed { get; private set; }
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

        private Camera _camera;
        private Vector2 _worldPosition;
        
        
        #endregion
        #region >--------------------------------------------------- EVENT
        
        
        public event UnityAction<PointerDownEvent> OnPointerDown;
        public event UnityAction<PointerUpEvent> OnPointerUp;
        
        
        #endregion
        #region >--------------------------------------------------- UNITY

        private void Update()
        {
            ScreenPosition = _posInputReference.action.ReadValue<Vector2>();
            
            if (Camera == null)
            {
                DebugUtil.LogWarning("Camera is null", "FAIL");
                WorldPosition = Vector2.zero;
            }
            else
            {
                WorldPosition = Camera.ScreenToWorldPoint(ScreenPosition);
            }

            IsPressed = _clickInputReference.action.IsPressed();
                
            if (_clickInputReference.action.WasPressedThisFrame())
            {
                OnPointerDown?.Invoke(
                    new PointerDownEvent(ScreenPosition, WorldPosition, GetHitObject()));
            }

            if (_clickInputReference.action.WasReleasedThisFrame())
            {
                OnPointerUp?.Invoke(
                    new PointerUpEvent(ScreenPosition, WorldPosition, GetHitObject()));
            }
        }

        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            if (_clickInputReference == null ||
                _posInputReference == null)
            {
                gameObject.SetActive(false);
                return UniTask.FromResult(false);
            }
            
            _clickInputReference.action.actionMap.Enable(); 
            _posInputReference.action.actionMap.Enable(); 
            
            GlobalService.Register<IPointerManager>(this);
            
            return UniTask.FromResult(true);
        }
  
        
        #endregion
        #region >--------------------------------------------------- GET & SET


        public GameObject GetHitObject()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return null;
            
            var hitCollider = Physics2D.OverlapPoint(WorldPosition);

            return hitCollider != null ? hitCollider.gameObject : null;
        }
        
        public void SetCamera(Camera cam)
        {
            _camera = cam;
        }
        
        
        #endregion
    }
}