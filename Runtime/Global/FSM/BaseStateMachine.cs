using System;
using System.Collections.Generic;

public abstract class BaseStateMachine<TState, T> where T : IState where TState : Enum
{
    #region >--------------------------------------------------- FIELD

    
    public T CurState { get; private set; }
    private Dictionary<TState, T> _states = new();

    
    #endregion
    #region >--------------------------------------------------- METHODS


    public virtual void Update()
    {
        CurState?.Update();
    }

    protected void AddState(TState type, T state) =>  _states[type] = state;
    
    public T GetState(TState type)
    {
        if (_states.TryGetValue(type, out T state))
        {
            return state;
        }
        
        return default; 
    }
    
    public void SetState(TState type)
    {
        var newState = GetState(type);
        if (newState == null) return;
        
        CurState?.Exit();
        CurState = newState;
        CurState.Enter();
    }
    
    
    #endregion
}