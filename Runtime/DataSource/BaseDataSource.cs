using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lumos
{
    public abstract class BaseDataSource : ScriptableObject
    {
        public abstract UniTask WriteAsync<T>(T data);
        public abstract UniTask<T> ReadAsync<T>();
    }
}