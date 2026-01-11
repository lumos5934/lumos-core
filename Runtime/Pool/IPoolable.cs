namespace Lumos
{
    public interface IPoolable
    {
        public void OnGet();
        public void OnRelease();
    }
}

