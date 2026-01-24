using UnityEngine;

namespace LumosLib
{
    public struct PointerDownEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly Vector2 WorldPosition;
        public readonly GameObject HitObject;
        
        public PointerDownEvent(Vector2 screenPosition, Vector2 worldPosition, GameObject hitObject)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            HitObject = hitObject;
        }
    }
}