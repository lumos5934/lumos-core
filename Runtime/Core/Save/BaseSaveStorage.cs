using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LumosLib
{
    public abstract class BaseSaveStorage : ScriptableObject
    {
        public abstract UniTask SaveAsync<T>(T data) where T : ISaveData;
        public abstract UniTask<T> LoadAsync<T>() where T : ISaveData;
    }
}