namespace LumosLib.Core
{
    public interface IPreInitialize
    {
        public int PreID { get; }
        public int PreInitOrder { get; }
        public bool PreInitialized { get; }
        public void PreInit();
    }
}