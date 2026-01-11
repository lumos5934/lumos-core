using Cysharp.Threading.Tasks;

namespace Lumos
{
    public interface ISaveManager
    {
        public UniTask SaveAsync<T>(T data);
        public UniTask<T> LoadAsync<T>();
    }
}