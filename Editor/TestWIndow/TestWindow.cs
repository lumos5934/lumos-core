using System;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LumosLib.Editor
{
    public class TestWindow : EditorWindow
    {
        private BaseTestWindowEntry[] _entries;
        private Vector2 _scrollPos;
        
        [MenuItem("Window/[ Lumos Lib ]/Test Window", false, int.MinValue)]
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
            
            EditorApplication.update += Repaint;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            
            EditorGUILayout.Space(20f);
            
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = 24;
            style.normal.textColor = Color.cyan;
            style.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.Space(10f);
            EditorGUILayout.LabelField("Test Window", style);
            EditorGUILayout.Space(20f);
            
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            for (int i = 0; i < _entries.Length; i++)
            {
                _entries[i].Draw();
            }
            
            EditorGUILayout.EndVertical();
            
            
            EditorGUILayout.Space(20f);
            EditorGUILayout.EndScrollView();
        }
    }
}

