namespace LumosLib
{
    public static class Constant
    {
        #region >--------------------------------------------------- PATH
        
        
        public const string PathLumosLib = "Packages/com.lumos.library";
        public const string PathPreInitializerConfig = "Assets/Resources/PreInitializer Config.asset";
        public const string PathGlobalHubTemplate = PathLumosLib + "/Editor/Templates/GlobalHub.txt";
        public const string PathTestEditorTemplate = PathLumosLib + "/Editor/Templates/TestEditor.txt";
        public const string PathRuntimeSamples = PathLumosLib + "/Runtime/_Samples";
        
        
        #endregion
        #region >--------------------------------------------------- POOL

        
        public const int PoolDefaultCapacity = 10;
        public const int PoolMaxSize = 100;

        
        #endregion
    }
}