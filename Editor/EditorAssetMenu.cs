using System.IO;
using UnityEditor;
using UnityEngine;

namespace LumosLib
{
    public static class EditorAssetMenu
    {
        #region >--------------------------------------------------- SCRIPT

        
        [MenuItem("Assets/[ ✨Lumos Lib ]/Script/GlobalHub Script", false, 0)]
        public static void CreateGlobalHubScript()
        {
            CreateScript("GlobalHub.cs", File.ReadAllText(Constant.PathGlobalHubTemplate));
        }
        
        [MenuItem("Assets/[ ✨Lumos Lib ]/Script/SceneManager Script", false, 0)]
        public static void CreateSceneManagerScript()
        {
            CreateScript("NewSceneManager.cs", File.ReadAllText(Constant.PathSceneManagerTemplate));
        }
        
        [MenuItem("Assets/[ ✨Lumos Lib ]/Script/TestEditor Script", false, 0)]
        public static void CreateTestEditorScript()
        {
            CreateScript("NewTestEditor.cs", File.ReadAllText(Constant.PathTestEditorTemplate));
        }
        
        private static void CreateScript(string assetName, string content)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);
            
            
            string className = Path.GetFileNameWithoutExtension(assetName);
            string finalContent = content.Replace("#SCRIPTNAME#", className);
            
            ProjectWindowUtil.CreateAssetWithContent(Path.Combine(path, assetName), finalContent);
        }
        

        #endregion
        #region >--------------------------------------------------- SO
        
        
        [MenuItem("Assets/[ ✨Lumos Lib ]/Scriptable Object/PreInitialize Config SO", false, 0)]
        public static void CreatePreInitializeSO()
        {
            CreateSO<PreInitializeConfigSO>("PreInitialize Config.asset");
        }
             
        [MenuItem("Assets/[ ✨Lumos Lib ]/Scriptable Object/Sound Asset SO", false, 0)]
        public static void CreateSoundAssetSO()
        {
            CreateSO<SoundAssetSO>("NewSoundAsset.asset");
        }
        
        private static void CreateSO<T>(string assetName) where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);
            
            
            T asset = ScriptableObject.CreateInstance<T>();

            string fullPath = Path.Combine(path, assetName);

            ProjectWindowUtil.CreateAsset(asset, fullPath);

            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
        
        
        #endregion
    }
}