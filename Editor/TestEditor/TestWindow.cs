using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace LumosLib.Editor
{
    public class TestWindow : EditorWindow
    {
        private BaseTestWindowEntry[] _entries;
        private Vector2 _scrollPos;
        
        [MenuItem("Window/Lumos Lib/Test Window")]
        public static void Open()
        {
            var window = GetWindow<TestWindow>();
            window.titleContent = new GUIContent("Test Window");
            window.Show();
        }

        private void OnEnable()
        {
            _entries = TypeCache
                .GetTypesDerivedFrom<BaseTestWindowEntry>()
                .Where(t => !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t) as BaseTestWindowEntry)
                .ToArray();
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(
                _scrollPos,
                false,
                true,
                GUIStyle.none,
                GUI.skin.verticalScrollbar,
                GUIStyle.none
            );
            
            EditorGUILayout.Space(20f);
            
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 24;
            style.normal.textColor = Color.cyan;
            style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField("Test Window", style);
            
            EditorGUILayout.Space(20f);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            for (int i = 0; i < _entries.Length; i++)
            {
                _entries[i].Draw();
            }
            
            EditorGUILayout.EndScrollView();
        }
    }
}

