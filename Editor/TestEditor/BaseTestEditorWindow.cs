using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Lumos.Core
{
    public abstract class BaseTestEditorWindow : EditorWindow
    {
        #region >--------------------------------------------------- FIELD

        
        private int _titleFontSize = 20;
        private static string _title;
        private Vector2 _scrollPos;
        private Color _titleColor  = Color.yellow;
        private List<TestEditorGroup> _groups = new();
        private HashSet<TestEditorGroup> _groupHashSet = new();
        
        
        #endregion
        #region >--------------------------------------------------- UNITY
        
        
        protected static void OnOpen<T>(string title) where T : BaseTestEditorWindow
        {
            var window = GetWindow<T>();
            window.titleContent = new GUIContent(title);
            window.Show();
        }

        protected virtual void OnEnable()
        {
            _groups.Clear();
            _groupHashSet.Clear();
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
            
            DrawTitle(_title);

            for (int i = 0; i < _groups.Count; i++)
            {
                _groups[i].Draw();
            }
            
            EditorGUILayout.Space(_titleFontSize);
            EditorGUILayout.EndScrollView();
            
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE
        
        
        protected TestEditorGroup AddGroup(string title, bool isFoldOut, UnityAction<TestEditorGroup> drawContents)
        {
            var group = CreateInstance<TestEditorGroup>();

            if (!_groupHashSet.Contains(group))
            {
                group.Init(title, isFoldOut, drawContents);
                _groups.Add(group);
                _groupHashSet.Add(group);
            }
            else
            {
                DestroyImmediate(group);
            }

            return group;
        }
       

        #endregion        
        #region >--------------------------------------------------- SET


        protected void SetTitle(string title)
        {
            _title = title;
        }
        
        protected void SetTitleFontSize(int fontSize)
        {
            _titleFontSize = fontSize;
        }

        protected void SetTitleColor(Color color)
        {
            _titleColor = color;
        }
        
        
        #endregion
        #region >--------------------------------------------------- DRAW : TITLE
        
        
        private void DrawTitle(string title)
        {
            EditorGUILayout.Space(_titleFontSize);
            
            EditorGUILayout.LabelField(title, new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = _titleFontSize,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = _titleColor },
                focused = { textColor = _titleColor },
                hover =  { textColor = _titleColor },
                active = { textColor = _titleColor },
            });
            
            EditorGUILayout.Space(_titleFontSize);
        }
        
        
        #endregion
    }
}
