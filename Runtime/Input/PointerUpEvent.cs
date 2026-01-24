using UnityEngine;

namespace LumosLib
{
    public struct PointerUpEvent : IGameEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly Vector2 WorldPosition;
        public readonly GameObject ScanObject;
        
        public PointerUpEvent(Vector2 screenPosition, Vector2 worldPosition, GameObject scanObject)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            ScanObject = scanObject;
        }
    }
}