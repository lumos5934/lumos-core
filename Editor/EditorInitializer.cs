using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Lumos.Core
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
            var name = nameof(LumosSettings);
            
            if (Resources.Load<LumosSettings>(name) != null)
                return;

            string resourcesDir = Path.Combine(Application.dataPath, "Resources");
            
            if (!Directory.Exists(resourcesDir))
                Directory.CreateDirectory(resourcesDir);

            var asset = ScriptableObject.CreateInstance<LumosSettings>();
            string assetPath = $"Assets/Resources/{name}.asset";

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}