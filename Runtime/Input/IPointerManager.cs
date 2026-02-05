using UnityEngine;
using UnityEngine.Events;

namespace LumosLib
{
    public interface IPointerManager
    {
        public bool IsPressed { get; }
        public Vector2 ScreenPosition { get; }
        public Vector2 WorldPosition { get; }
        public Collider2D GetHitCollider();
        public event UnityAction<PointerDownEvent> OnPointerDown;
        public event UnityAction<PointerUpEvent> OnPointerUp;
    }
}