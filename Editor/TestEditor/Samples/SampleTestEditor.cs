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
        
        private TestEditorGroup _testGroup;
        private bool _isToggledTestGroup;
        
        private TestEditorGroup _test2Group;
        private bool _isToggledTest2Group;


        private GameObject _testObject;
        

       
        
        
        [MenuItem("✨Lumos Lib/01.Test Editor/Template")]
        public static void Open()
        {
            OnOpen<SampleTestEditor>("TEMPLATE");
        }

        
        //MEMO : if you want ( create group / change properties )
        private void OnEnable()
        {
            TitleFontSize = 20;
            
            _testGroup = CreateGroup("Test");
            _test2Group = CreateGroup("Test2");
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
            DrawGroup(_testGroup, group =>
            {
                group.DrawField("Int" ,ref _intField);
                group.DrawSpaceLine();
                group.DrawField("Float",ref _floatField);
                group.DrawField("String", ref _stringField);
                group.DrawField("Vector2", ref _vector2Field);
                group.DrawField("Vector3", ref _vector3Field);
                // MEMO : onClick call back => checked toggle
                group.DrawField("Boolean", ref _booleanField, null);
                group.DrawButton("Test Click", () =>
                {
                    Debug.Log("Test click");
                });
            });
            
            DrawToggleGroup(_testGroup, ref _isToggledTestGroup, group =>
            {
                group.DrawField("Int" ,ref _intField);
                group.DrawSpaceLine();
                group.DrawField("Float",ref _floatField);
                group.DrawField("String", ref _stringField);
                group.DrawField("Vector2", ref _vector2Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Boolean", ref _booleanField, null);
                group.DrawButton("Test Click", () =>
                {
                    Debug.Log("Test click");
                });
            });
            
            DrawToggleGroup(_test2Group, ref _isToggledTest2Group, group =>
            {
                group.DrawField("Int" ,ref _intField);
                group.DrawSpaceLine();
                group.DrawField("Float",ref _floatField);
                group.DrawField("String", ref _stringField);
                group.DrawField("Vector2", ref _vector2Field);
                group.DrawField("Vector3", ref _vector3Field);
                group.DrawField("Boolean", ref _booleanField, null);
                group.DrawButton("Test Click", () =>
                {
                    Debug.Log("Test click");
                });
            });

            FinishDraw();
        }
    }
}