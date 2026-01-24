using UnityEngine;

namespace LumosLib
{
    public interface IPointerManager
    {
        public bool IsPressed { get; }
        public Vector2 ScreenPosition { get; }
        public Vector2 WorldPosition { get; }
        public GameObject GetHitObject();
        public UnityAction<PointerDownEvent> OnPointerDown;
        public UnityAction<PointerUpEvent> OnPointerUp;
    }
}