using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : SingletonGlobal<GameManager>
{
    #region  >--------------------------------------------------- PROPERTIES
    
    
    public bool IsInitialized { get; private set; }

    
    #endregion
    #region  >--------------------------------------------------- FIEDLS
    
    
    private Dictionary<Type ,GameManagerComponent> _components = new();
    
    
    #endregion
    #region  >--------------------------------------------------- UNITY

    protected override void Awake()
    {
        base.Awake();
        
        Init();
    }

    #endregion
    #region  >--------------------------------------------------- INIT
    
    
    public void Init()
    {
        if (IsInitialized) return;
        
        var container = Resources.Load<GameManagerComponentsSO>(Constant.GAME_MGR_COMPONENTS_PATH);

        if (container == null)
        {
            DebugUtil.LogError(" CONTAINER PATH ", "INIT FAIL");
            return;
        }
        
        StartCoroutine(InitComponents(container));
    }
    
    private IEnumerator InitComponents(GameManagerComponentsSO container)
    {
        var orderPrefabs = container.ComponentPrefabs.OrderBy( manager => manager.Order ).ToArray();
        
        Debug.Log(orderPrefabs.Length);
        
        for (int i = 0; i < orderPrefabs.Length; i++)
        {
            var component = Instantiate(orderPrefabs[i], transform);
            var type = component.GetType();
            
            if (_components.ContainsKey(type))
            {
                DebugUtil.LogError(" INIT FAIL ", $" { component.GetType() } ");
                yield break;
            }
            
            component.Init();
            yield return new WaitUntil( () => component.IsInitialized );
            
            _components[type] = component;
            DebugUtil.Log(" INIT COMPLETE ", $" { type } ");
        }
        
        IsInitialized = true;
        DebugUtil.Log("", " All Managers INIT COMPLETE ");
    }
    
    
    #endregion
    #region  >--------------------------------------------------- GET
    
    
    public T Get<T>() where T : GameManagerComponent
    {
        if (_components.TryGetValue(typeof(T), out var component))
        {
            return component as T;
        }

        return null;
    }
    
    
    #endregion
    #region  >--------------------------------------------------- REGISTER

    
    public void Register<T>(T component) where T : GameManagerComponent
    {
        if (_components.ContainsKey(component.GetType()))
        {
            DebugUtil.LogWarning("Duplicate component registration ", component.GetType().ToString());
        }
        else
        {
            _components[component.GetType()] = component;
        }
    }
    
    public void Unregister<T>(T component) where T : GameManagerComponent
    {
        _components.Remove(component.GetType());
    }
    
    
    #endregion
 
}