using System.Threading.Tasks;

namespace Lumos.DevPack
{
    public interface IBootable
    {
        public int Order { get; }
        public bool IsInitialized { get; }
        public Task InitAsync();
    }
}