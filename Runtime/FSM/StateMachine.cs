using System;
using System.Collections.Generic;
using System.Linq;
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
        public event UnityAction<IState> OnUpdate;
        
        
        #endregion    
        #region >--------------------------------------------------- CORE



        public StateMachine(IState[] states)
        {
            _stateDict = states.ToDictionary(state => state.GetType(),  state => state);
        }
        
        public void Update()
        {
            if (CurState != null)
            {
                CurState.Update();
                OnUpdate?.Invoke(CurState);
            }
        }


        public T GetState<T>() where T : IState
        {
            if(_stateDict.TryGetValue(typeof(T), out var state))
            {
                return (T)state;
            }
            
            return default;
        }
        
        public void SetState<T>() where T : IState
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
}
