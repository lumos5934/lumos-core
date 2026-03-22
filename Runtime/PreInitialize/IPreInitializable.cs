using System;
using Cysharp.Threading.Tasks;

namespace LumosLib
{
    public interface IPreInitializable
    {
        Type RegisterType { get; }
        UniTask<bool> InitAsync(PreInitContext ctx);
    }
}