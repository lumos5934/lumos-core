namespace LLib.Core
{
    public interface IPoolable
    {
        public void OnGet();
        public void OnRelease();
    }
}

