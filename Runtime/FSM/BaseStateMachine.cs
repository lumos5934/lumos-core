using System;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class BaseStateMachine
{
    #region >--------------------------------------------------- PROPERTIE
    
    
    public BaseState CurState => _curState;

    private Dictionary<Type, BaseState> States
    {
        get
        {
            if (_stateDict == null)
            {
                _stateDict = new();
                
                var states = InitStates();
                foreach (var state in states)
                {
                    _stateDict[state.GetType()] = state;
                }
            }

            return _stateDict;
        }
    }
    
    
    #endregion
    #region >--------------------------------------------------- FIELD

    
    private Dictionary<Type, BaseState> _stateDict;
    private BaseState _curState;

    
    #endregion
    #region >--------------------------------------------------- EVENT
    
    
    public event UnityAction<BaseState> OnExit;
    public event UnityAction<BaseState> OnEnter;
    public event UnityAction<BaseState> OnUpdate;
    
    
    #endregion    
    #region >--------------------------------------------------- CORE

    
    protected abstract BaseState[] InitStates();
    public virtual void Update()
    {
        if (CurState != null)
        {
            CurState.Update();
            OnUpdate?.Invoke(CurState);
        }
    }

    
    public T GetState<T>() where T : BaseState
    {
        if(States.TryGetValue(typeof(T), out var state))
        {
            return state as T;
        }
        
        return null;
    }
    
    public void SetState<T>() where T : BaseState
    {
        var newState = GetState<T>();
        if (newState == null) return;

        if (CurState != null)
        {
            CurState.Exit();
            OnExit?.Invoke(CurState);
        }

        _curState = newState;

        if (CurState != null)
        {
            CurState.Enter();
            OnEnter?.Invoke(CurState);
        }
    }
    
    
    #endregion
}