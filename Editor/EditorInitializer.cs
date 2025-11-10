using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LumosLib
{
    public static class EditorInitializer
    {
        [DidReloadScripts]
        static void TryCreateConfig()
        {
            
            string path = $"Assets/Resources/{Constant.GlobalConfig}.asset"; 
            
            GlobalConfigSO config = AssetDatabase.LoadAssetAtPath<GlobalConfigSO>(path);
            
            
            string folderPath = Path.GetDirectoryName(path);
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }
            
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<GlobalConfigSO>();
                
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        
        
        [DidReloadScripts]
        public static void CreateGlobalsScript()
        {
            
            string[] guids = AssetDatabase.FindAssets("Globals t:MonoScript", new[] { "Assets" });

            if (guids.Length > 0) return;

            
            string templatePath = $"{Constant.LumosLib}/Editor/Templates/TemplateGlobals.txt";
            
            string template = File.ReadAllText(templatePath);
            
            File.WriteAllText("Assets/Scripts/Globals.cs", template);

            AssetDatabase.Refresh();
            
        }
    }
}