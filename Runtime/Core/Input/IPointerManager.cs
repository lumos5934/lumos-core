using UnityEngine;
using UnityEngine.Events;

namespace LumosLib
{
    public interface IPointerManager
    {
        public bool GetOverUI();
        public Vector2 GetPos();
        public GameObject GetScanObject(bool ignoreUI);
    }
}