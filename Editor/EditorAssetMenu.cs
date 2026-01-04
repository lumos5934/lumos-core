using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace LumosLib
{
    public static class EditorAssetMenu
    {
        #region >--------------------------------------------------- CORE


        private static string GetCurrentPath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (!Directory.Exists(path))
                path = Path.GetDirectoryName(path);

            return path;
        }
        
        
        #endregion
        #region >--------------------------------------------------- SCRIPT
        
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Script/Global", false, int.MinValue)]
        public static void CreateGlobalScript()
        {
            CreateScript("Global.cs", File.ReadAllText(Constant.PathGlobalTemplate));
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
            string path = GetCurrentPath();
            
            
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
        
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Scriptable Object/Sound Asset", false, int.MinValue)]
        public static void CreateSoundAssetSO()
        {
            CreateSO<SoundAsset>("NewSoundAsset.asset");
        }
        
        private static void CreateSO<T>(string assetName) where T : ScriptableObject
        {
            string path = GetCurrentPath();
            
            
            T asset = ScriptableObject.CreateInstance<T>();

            string fullPath = Path.Combine(path, assetName);

            ProjectWindowUtil.CreateAsset(asset, fullPath);

            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
        
        
        #endregion
        #region >--------------------------------------------------- PREFAB

        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Empty", false, int.MinValue)]
        public static void CreateEmptyPrefab()
        {
            CreatePrefab(new GameObject("Empty"));
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Event", false, int.MinValue)]
        public static void CreateEventManagerPrefab()
        {
            CreatePrefab<EventManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Resource", false, int.MinValue)]
        public static void CreateResourceManagerPrefab()
        {
            CreatePrefab<ResourceManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Audio", false, int.MinValue)]
        public static void CreateAudioManagerPrefab()
        {
            CreatePrefab<AudioManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/UI", false, int.MinValue)]
        public static void CreateUIManagerPrefab()
        {
            CreatePrefab<UIManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Database", false, int.MinValue)]
        public static void CreateDatabaseManagerPrefab()
        {
            CreatePrefab<DatabaseManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Pool", false, int.MinValue)]
        public static void CreatePoolManagerPrefab()
        {
            CreatePrefab<PoolManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Pointer", false, int.MinValue)]
        public static void CreatePointerManagerPrefab()
        {
            CreatePrefab<PointerManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Tutorial", false, int.MinValue)]
        public static void CreateTutorialManagerPrefab()
        {
            CreatePrefab<TutorialManager>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Audio Player", false, int.MinValue)]
        public static void CreateAudioPlayerPrefab()
        {
            CreatePrefab<AudioPlayer>();
        }
        
        [MenuItem("Assets/Create/[ ✨Lumos Lib ]/Prefab/Managers/Save", false, int.MinValue)]
        public static void CreateSaveManagerPrefab()
        {
            CreatePrefab<SaveManager>();
        }
  
        
        private static GameObject CreatePrefab<T>() where T : MonoBehaviour
        {
            string typeName = typeof(T).Name;
            
            GameObject obj = new GameObject(typeName);
            obj.AddComponent<T>();

            CreatePrefab(obj);

            return obj;
        }

        private static void CreatePrefab(GameObject obj)
        {
            string prefabPath = Path.Combine(GetCurrentPath(), obj.name + ".prefab");

            PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
            Object.DestroyImmediate(obj);
            AssetDatabase.Refresh();
        }
        

        #endregion
    }
}