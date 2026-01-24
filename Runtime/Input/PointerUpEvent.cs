using UnityEngine;

namespace LumosLib
{
    public struct PointerUpEvent : IGameEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly Vector2 WorldPosition;
        public readonly GameObject HitObject;
        
        public PointerUpEvent(Vector2 screenPosition, Vector2 worldPosition, GameObject hitObject)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            HitObject = hitObject;
        }
    }
}