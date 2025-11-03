using System.Collections.Generic;

namespace LLib.Core
{
    public abstract class UIBaseGlobalManager : UIBaseManager, IUIManager, IPreInitialize
    {
        #region >--------------------------------------------------- PROPERTIES


        public int PreID => (int)PreInitializeOrder.UI;
        public abstract int PreInitOrder { get; }
        public bool PreInitialized { get; protected set; }


        #endregion
        #region >--------------------------------------------------- PROPERTIES


        protected Dictionary<int, UIBase> _globalUIPrefabs = new();
        
        
        #endregion
        #region >--------------------------------------------------- INIT


        public virtual void PreInit()
        {
            var uiGlobalPrefabs = Global.Get<IResourceManager>().LoadAll<UIBase>(Constant.UI);

            for (int i = 0; i < uiGlobalPrefabs.Length; i++)
            {
                var key = uiGlobalPrefabs[i].ID;
                var value = uiGlobalPrefabs[i];

                _globalUIPrefabs[key] = value;
            }
            
            Global.Register((IUIManager)this);
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