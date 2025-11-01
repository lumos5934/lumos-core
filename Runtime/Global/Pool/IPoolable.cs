using UnityEngine.Events;

namespace Lumos.DevPack
{
    public interface IPoolable
    {
        public void OnGet();
        public void OnRelease();
    }
}

