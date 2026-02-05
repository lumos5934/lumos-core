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
            style.normal.textColor = Color.gray;
            style.onNormal.textColor = Color.green;
            style.hover.textColor  = Color.gray;
            style.onHover.textColor= Color.green;
            style.active.textColor  = Color.gray;
            style.onActive.textColor= Color.green;
            style.focused.textColor  = Color.gray;
            style.onFocused.textColor= Color.green;
            
            _isOpen = EditorGUILayout.Foldout(
                _isOpen,
                Title,
                true,
                style
            );

            if (_isOpen)
            {
                var contentStyle = new GUIStyle
                {
                    padding = new RectOffset(6, 6, 0, 0)
                };
                EditorGUILayout.BeginVertical(contentStyle);
               
                OnDraw();
                DrawLine();
                DrawButton("Open Script", OpenScript);
            
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndVertical();
        }

        private void OpenScript()
        {
            string[] guids = AssetDatabase.FindAssets($"{GetType().Name} t:MonoScript");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                if (script != null && script.GetClass() == GetType())
                {
                    AssetDatabase.OpenAsset(script);
                    return;
                }
            }
        }
        protected abstract void OnDraw();
        
        #region >--------------------------------------------------- DRAW : OTHER


        protected void DrawBox(Action drawContents)
        {
            EditorGUILayout.BeginVertical("box");
            
            drawContents?.Invoke();
            
            EditorGUILayout.EndVertical();
        }
        
        protected void DrawFoldout(string label, ref bool isOpen)
        {
            isOpen = EditorGUILayout.Foldout(isOpen, label, true);
        }
        
        protected void DrawLabel(string label, GUIStyle style = null)
        {
            if (style == null)
                GUILayout.Label(label);
            else
                GUILayout.Label(label, style);
        }

        protected void DrawLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }
        
        protected void DrawSpace(float height = 11)
        {
            EditorGUILayout.Space(height); 
        }
        
        protected void DrawButton(string label, UnityAction onClick, float width = -1, float height = -1)
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
        
        protected void DrawField(string label, ref bool toggleValue, UnityAction<bool> onClick)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent(label)).x + 10));
            
            bool newValue = GUILayout.Toggle(toggleValue, GUIContent.none);

            if (newValue != toggleValue)
            {
                toggleValue = newValue;
                onClick?.Invoke(toggleValue);
            }

            GUILayout.EndHorizontal();
        }

        
        #endregion
    }
}