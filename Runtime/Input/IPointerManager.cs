using UnityEngine;
using UnityEngine.Events;

namespace Lumos
{
    public interface IPointerManager
    {
        public bool GetOverUI();
        public Vector2 GetPos();
        public GameObject GetScanObject(bool ignoreUI);
    }
}