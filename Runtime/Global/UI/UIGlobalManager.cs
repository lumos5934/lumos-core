using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lumos.DevPack
{
    public class UIGlobalManager : UIBaseManager, IPreInitializer
    {
        #region >--------------------------------------------------- PROPERTIES

        
        public int Order => (int)PreInitializeOrder.UI;
        public bool IsInitialized { get; private set; }


        #endregion
        #region >--------------------------------------------------- PROPERTIES


        private Dictionary<int, UIBase> _globalUIPrefabs = new();
        
        
        #endregion
        #region >--------------------------------------------------- INIT


        public Task InitAsync()
        {
            var uiGlobalPrefabs = Global.Get<ResourceManager>().LoadAll<UIBase>(Path.UI);

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var key = uiGlobalPrefabs[i].ID;
                var value = uiGlobalPrefabs[i];

                _globalUIPrefabs[key] = value;
            }
            
            Global.Register(this);
            
            
            IsInitialized = true;
            return Task.CompletedTask;
        }
  

        #endregion
        #region >--------------------------------------------------- GET & SET


        public override T Get<T>(int id)
        {
            var returnUi = base.Get<T>(id);
            
            if(returnUi != null) return returnUi;
            
            return TryCreateGlobalUI<T>(id);
        }
        
        private T TryCreateGlobalUI<T>(int id) where T : UIBase
        {
            if (!_globalUIPrefabs.TryGetValue(id, out var prefab)) return null;

            var createdUI = Instantiate(prefab, transform);
            
            Register(createdUI);
            
            return createdUI as T;
        }


        #endregion
    }
}