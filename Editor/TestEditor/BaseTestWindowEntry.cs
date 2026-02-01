using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace LumosLib.Editor
{
    public abstract class BaseTestWindowEntry
    {
        protected abstract string Title { get; }
        
        private bool _isOpen;
        
        public void Draw()
        {
            EditorGUILayout.BeginVertical("box");
            
            var style = new GUIStyle(EditorStyles.foldout);
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 13;
            
            _isOpen = EditorGUILayout.Foldout(
                _isOpen,
                Title,
                true,
                style
            );

            if (_isOpen)
            {
                OnDraw();
            }
            
            EditorGUILayout.EndVertical();
        }

        protected abstract void OnDraw();
        
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

        
        protected void DrawSpaceLine(float height = 11)
        {
            EditorGUILayout.Space(height); 
        }
        
        protected void DrawField(string label, ref int value)
        {
            value = EditorGUILayout.IntField(label, value); 
        }
        
        protected void DrawField(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value); 
        }
        
        protected void DrawField(string label, ref Vector2 value)
        {
            value = EditorGUILayout.Vector2Field(label, value); 
        }
        
        protected void DrawField(string label, ref Vector2Int value)
        {
            value = EditorGUILayout.Vector2IntField(label, value); 
        }
     
        
        protected void DrawField(string label, ref Vector3 value)
        {
            value = EditorGUILayout.Vector3Field(label, value); 
        }
        
        protected void DrawField(string label, ref Vector3Int value)
        {
            value = EditorGUILayout.Vector3IntField(label, value); 
        }
        
        protected void DrawField(string label, ref Vector4 value)
        {
            value = EditorGUILayout.Vector4Field(label, value);
        }
        
        protected void DrawField(string label, ref string value)
        {
            value = EditorGUILayout.TextField(label, value); 
        }
        
        protected void DrawField<T>(string label, ref T value) where T : Object
        {
            value = (T)EditorGUILayout.ObjectField(label, value, typeof(T), true);
        }
        
        protected void DrawField(string label, ref Bounds value)
        {
            value = EditorGUILayout.BoundsField(label, value);
        }
        
        protected void DrawField(string label, ref BoundsInt value)
        {
            value = EditorGUILayout.BoundsIntField(label, value);
        }
        
        protected void DrawField(string label, ref Color value)
        {
            value = EditorGUILayout.ColorField(label, value);
        }
        
        protected void DrawField(string label, ref Rect value)
        {
            value = EditorGUILayout.RectField(label, value);
        }
        
        protected void DrawField(string label, ref RectInt value)
        {
            value = EditorGUILayout.RectIntField(label, value);
        }
        
        protected void DrawField(string label, ref Enum value)
        {
            value = EditorGUILayout.EnumFlagsField(label, value);
        }
        
        protected void DrawField(string label, ref bool value, UnityAction<bool> onClick)
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
    }
}