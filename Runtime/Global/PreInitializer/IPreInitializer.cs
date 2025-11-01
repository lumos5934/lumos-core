using System.Threading.Tasks;

namespace Lumos.DevPack
{
    public interface IPreInitializer
    {
        public int Order { get; }
        public bool IsInitialized { get; }
        public Task InitAsync();
    }
}