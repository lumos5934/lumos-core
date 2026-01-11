using Cysharp.Threading.Tasks;

namespace Lumos.Core
{
    public interface ISaveManager
    {
        public UniTask SaveAsync<T>(T data);
        public UniTask<T> LoadAsync<T>();
    }
}