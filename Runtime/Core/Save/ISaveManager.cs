using System.Threading.Tasks;

namespace LumosLib
{
    public interface ISaveManager
    {
        public Task SaveAsync<T>(T data) where T : ISaveData;
        public Task<T> LoadAsync<T>() where T : ISaveData;
    }
}