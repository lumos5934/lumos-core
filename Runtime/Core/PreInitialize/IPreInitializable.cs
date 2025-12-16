using System;
using System.Collections;

namespace LumosLib
{
    public interface IPreInitializable
    {
        public IEnumerator InitAsync(Action<bool> onComplete);
    }
}