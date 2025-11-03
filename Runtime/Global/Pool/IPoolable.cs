namespace LumosLib.Core
{
    public interface IPoolable
    {
        public void OnGet();
        public void OnRelease();
    }
}

