using Lumos.DevPack;
using UnityEngine;

public class TemplateSceneManager : BaseSceneManager
{
    public UITestSceneManager UI => _uiTestSceneManager;
    
    
    [SerializeField] private UITestSceneManager _uiTestSceneManager;
    
    protected override void Init()
    {
        _uiTestSceneManager.Init();
    }
}