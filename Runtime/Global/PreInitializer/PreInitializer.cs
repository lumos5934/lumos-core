using UnityEngine;

namespace LLib.Core
{
    public static class PreInitializer
    {
        public static bool Initialized { get; private set; }
         
        static PreInitializer()
        {
            var runner = new GameObject("Initialize Runner").AddComponent<PreInitializeRunner>();
            runner.Run();
        }

        public static void SetInitialized(bool enabled)
        {
            Initialized = enabled;
        }
    }
}