using UnityEngine;

namespace LumosLib
{
    public struct PointerDownEvent : IGameEvent
    {
        public readonly Vector2 ScreenPosition;
        public readonly Vector2 WorldPosition;
        public readonly GameObject ScanObject;
        
        public PointerDownEvent(Vector2 screenPosition, Vector2 worldPosition, GameObject scanObject)
        {
            ScreenPosition = screenPosition;
            WorldPosition = worldPosition;
            ScanObject = scanObject;
        }
    }
}