namespace LumosLib
{
    public interface IPreInitialize
    {
        public int PreInitOrder { get; }
        public bool PreInitialized { get; }
    }
}