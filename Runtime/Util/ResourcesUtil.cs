using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LumosLib
{
    public static class ResourcesUtil
    {
        public static List<ResourceEntry> Find<T>(Object owner, string folderPath, SearchOption searchOption)
        {
#if UNITY_EDITOR
            
            Undo.RecordObject(owner, "Find Assets");

            var results =  new List<ResourceEntry>();
            
            var path = folderPath.Replace("Assets/", "");
            path = path.Replace("Resources/", "");
            path = "Resources/" + path;
            
            string absolutePath = Path.Combine(Application.dataPath, path);
                
            if (!Directory.Exists(absolutePath))
            {
                DebugUtil.LogWarning($"folder path not found : {path}", "Resource");
                return null;
            }

            string[] files = Directory.GetFiles(absolutePath, "*.*", searchOption);
            
            foreach (var file in files)
            {
                if (file.EndsWith(".meta")) continue;
                
                string relativePath = "Assets" + file.Replace(Application.dataPath, "").Replace("\\", "/");

                Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(relativePath);
    
                ResourceEntry entry = null;
                
                foreach (var asset in allAssets)
                {   
                    if (asset == null) continue;

                    if (asset is T)
                    {
                        if (entry == null)
                        {
                            entry = new ResourceEntry()
                            {
                                key = Path.GetFileNameWithoutExtension(relativePath),
                                _resources = new()
                            };
                        }
                        
                        entry._resources.Add(asset);
                    }
                }
                
                if (entry != null && entry._resources.Count > 0)
                {
                    results.Add(entry);
                }
            }
            
            if (results.Count > 0)
            {
                EditorUtility.SetDirty(owner);
            }

            return results;
#endif
            return null;
        }
    }
    
    [System.Serializable]
    public class ResourceEntry
    {
        public string key;
        public List<Object> _resources;
        
        public T GetResource<T>()
        {
            foreach (var resource in _resources)
            {
                if (resource is T t)
                {
                    return t;
                }
            }
            
            return default;
        }
        
    }
}
