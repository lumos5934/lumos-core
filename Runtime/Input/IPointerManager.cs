using UnityEngine;

namespace LumosLib
{
    public interface IPointerManager
    {
        public bool IsOverUI { get; }
        public bool IsPressed { get; }
        public Vector2 ScreenPosition { get; }
        public Vector2 WorldPosition { get; }
        public GameObject GetHitObject();
    }
}