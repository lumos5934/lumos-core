using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace LumosLib
{
    public static class AssetFinder
    {
        public static Dictionary<string, List<Object>> Find<T>(Object owner, string rootPath, SearchOption searchOption)
        {
            Undo.RecordObject(owner, "Find Assets");

            var results = new Dictionary<string, List<Object>>();
            
            string fullPath = Path.Combine(Application.dataPath, rootPath.Replace("Assets/", ""));
                
            if (!Directory.Exists(fullPath))
                return null;
            

            string[] files = Directory.GetFiles(fullPath, "*.*", searchOption);
            
            foreach (var file in files)
            {
                if (file.EndsWith(".meta")) continue;
                
                string assetPath = "Assets" + file.Replace(Application.dataPath, "").Replace('\\', '/');
                
                Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                string fileName = Path.GetFileNameWithoutExtension(assetPath);
                
                foreach (var asset in allAssets)
                {   
                    if (asset == null) continue;

                    if (asset is T)
                    {
                        if (!results.ContainsKey(fileName))
                        {
                            results[fileName] = new();
                        }
                        
                        results[fileName].Add(asset);
                    }
                }
            }
            
            if (results.Count > 0)
            {
                EditorUtility.SetDirty(owner);
            }

            return results;
        }
    }
}
#endif

