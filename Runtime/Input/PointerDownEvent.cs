using UnityEngine;

namespace LumosLib
{
    public struct PointerDownEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly Vector2 WorldPosition;
        public readonly Collider2D HitCollider;
        
        public PointerDownEvent(Vector2 screenPosition, Vector2 worldPosition, Collider2D hitCollider)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            HitCollider = hitCollider;
        }
    }
}