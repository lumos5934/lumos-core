using LLib.Core;
using UnityEngine;

public class TemplateSceneManager : BaseSceneManager
{
    public UIBaseTestSceneManager UIBase => uiBaseTestSceneManager;
    
    
    [SerializeField] private UIBaseTestSceneManager uiBaseTestSceneManager;
    
    protected override void Init()
    {
        uiBaseTestSceneManager.Init();
    }
}