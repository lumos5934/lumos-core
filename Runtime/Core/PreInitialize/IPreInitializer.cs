using System.Collections;

namespace LumosLib
{
    public interface IPreInitializer
    {
        public IEnumerator InitAsync();
    }
}