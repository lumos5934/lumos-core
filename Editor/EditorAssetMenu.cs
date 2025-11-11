using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace LumosLib
{
   
    public static class EditorAssetMenu
    {
        #region >--------------------------------------------------- SCRIPT
        
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Script/GlobalHub", false, int.MinValue)]
        public static void CreateGlobalHubScript()
        {
            CreateScript("GlobalHub.cs", File.ReadAllText(Constant.PathGlobalHubTemplate));
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Script/SceneManager", false, int.MinValue)]
        public static void CreateSceneManagerScript()
        {
            CreateScript("NewSceneManager.cs", File.ReadAllText(Constant.PathSceneManagerTemplate));
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Script/TestEditor", false, int.MinValue)]
        public static void CreateTestEditorScript()
        {
            CreateScript("NewTestEditor.cs", File.ReadAllText(Constant.PathTestEditorTemplate));
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Script/UI", false, int.MinValue)]
        public static void CreateUIScript()
        {
            CreateScript("UINew.cs", File.ReadAllText(Constant.PathUITemplate));
        }
        
        private static void CreateScript(string scriptName, string template)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);
            
            
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<OnCreateScript>(),
                Path.Combine(path, $"{scriptName}.cs"),
                null,
                template
            );
        }
        
        internal class OnCreateScript : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string className = Path.GetFileNameWithoutExtension(pathName);
                string finalContent = resourceFile.Replace("#SCRIPTNAME#", className);

                File.WriteAllText(pathName, finalContent);
                AssetDatabase.ImportAsset(pathName);
                var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
                ProjectWindowUtil.ShowCreatedAsset(asset);
            }
        }
        

        #endregion
        #region >--------------------------------------------------- SO
        
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Scriptable Object/PreInitialize Config", false, int.MinValue)]
        public static void CreatePreInitializeSO()
        {
            CreateSO<PreInitializeConfig>($"{Constant.PreInitializerConfig}.asset");
        }
             
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Scriptable Object/Sound Asset", false, int.MinValue)]
        public static void CreateSoundAssetSO()
        {
            CreateSO<SoundAsset>("NewSoundAsset.asset");
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