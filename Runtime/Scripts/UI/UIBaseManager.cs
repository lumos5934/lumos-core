using System.Collections.Generic;
using UnityEngine;

namespace Lumos.DevPack
{
    public abstract class UIBaseManager : MonoBehaviour
    {
        #region >--------------------------------------------------- FIELDS


        protected Dictionary<int, UIBase> _ui = new();


        #endregion
        #region >--------------------------------------------------- GET & SET


        public void SetEnable<T>(int id, bool enable) where T : UIBase
        {
            var ui = Get<T>(id);

            if (ui == null) return;

            ui.SetEnable(enable);
        }

        public virtual T Get<T>(int id) where T : UIBase
        {
            if (_ui.TryGetValue(id, out var ui))
            {
                return ui as T;
            }

            return null;
        }


        #endregion
        #region >--------------------------------------------------- REGISTER


        protected void Register<T>(T ui)  where T : UIBase
        {
            _ui[ui.ID] = ui;
        }

        
        #endregion
    }
}