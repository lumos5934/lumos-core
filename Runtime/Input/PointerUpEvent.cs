using UnityEngine;

namespace LumosLib
{
    public struct PointerUpEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly Vector2 WorldPosition;
        public readonly Collider2D HitCollider;

        
        public PointerUpEvent(Vector2 screenPosition, Vector2 worldPosition, Collider2D hitCollider)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            HitCollider = hitCollider;

        }
    }
}