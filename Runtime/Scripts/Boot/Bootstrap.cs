using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Lumos.DevPack
{
    public static class Bootstrap
    {
        public static bool IsInitialized => _isInitialized;
        private static bool _isInitialized;

        static Bootstrap()
        {
            _ = InitAsync();
        }
        
        private static async Task InitAsync()
        {
            if (_isInitialized) return;
            
            var bootablePrefabs = Resources.LoadAll<MonoBehaviour>(Constant.BOOT)
                .OfType<IBootable>()
                .OrderBy(x => x.Order); 
            
            
            var sortedBootable = bootablePrefabs.OrderBy(x => x.Order).ToList();

            foreach (var bootable in sortedBootable)
            {
                var bootableObject = Object.Instantiate( bootable as MonoBehaviour).gameObject;

                await bootable.InitAsync();

                Global.Register(bootable);
                DebugUtil.Log(" INIT COMPLETE ", $" { bootableObject.name } ");
            }

            _isInitialized = true;
            DebugUtil.Log("", " All Managers INIT COMPLETE ");
        }
    }
}