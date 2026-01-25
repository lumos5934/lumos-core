using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace LumosLib
{
    public class StateMachine
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public Type CurStateType => _curState?.GetType();

        
        #endregion
        #region >--------------------------------------------------- FIELD

        
        private Dictionary<Type, IState> _stateDict;
        private IState _curState;

        
        #endregion
        #region >--------------------------------------------------- EVENT
        
        
        public event UnityAction<Type> OnExit;
        public event UnityAction<Type> OnEnter;
        
        
        #endregion    
        #region >--------------------------------------------------- CORE


        public StateMachine(IState[] states)
        {
            _stateDict = states.ToDictionary(state => state.GetType(),  state => state);
        }

        
        public void Update()
        {
            _curState?.Update();
        }


        public void SetState<T>() where T : IState
        {
            if (!_stateDict.TryGetValue(typeof(T), out var newState) ||
                _curState == newState) return;

            var prevState = _curState;
            
            prevState?.Exit();
            OnExit?.Invoke(prevState?.GetType());
            
            _curState = newState;
            
            _curState?.Enter();
            OnEnter?.Invoke(_curState?.GetType());
        }
        
        
        #endregion
    }
}
