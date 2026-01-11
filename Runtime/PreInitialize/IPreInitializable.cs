using System;
using System.Collections;
using Cysharp.Threading.Tasks;

namespace Lumos.Core
{
    public interface IPreInitializable
    {
        public UniTask<bool> InitAsync();
    }
}