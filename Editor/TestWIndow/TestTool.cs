using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LumosLib.Editor
{
    public class TestTool : EditorWindow
    {
        private List<ITestToolElement> _elements;
        private ITestToolElement _selectedElement;
        private Vector2 _scrollPos;
        
        private GUIStyle _titleStyle;
        private GUIStyle _titleShadowStyle;
        
        private GUIStyle _mainRectStyle;
        private GUIStyle _sideRectStyle;
        private GUIStyle _contentRectStyle;

        private GUIStyle _btnStyle;
        private GUIStyle _noticeStyle;
        
        
        [MenuItem("Window/[ Lumos Lib ]/Test Tool", false, int.MinValue)]
        public static void Open()
        {
            var window = GetWindow<TestTool>();
            window.titleContent = new GUIContent("Test Tool");
            window.Show();
        }

        
        private void OnEnable()
        {
            _elements = TypeCache
                .GetTypesDerivedFrom<ITestToolElement>()
                .Where(t => !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t) as ITestToolElement)
                .OrderBy(t => t.Priority)
                .ToList();

            _titleStyle = null;
            _titleShadowStyle = null;
            
            _mainRectStyle = null;
            _sideRectStyle = null;
            _contentRectStyle = null;
            
            _btnStyle = null;
            _noticeStyle = null;
            
            EditorApplication.update += Repaint;
        }

        
        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
        }

        
        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            
            DrawTitle();

            Rect mainRect = new Rect();
            Rect sideRect = new Rect();
            Rect contentRect = new Rect();

            DrawMain(out mainRect, () =>
            {
                if (!Application.isPlaying)
                {
                    DrawNotice();
                    return;
                }
                
                if (_selectedElement == null)
                {
                    DrawGrid(mainRect);
                    EditorGUILayout.Space(3);
                    DrawElementButtons();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                
                    DrawSideRect(out sideRect, () =>
                    {
                        DrawGrid(sideRect);
                        DrawElementButtons();
                    });
                
                    DrawContentRect(out contentRect, () =>
                    {
                        if (_selectedElement != null)
                        {
                            _selectedElement.OnGUI();
                            HandleElementMenu(contentRect, _selectedElement);
                        }
                    });
                
                    EditorGUILayout.EndHorizontal();
                }
            });
            
            EditorGUILayout.EndScrollView();
        }

        
        #region Notice

        private void DrawNotice()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            string message = "⚠️ RUNTIME ONLY";
            GUILayout.Label(message, GetNoticeStyle(), GUILayout.MinWidth(300), GUILayout.MinHeight(100));

            GUILayout.FlexibleSpace(); // 오른쪽 여백 밀어내기
            EditorGUILayout.EndHorizontal();

            GUILayout.FlexibleSpace(); // 아래쪽 여백 밀어내기
            EditorGUILayout.EndVertical();
        }
        
        private GUIStyle GetNoticeStyle()
        {
            if (_noticeStyle != null)
                return _noticeStyle;

            _noticeStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(20, 20, 20, 20),
            };

            _noticeStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
            
            return _noticeStyle;
        }

        #endregion
        
        #region Title
        
        private void DrawTitle()
        {
            EditorGUILayout.Space(20f);

            var titleStyle = GetTitleStyle();
            
            GUIContent content = EditorGUIUtility.TrTextContent("TEST TOOL");
            Rect rect = GUILayoutUtility.GetRect(content, titleStyle);

            
            var shadowStyle = GetShadowStyle();
            GUI.Label(new Rect(rect.x + 2.5f, rect.y + 2.5f, rect.width, rect.height), content, shadowStyle);

            
            GUI.Label(rect, content, titleStyle);

            
            EditorGUILayout.Space(20f);
            
            DrawUnderline(GUILayoutUtility.GetLastRect());
        }
        
        
        private void DrawUnderline(Rect rect)
        {
            float lineY = rect.yMax - 1.5f;
            EditorGUI.DrawRect(new Rect(rect.x, lineY - 1f, rect.width, 1), new Color(0.6f, 0.6f, 0.6f, 0.15f));
            EditorGUI.DrawRect(new Rect(rect.x, lineY, rect.width, 2), new Color(1, 1, 1, 0.15f));
            EditorGUI.DrawRect(new Rect(rect.center.x - 20, lineY - 1f, 40, 1), new Color(0.6f, 0.45f, 0f, 0.6f));
            EditorGUI.DrawRect(new Rect(rect.center.x - 20, lineY, 40, 2), new Color(1f, 0.85f, 0.4f, 0.6f));
        }


        private GUIStyle GetShadowStyle()
        {
            if(_titleShadowStyle != null)
                return  _titleShadowStyle;
            
            _titleShadowStyle = new GUIStyle(EditorStyles.boldLabel);
            _titleShadowStyle.fontSize = 25;
            _titleShadowStyle.fontStyle = FontStyle.Bold;
            _titleShadowStyle.alignment = TextAnchor.MiddleCenter;
            
            Color mainColor = new Color(0.15f, 0.1f, 0f, 0.8f);
            _titleShadowStyle.normal.textColor = mainColor;
            _titleShadowStyle.hover.textColor = mainColor;
            _titleShadowStyle.active.textColor = mainColor;
            _titleShadowStyle.focused.textColor = mainColor;
                     
            _titleShadowStyle.hover.background = null;
            _titleShadowStyle.active.background = null;
            
            return _titleShadowStyle;
        }
        
        private GUIStyle GetTitleStyle()
        {
            if (_titleStyle != null)
                return _titleStyle;

            _titleStyle = new GUIStyle(EditorStyles.boldLabel);
            _titleStyle.fontSize = 25;
            _titleStyle.fontStyle = FontStyle.Bold;
            _titleStyle.alignment = TextAnchor.MiddleCenter;
        
            Color mainColor = new Color(1f, 0.9f, 0.9f);
            _titleStyle.normal.textColor = mainColor;
            _titleStyle.hover.textColor = mainColor;
            _titleStyle.active.textColor = mainColor;
            _titleStyle.focused.textColor = mainColor;
            
            _titleStyle.hover.background = null;
            _titleStyle.active.background = null;
            
            return _titleStyle;
        }

        #endregion
        
        #region Main

        private void DrawMain(out Rect rect, Action onMain)
        {
            GUI.backgroundColor = Color.black;
            rect = EditorGUILayout.BeginVertical(GetMainRectStyle());
            GUI.backgroundColor = Color.white;
            
            onMain?.Invoke();
            
            EditorGUILayout.EndVertical();
        }
        
        private GUIStyle GetMainRectStyle()
        {
            if (_mainRectStyle != null)
                return _mainRectStyle;
            
            _mainRectStyle = new GUIStyle("box")
            {
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                stretchWidth = true,
                stretchHeight = true,
            };
            
            return _mainRectStyle;
        }

        #endregion
        
        #region Side


        private void DrawSideRect(out Rect rect, Action onSide)
        {
            rect = EditorGUILayout.BeginVertical(GetSideRectStyle(), GUILayout.Width(90), GUILayout.ExpandHeight(true));
           
            onSide?.Invoke();
            
            EditorGUILayout.EndVertical();
        }


        private GUIStyle GetSideRectStyle()
        {
            if (_sideRectStyle != null)
                return _sideRectStyle;

            
            _sideRectStyle = new GUIStyle()
            {
                padding = new RectOffset(2, 0, 3, 3),
                margin = new RectOffset(0, 0, 0, 0)
            };

            return _sideRectStyle;
        }

        #endregion
        
        #region Button
        
        private void DrawElementButtons()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                var target = _elements[i];
                
                GUI.backgroundColor = (_selectedElement == target) ? 
                    new Color(0.40f,0.40f,0.40f, 1f) : 
                    Color.white;

                var btnStyle = GetButtonStyle();
                var label = target.Title.ToUpper();
                
                if (GUILayout.Button(label, btnStyle))
                {
                    _selectedElement = _selectedElement == target ? null : target;
                }
                
                Rect rect = GUILayoutUtility.GetLastRect();
                
                if (_selectedElement == target)
                {
                    rect.width -= 2;
                    rect.height -= 2;
                    rect.center -= new Vector2(0f, -1f);
                    
                    var outlineColor = new Color(1f, 0.85f, 0.4f, 0.9f);
                    
                    Handles.color = outlineColor;
                    // 내부 색상은 투명하게(new Color(0,0,0,0)), 테두리는 노란색으로
                    Handles.DrawSolidRectangleWithOutline(rect, 
                        new Color(0, 0, 0, 0), 
                        outlineColor);
                }
                
                GUI.backgroundColor = Color.white;
            }
        }
        
        
        private GUIStyle GetButtonStyle()
        {
            if (_btnStyle != null)
                return _btnStyle;

            _btnStyle = new GUIStyle(EditorStyles.miniButtonMid)
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0,0,0,0),
                overflow = new RectOffset(0,0,0,0),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                stretchWidth = true,
                fixedHeight = 25,
            };

            _btnStyle.hover.textColor = new Color(1f, 0.9f, 0.7f, 1);

            return _btnStyle;
        }

        
        #endregion
        
        #region Content

        private void DrawContentRect(out Rect rect, Action onContent)
        {
            GUI.backgroundColor = new Color(0f, 0f, 0f, 0.7f);
            rect = EditorGUILayout.BeginVertical(GetContentRectStyle(), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            GUI.backgroundColor = Color.white;
                
            
            onContent?.Invoke();
                
            EditorGUILayout.EndVertical();
        }

        private GUIStyle GetContentRectStyle()
        {
            if (_contentRectStyle != null)
                return _contentRectStyle;

            _contentRectStyle = new GUIStyle("box")
            {
                margin = new RectOffset(0, 1, 1, 0),
            };
            
            return _contentRectStyle;
        }

        #endregion
        
        private void DrawGrid(Rect rect)
        {
            Color gridColor = new Color(1, 1, 1, 0.03f); 
            Handles.BeginGUI();
    
            Handles.color = gridColor;
            
            for (float i = rect.y; i < rect.yMax; i += 15)
            {
                Handles.DrawLine(new Vector2(rect.x, i), new Vector2(rect.xMax, i));
            }

            for (float i = rect.x; i < rect.xMax; i += 15)
            {
                Handles.DrawLine(new Vector2(i, rect.y), new Vector2(i, rect.yMax));
            }

            Handles.EndGUI();
        }
        
        
        private void HandleElementMenu(Rect rect, ITestToolElement element)
        {
            Event current = Event.current;
        
            if (current.type == EventType.ContextClick && rect.Contains(current.mousePosition))
            {
                GenericMenu menu = new GenericMenu();
            
                menu.AddItem(new GUIContent("Script"), false, () => OpenScript(element));
            
                menu.ShowAsContext();
            
                current.Use();
            }
        }
        
        
        private void OpenScript(ITestToolElement element)
        {
            var type = element.GetType();
            
            string[] guids = AssetDatabase.FindAssets($"{type.Name} t:MonoScript");
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                if (script != null && script.GetClass() == type)
                {
                    AssetDatabase.OpenAsset(script);
                    return;
                }
            }
        }
    }
}

