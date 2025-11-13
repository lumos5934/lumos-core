using UnityEditor;
using UnityEngine;

namespace LumosLib
{
    public static class ScriptableObjectUtil
    {
        public static T CreateAsset<T>(string path, string name = null) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();

            if (string.IsNullOrEmpty(name))
            {
                name = typeof(T).Name;
            }
        
            string assetPath = $"{path}/{name}.asset";

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }
    }
}