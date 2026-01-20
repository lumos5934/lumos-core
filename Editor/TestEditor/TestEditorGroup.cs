using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LumosLib.Editor
{
    public class TestEditorGroup : UnityEditor.Editor
    {
        #region >--------------------------------------------------- FIELD
        
        public event UnityAction<TestEditorGroup> OnDraw;
        
        private int _titleFontSize = 12;

        private string _title;
        private bool _isFoldOut;
        private bool _isToggled;
        private bool _isRuntimeOnly;
        
        private Color _enabledTitleColor = Color.cyan;
        private Color _disabledTitleColor = Color.gray;
        
        
        #endregion
        #region >--------------------------------------------------- INIT

        
        public void Init(string title, bool isFoldOut, UnityAction<TestEditorGroup> drawContents)
        {
            _title = title;
            _isFoldOut = isFoldOut;
            OnDraw = drawContents;
        }


        #endregion
        #region >--------------------------------------------------- DRAW


        public void Draw()
        {
            if (_isRuntimeOnly)
            {
                if (!Application.isPlaying)
                {
                    EditorGUILayout.HelpBox($"Group '{_title}' is runtime only", MessageType.Warning);
                    
                    return;
                }
            }
            
            if (_isFoldOut)
            {
                DrawFoldOut();
            }
            else
            {
                DrawBasic();
            }
        }

        private void DrawBasic()
        {
            EditorGUILayout.LabelField(_title, new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = _titleFontSize,
                normal = { textColor = _enabledTitleColor },
                focused = { textColor = _enabledTitleColor },
                hover =  { textColor = _enabledTitleColor },
                active = { textColor = _enabledTitleColor },
            });
            EditorGUILayout.BeginVertical("box");
            OnDraw?.Invoke(this);
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(_titleFontSize);
        }

        private void DrawFoldOut()
        {
            _isToggled = EditorGUILayout.Foldout(_isToggled, _title, true, new GUIStyle(EditorStyles.foldout)
            {
                fontSize = _titleFontSize,
                fontStyle = FontStyle.Bold,
                normal = { textColor = _disabledTitleColor },
                focused = { textColor = _disabledTitleColor },
                active = { textColor = _disabledTitleColor },
                hover = { textColor = _disabledTitleColor },
                onNormal = { textColor = _enabledTitleColor },
                onFocused =  { textColor = _enabledTitleColor },
                onHover = { textColor = _enabledTitleColor },
                onActive = { textColor = _enabledTitleColor },
            });

            if (_isToggled)
            {
                EditorGUILayout.BeginVertical("box");
                OnDraw?.Invoke(this);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(_titleFontSize);
            }
        }
        

        #endregion
        #region >--------------------------------------------------- DRAW : BUTTON
        
        
        public void DrawButton(string label, UnityAction onClick, float width = -1, float height = -1)
        {
            List<GUILayoutOption> options = new();

            if (width > 0) options.Add(GUILayout.Width(width));
            if (height > 0) options.Add(GUILayout.Height(height));

            if (GUILayout.Button(label, options.ToArray()))
            {
                onClick?.Invoke();
            }
        }
        
        
        #endregion
        #region >--------------------------------------------------- DRAW : FIELD

        
        public void DrawSpaceLine(float height = 11)
        {
            EditorGUILayout.Space(height); 
        }
        
        public void DrawField(string label, ref int value)
        {
            value = EditorGUILayout.IntField(label, value); 
        }
        
        public void DrawField(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value); 
        }
        
        public void DrawField(string label, ref Vector2 value)
        {
            value = EditorGUILayout.Vector2Field(label, value); 
        }
        
        public void DrawField(string label, ref Vector2Int value)
        {
            value = EditorGUILayout.Vector2IntField(label, value); 
        }
     
        
        public void DrawField(string label, ref Vector3 value)
        {
            value = EditorGUILayout.Vector3Field(label, value); 
        }
        
        public void DrawField(string label, ref Vector3Int value)
        {
            value = EditorGUILayout.Vector3IntField(label, value); 
        }
        
        public void DrawField(string label, ref Vector4 value)
        {
            value = EditorGUILayout.Vector4Field(label, value);
        }
        
        public void DrawField(string label, ref string value)
        {
            value = EditorGUILayout.TextField(label, value); 
        }
        
        public void DrawField<T>(string label, ref T value) where T : Object
        {
            value = (T)EditorGUILayout.ObjectField(label, value, typeof(T), false);
        }
        
        public void DrawField(string label, ref Bounds value)
        {
            value = EditorGUILayout.BoundsField(label, value);
        }
        
        public void DrawField(string label, ref BoundsInt value)
        {
            value = EditorGUILayout.BoundsIntField(label, value);
        }
        
        public void DrawField(string label, ref Color value)
        {
            value = EditorGUILayout.ColorField(label, value);
        }
        
        public void DrawField(string label, ref Rect value)
        {
            value = EditorGUILayout.RectField(label, value);
        }
        
        public void DrawField(string label, ref RectInt value)
        {
            value = EditorGUILayout.RectIntField(label, value);
        }
        
        public void DrawField(string label, ref Enum value)
        {
            value = EditorGUILayout.EnumFlagsField(label, value);
        }
        
        public void DrawField(string label, ref bool value, UnityAction<bool> onClick)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent(label)).x + 10));
            
            bool newValue = GUILayout.Toggle(value, GUIContent.none);

            if (newValue != value)
            {
                value = newValue;
                onClick?.Invoke(value);
            }

            GUILayout.EndHorizontal();
        }


        #endregion
        #region >--------------------------------------------------- SET


        public TestEditorGroup SetTitleColor(bool enabled, Color color)
        {
            if (enabled)
            {
                _enabledTitleColor = color;
            }
            else
            {
                _disabledTitleColor = color;
            }

            return this;
        }

        public TestEditorGroup SetTitleFontSize(int fontSize)
        {
            _titleFontSize = fontSize;
            
            return this;
        }

        public TestEditorGroup SetRuntimeOnly(bool runtimeOnly)
        {
            _isRuntimeOnly = runtimeOnly;

            return this;
        }
        

        #endregion
    }
}