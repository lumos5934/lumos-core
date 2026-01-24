using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace LumosLib
{
    public class StateMachine
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public Type CurStateType => _curState.GetType();

        
        #endregion
        #region >--------------------------------------------------- FIELD

        
        private Dictionary<Type, IState> _stateDict;
        private IState _curState;

        
        #endregion
        #region >--------------------------------------------------- EVENT
        
        
        public event UnityAction<Type> OnExit;
        public event UnityAction<Type> OnEnter;
        public event UnityAction<Type> OnUpdate;
        
        
        #endregion    
        #region >--------------------------------------------------- CORE



        public StateMachine(IState[] states)
        {
            _stateDict = states.ToDictionary(state => state.GetType(),  state => state);
        }

        
        public void Update()
        {
            if (_curState != null)
            {
                _curState.Update();
                OnUpdate?.Invoke(CurStateType);
            }
        }


        public void SetState<T>() where T : IState
        {
            _stateDict.TryGetValue(typeof(T), out var newState);

            if (newState == null) return;

            if (_curState != null)
            {
                _curState.Exit();
                OnExit?.Invoke(CurStateType);
            }

            _curState = newState;

            if (_curState != null)
            {
                _curState.Enter();
                OnEnter?.Invoke(CurStateType);
            }
        }
        
        
        #endregion
    }
}
