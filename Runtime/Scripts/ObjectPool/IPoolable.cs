namespace Lumos.DevPack
{
    public interface IPoolable
    {
        public void OnGet();
        public void OnRealease();
    }
}

