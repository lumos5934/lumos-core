namespace LumosLib
{
    public static class Constant
    {
        #region >--------------------------------------------------- PATH
        
        
        public const string PathLumosLib = "Packages/com.lumos.library";
        public const string PathPreInitializerConfig = "PreInitialize Config";
        public const string PathGlobalHubTemplate = PathLumosLib + "/Editor/Templates/GlobalHub.txt";
        public const string PathSceneManagerTemplate = PathLumosLib + "/Editor/Templates/SceneManager.txt";
        public const string PathTestEditorTemplate = PathLumosLib + "/Editor/Templates/TestEditor.txt";
        public const string PathUITemplate = PathLumosLib + "/Editor/Templates/UI.txt";
        
        
        #endregion
        #region >--------------------------------------------------- POOL

        
        public const int PoolDefaultCapacity = 10;
        public const int PoolMaxSize = 100;

        
        #endregion
    }
}