using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace LumosLib
{
    public static class ResourcesUtil
    {
        public static List<ResourceElement> Find<T>(Object owner, string folderPath, SearchOption searchOption)
        {
            
            Undo.RecordObject(owner, "Find Assets");

            var results =  new List<ResourceElement>();
            
            var path = folderPath.Replace("Assets/", "");
            path = path.Replace("Resources/", "");
            path = "Resources/" + path;
            
            string absolutePath = Path.Combine(Application.dataPath, path);
                
            if (!Directory.Exists(absolutePath))
                return null;
            

            string[] files = Directory.GetFiles(absolutePath, "*.*", searchOption);
            
            foreach (var file in files)
            {
                if (file.EndsWith(".meta")) continue;
                
                string relativePath = "Assets" + file.Replace(Application.dataPath, "").Replace("\\", "/");

                Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(relativePath);
    
                ResourceElement element = null;
                
                foreach (var asset in allAssets)
                {   
                    if (asset == null) continue;

                    if (asset is T)
                    {
                        if (element == null)
                        {
                            element = new ResourceElement()
                            {
                                key = Path.GetFileNameWithoutExtension(relativePath),
                                resources = new()
                            };
                        }
                        
                        element.resources.Add(asset);
                    }
                }
                
                if (element != null && element.resources.Count > 0)
                {
                    results.Add(element);
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

