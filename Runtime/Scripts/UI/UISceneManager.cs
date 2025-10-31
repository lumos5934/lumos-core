using UnityEngine;

namespace Lumos.DevPack
{
    public abstract class UISceneManager : UIBaseManager
    {
        #region >--------------------------------------------------- INIT

        
        public virtual void Init()
        {
            var sceneUI = FindObjectsByType<UIBase>(FindObjectsSortMode.None);

            for (int i = 0; i < sceneUI.Length; i++)
            {
                Register(sceneUI[i]);
            }
        }
        
        
        #endregion
        
    }
}