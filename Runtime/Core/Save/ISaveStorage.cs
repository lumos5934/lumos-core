using Cysharp.Threading.Tasks;

namespace LumosLib
{
    public interface ISaveStorage
    {
        public UniTask SaveAsync<T>(T data) where T : ISaveData;
        public UniTask<T> LoadAsync<T>() where T : ISaveData;
    }
}