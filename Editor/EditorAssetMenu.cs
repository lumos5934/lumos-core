using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Lumos.Core
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
        
        public static void CreatePrefab<T>() where T : MonoBehaviour
        {
            string typeName = typeof(T).Name;
            
            GameObject obj = new GameObject(typeName);
            obj.AddComponent<T>();

            CreatePrefab(obj);
        }
        
        private static void CreatePrefab(GameObject obj)
        {
            string prefabPath = Path.Combine(GetCurrentPath(), obj.name + ".prefab");

            PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
            
            Selection.activeObject = null;
            Object.DestroyImmediate(obj);
            AssetDatabase.Refresh();
        }
        

        
        #endregion
        #region >--------------------------------------------------- SCRIPT
        
        
        [MenuItem("Assets/Create/Scripting/Global", false, int.MinValue)]
        public static void CreateGlobalScript()
        {
            CreateScript("Global.cs", File.ReadAllText(Constant.PathGlobalTemplate));
        }
        
        [MenuItem("Assets/Create/Scripting/TestEditor", false, int.MinValue)]
        public static void CreateTestEditorScript()
        {
            CreateScript("NewTestEditor.cs", File.ReadAllText(Constant.PathTestEditorTemplate));
        }
        
        [MenuItem("Assets/Create/Scripting/UI", false, int.MinValue)]
        public static void CreateUIScript()
        {
            CreateScript("UINew.cs", File.ReadAllText(Constant.PathUITemplate));
        }
        
        public static void CreateScript(string scriptName, string template)
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
        #region >--------------------------------------------------- PREFAB

        
        [MenuItem("Assets/Create/Prefab/Empty", false, int.MinValue)]
        public static void CreateEmptyPrefab()
        {
            CreatePrefab(new GameObject("Empty"));
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Event", false, int.MinValue)]
        public static void CreateEventManagerPrefab()
        {
            CreatePrefab<EventManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Resource", false, int.MinValue)]
        public static void CreateResourceManagerPrefab()
        {
            CreatePrefab<ResourceManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Audio", false, int.MinValue)]
        public static void CreateAudioManagerPrefab()
        {
            CreatePrefab<AudioManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/UI", false, int.MinValue)]
        public static void CreateUIManagerPrefab()
        {
            CreatePrefab<UIManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Pool", false, int.MinValue)]
        public static void CreatePoolManagerPrefab()
        {
            CreatePrefab<PoolManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Pointer", false, int.MinValue)]
        public static void CreatePointerManagerPrefab()
        {
            CreatePrefab<PointerManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Tutorial", false, int.MinValue)]
        public static void CreateTutorialManagerPrefab()
        {
            CreatePrefab<TutorialManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Manager/Save", false, int.MinValue)]
        public static void CreateSaveManagerPrefab()
        {
            CreatePrefab<SaveManager>();
        }
        
        [MenuItem("Assets/Create/Prefab/Audio Player", false, int.MinValue)]
        public static void CreateAudioPlayerPrefab()
        {
            CreatePrefab<AudioPlayer>();
        }

        #endregion
    }
}