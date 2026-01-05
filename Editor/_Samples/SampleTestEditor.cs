using UnityEditor;
using UnityEngine;

namespace LumosLib
{
    public class SampleTestEditor : BaseTestEditorWindow
    {
        private int _intField;
        private float _floatField;
        private string _stringField;
        private Vector2 _vector2Field;
        private Vector3 _vector3Field;
        private bool _booleanField;

        private GameObject _testObject;
        
        
        //menu name
        [MenuItem("[ ✨Lumos Lib ]/Test Editor/SAMPLE")]
        public static void Open()
        {
            //window tab name
            OnOpen<SampleTestEditor>("SAMPLE");
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            //window sign title
            SetTitle("SAMPLE");
            
            //default : yellow
            SetTitleColor(Color.white);
            
            //default : 20
            SetTitleFontSize(20);

            AddGroup("Script", false, group =>
            {
                group.DrawButton("Open", OpenScript);
            });
            
            AddGroup("Test", false, group =>
            {
                group.DrawField("Int" ,ref _intField);
                group.DrawSpaceLine();
                group.DrawField("Float",ref _floatField);
                group.DrawField("String", ref _stringField);
                group.DrawField("Vector2", ref _vector2Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Vector3", ref _vector3Field);
                // MEMO : onClick call back => checked toggle
                group.DrawField("Boolean", ref _booleanField, null);
                group.DrawButton("Test Click", () =>
                {
                    Debug.Log("Test click");
                });
            })
            //default : cyan
            .SetTitleColor(true, Color.green)
            //default : gray
            .SetTitleColor(false, Color.red)
            //default : 12
            .SetTitleFontSize(12);


            AddGroup("Test2", true, group =>
            {
                group.DrawField("Int", ref _intField);
                group.DrawSpaceLine();
                group.DrawField("Float", ref _floatField);
                group.DrawField("String", ref _stringField);
                group.DrawField("Vector2", ref _vector2Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Vector3", ref _vector3Field);
                // MEMO : onClick call back => checked toggle
                group.DrawField("Boolean", ref _booleanField, null);
                group.DrawButton("Test Click", () =>
                {
                    Debug.Log("Test click");
                });
            });
            
            AddGroup("Test3", true, group =>
            {
                group.DrawField("Int", ref _intField);
                group.DrawSpaceLine();
                group.DrawField("Float", ref _floatField);
                group.DrawField("String", ref _stringField);
                group.DrawField("Vector2", ref _vector2Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Vector3", ref _vector3Field);
                // MEMO : onClick call back => checked toggle
                group.DrawField("Boolean", ref _booleanField, null);
                group.DrawButton("Test Click", () =>
                {
                    Debug.Log("Test click");
                });
            })
            .SetRuntimeOnly(true);
        }
        
        private static void OpenScript()
        {
            string[] guids = AssetDatabase.FindAssets($"{nameof(SampleTestEditor)} t:MonoScript");

            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                MonoScript script =
                    AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                AssetDatabase.OpenAsset(script);
            }
        }
    }
}