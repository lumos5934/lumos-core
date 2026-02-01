using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LumosLib
{
    public class StateMachine
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public IState CurState => _curState;

        
        #endregion
        #region >--------------------------------------------------- FIELD

        
        private Dictionary<Type, IState> _stateDict = new();
        private IState _curState;

        
        #endregion
        #region >--------------------------------------------------- EVENT
        
        
        public event UnityAction<IState> OnExit;
        public event UnityAction<IState> OnEnter;
        
        
        #endregion    
        #region >--------------------------------------------------- CORE

        public void Register(IState state)
        {
            _stateDict[state.GetType()] = state;
        }
        
        public void Update()
        {
            _curState?.Update();
        }

        public void Transition<T>() where T : IState
        {
            if (!_stateDict.TryGetValue(typeof(T), out var newState) ||
                _curState == newState) return;

            var prevState = _curState;

            if (prevState != null)
            {
                prevState.Exit();
                OnExit?.Invoke(prevState);
            }
          
            _curState = newState;
            _curState.Enter();
            OnEnter?.Invoke(_curState);
        }
        
        
        #endregion
    }
}
