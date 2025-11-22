using System.Collections;

namespace LumosLib
{
    public interface IPreInitializer
    {
        public int PreInitOrder { get; }
        public IEnumerator InitAsync();
    }
}