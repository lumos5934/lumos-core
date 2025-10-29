using System.Collections;
using UnityEngine;

public abstract class GameSceneManager : GameManagerComponent
{
    #region  --------------------------------------------------- PROPERTIES


    public override int Order => 0;
    
    
    #endregion
    #region  --------------------------------------------------- UNITY

    
    protected virtual void Awake()
    {
        StartCoroutine(InitAsync());
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance?.Unregister(this);
    }
    
    
    #endregion
    #region  --------------------------------------------------- INIT
    
    
    private IEnumerator InitAsync()
    {
        var gameMgr = GameManager.Instance;
        
        yield return new WaitUntil(() => gameMgr.IsInitialized);

        gameMgr.Register(this);
        
        Init();
    }
    
    
    #endregion
}