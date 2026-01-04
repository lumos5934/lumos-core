using System.Threading.Tasks;

namespace LumosLib
{
    public interface ISaveStorage
    {
        public Task SaveAsync<T>(T data) where T : ISaveData;
        public Task<T> LoadAsync<T>() where T : ISaveData;
    }
}