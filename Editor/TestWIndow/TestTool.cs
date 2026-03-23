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
        private GUIStyle TitleStyle
        {
            get
            {
                if (_titleStyle == null)
                {
                    _titleStyle = new GUIStyle(EditorStyles.boldLabel);
                    _titleStyle.fontSize = 25;
                    _titleStyle.fontStyle = FontStyle.Bold;
                    _titleStyle.alignment = TextAnchor.MiddleCenter;

                    _titleStyle.hover.background = null;
                    _titleStyle.active.background = null;
                }

                return _titleStyle;
            }
        }
        
        private GUIStyle _titleShadowStyle;
        private GUIStyle TitleShadowStyle
        {
            get
            {
                if (_titleShadowStyle == null)
                {
                    _titleShadowStyle = new GUIStyle(EditorStyles.boldLabel);
                    _titleShadowStyle.fontSize = 25;
                    _titleShadowStyle.fontStyle = FontStyle.Bold;
                    _titleShadowStyle.alignment = TextAnchor.MiddleCenter;
                    _titleShadowStyle.hover.background = null;
                    _titleShadowStyle.active.background = null;
                }

                return _titleShadowStyle;
            }
        }
        
        private GUIStyle _mainRectStyle;
        private GUIStyle MainRectStyle
        {
            get
            {
                if (_mainRectStyle == null)
                {
                    _mainRectStyle = new GUIStyle("box")
                    {
                        padding = new RectOffset(0, 0, 0, 0),
                        margin = new RectOffset(0, 0, 0, 0),
                        stretchWidth = true,
                        stretchHeight = true,
                    };
                }
            
                return _mainRectStyle;
            }
        }
        private GUIStyle _sideRectStyle;
        private GUIStyle SideRectStyle
        {
            get
            {
                if (_sideRectStyle == null)
                {
                    _sideRectStyle = new GUIStyle()
                    {
                        padding = new RectOffset(2, 0, 3, 3),
                        margin = new RectOffset(0, 0, 0, 0)
                    };
                }

                return _sideRectStyle;
            }
        }
        private GUIStyle _contentRectStyle;
        private GUIStyle ContentRectStyle
        {
            get
            {
                if (_contentRectStyle == null)
                {
                    _contentRectStyle = new GUIStyle("box")
                    {
                        margin = new RectOffset(0, 1, 1, 0),
                    };
                }
              
                return _contentRectStyle;
            }
        }

        private GUIStyle _btnStyle;
        private GUIStyle BtnStyle
        {
            get
            {
                if (_btnStyle == null)
                {
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
                }

                return _btnStyle;
            }
        }
        private GUIStyle _noticeStyle;
        private GUIStyle NoticeStyle
        {
            get
            {
                if (_noticeStyle == null)
                {
                    _noticeStyle = new GUIStyle()
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 18,
                        fontStyle = FontStyle.Bold,
                        padding = new RectOffset(20, 20, 20, 20),
                    };
                    
                    _noticeStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
                }
            
                return _noticeStyle;
            }
        }

        private LumosLibSettings _settings;
        private LumosLibSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<LumosLibSettings>(nameof(LumosLibSettings));
                }

                return _settings;
            }
        }

        
        
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
            Color titleColor = Settings.NameColor;
            TitleStyle.normal.textColor = titleColor;
            TitleStyle.hover.textColor = titleColor;
            TitleStyle.active.textColor = titleColor;
            TitleStyle.focused.textColor = titleColor;
            
            Color titleShadowColor = Settings.NameShadowColor;
            TitleShadowStyle.normal.textColor = titleShadowColor;
            TitleShadowStyle.hover.textColor = titleShadowColor;
            TitleShadowStyle.active.textColor = titleShadowColor;
            TitleShadowStyle.focused.textColor = titleShadowColor;
            
            BtnStyle.normal.textColor = Settings.ButtonNameNormalColor;
            BtnStyle.hover.textColor = Settings.ButtonNameHoverColor;
            
            
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            {
                DrawTitle();

            
                GUI.backgroundColor = Color.black;
                Rect mainRect = EditorGUILayout.BeginVertical(MainRectStyle);
                {
                    GUI.backgroundColor = Color.white;
                
                    if (!Application.isPlaying)
                    {
                        DrawNotice();
                    }
                    else
                    {
                        if (_selectedElement == null)
                        {
                            DrawGrid(mainRect);
                            EditorGUILayout.Space(3);
                            DrawElementButtons();
                        }
                        else
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                Rect sideRect = EditorGUILayout.BeginVertical(SideRectStyle, 
                                    GUILayout.Width(90), GUILayout.ExpandHeight(true));
                                {
                                    DrawGrid(sideRect);
                                    DrawElementButtons();
                                
                                    EditorGUILayout.EndVertical();
                                }
                
                            
                                GUI.backgroundColor = new Color(0f, 0f, 0f, 0.7f);
                                Rect contentRect = EditorGUILayout.BeginVertical(ContentRectStyle,
                                    GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                                {
                                    GUI.backgroundColor = Color.white;
                                
                                    if (_selectedElement != null)
                                    {
                                        _selectedElement.OnGUI();
                                        HandleElementMenu(contentRect, _selectedElement);
                                    }
                                
                                    EditorGUILayout.EndVertical();
                                }
                            
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
        }


        private void DrawNotice()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    string message = "⚠️ RUNTIME ONLY";
                    GUILayout.Label(message, NoticeStyle, GUILayout.MinWidth(300), GUILayout.MinHeight(100));

                    GUILayout.FlexibleSpace(); // 오른쪽 여백 밀어내기
                    EditorGUILayout.EndHorizontal();
                    {
                        GUILayout.FlexibleSpace(); // 아래쪽 여백 밀어내기
                        EditorGUILayout.EndVertical();
                    }
                }
            }
        }
     
        private void DrawTitle()
        {
            EditorGUILayout.Space(20f);

            GUIContent content = EditorGUIUtility.TrTextContent("TEST TOOL");
            Rect rect = GUILayoutUtility.GetRect(content, TitleStyle);


            var shadowStyle = TitleShadowStyle;
            GUI.Label(new Rect(rect.x + 2.5f, rect.y + 2.5f, rect.width, rect.height), content, shadowStyle);

            
            GUI.Label(rect, content, TitleStyle);

            
            EditorGUILayout.Space(20f);
            
            DrawUnderline(GUILayoutUtility.GetLastRect());
        }
        
        
        private void DrawUnderline(Rect rect)
        {
            float lineY = rect.yMax - 1.5f;
            EditorGUI.DrawRect(new Rect(rect.x, lineY - 1f, rect.width, 1), new Color(0.6f, 0.6f, 0.6f, 0.15f));
            EditorGUI.DrawRect(new Rect(rect.x, lineY, rect.width, 2), new Color(1, 1, 1, 0.15f));
            EditorGUI.DrawRect(new Rect(rect.center.x - 20, lineY - 1f, 40, 1), Settings.UnderLineHighlightShadowColor);
            EditorGUI.DrawRect(new Rect(rect.center.x - 20, lineY, 40, 2), Settings.UnderLineHighlightColor);
        }
       
        private void DrawElementButtons()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                var target = _elements[i];

                GUI.backgroundColor = (_selectedElement == target)
                    ? Settings.ButtonSelectedColor
                    : Settings.ButtonNormalColor;


                var label = target.Title.ToUpper();
                
                if (GUILayout.Button(label, BtnStyle))
                {
                    _selectedElement = _selectedElement == target ? null : target;
                }
                
                Rect rect = GUILayoutUtility.GetLastRect();
                
                if (_selectedElement == target)
                {
                    rect.width -= 2;
                    rect.height -= 2;
                    rect.center -= new Vector2(0f, -1f);

                    var outlineColor = Settings.ButtonHighlightColor;
                    
                    Handles.color = outlineColor;
                    // 내부 색상은 투명하게(new Color(0,0,0,0)), 테두리는 노란색으로
                    Handles.DrawSolidRectangleWithOutline(rect, 
                        new Color(0, 0, 0, 0), 
                        outlineColor);
                }
                
                GUI.backgroundColor = Color.white;
            }
        }
        
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

