namespace LumosLib
{
    public interface IPoolable
    {
        void OnCreated();
        void OnGet();
        void OnRelease();
        void OnDestroyed();
    }
}

