using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LumosLib
{
    public static class EditorInitializer
    {
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            EditorApplication.delayCall += OnEditorFullyLoaded;
            
        }
        
        private static void OnEditorFullyLoaded()
        {
            var name = nameof(LumosLibSettings);
            
            if (Resources.Load<LumosLibSettings>(name) != null)
                return;

            string resourcesDir = Path.Combine(Application.dataPath, "Resources");
            
            if (!Directory.Exists(resourcesDir))
                Directory.CreateDirectory(resourcesDir);

            var asset = ScriptableObject.CreateInstance<LumosLibSettings>();
            string assetPath = $"Assets/Resources/{name}.asset";

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}