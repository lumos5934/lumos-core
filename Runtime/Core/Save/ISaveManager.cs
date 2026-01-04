using System.Threading.Tasks;

namespace LumosLib
{
    public interface ISaveManager
    {
        public void Register<T>(T data) where T : ISaveData;
        public Task SaveAsync<T>() where T : ISaveData;
        public Task<T> LoadAsync<T>() where T : ISaveData;
    }
}