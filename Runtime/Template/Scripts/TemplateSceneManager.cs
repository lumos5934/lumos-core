using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TemplateSceneManager : GameSceneManager
{
    public override bool IsInitialized { get; protected set; }
    public override void Init()
    {
        IsInitialized = true;
    }
}