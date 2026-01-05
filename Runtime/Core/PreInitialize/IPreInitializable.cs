using System;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace LumosLib
{
    public interface IPreInitializable
    {
        public UniTask<bool> InitAsync();
    }
}