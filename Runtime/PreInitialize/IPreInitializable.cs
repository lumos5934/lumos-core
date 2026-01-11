using System;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace Lumos
{
    public interface IPreInitializable
    {
        public UniTask<bool> InitAsync();
    }
}