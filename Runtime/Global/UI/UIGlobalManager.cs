using UnityEngine;

namespace LumosLib.Core
{
    public class UIGlobalManager : UIBaseGlobalManager
    {
        public override int PreInitOrder => (int)PreInitializeOrder.UI;

        public override void PreInit()
        {
            base.PreInit();
            
            PreInitialized = true;
            
            Debug.Log(1);
        }
    }
}