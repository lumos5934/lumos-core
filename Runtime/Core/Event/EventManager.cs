using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LumosLib
{
    public class EventManager : MonoBehaviour, IPreInitializable, IEventManager
    {
        #region >--------------------------------------------------- FIELD

        
        private readonly Dictionary<Type, Delegate> _events = new();

        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            GlobalService.Register<IEventManager>(this);
            return UniTask.FromResult(true);
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE

        
        public void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (_events.TryGetValue(typeof(T), out var del))
                _events[typeof(T)] = Delegate.Combine(del, handler);
            else
                _events[typeof(T)] = handler;
        }

        
        public void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (_events.TryGetValue(typeof(T), out var del))
                _events[typeof(T)] = Delegate.Remove(del, handler);
        }

        public void Publish<T>(T evt) where T : IGameEvent
        {
            if (_events.TryGetValue(typeof(T), out var del))
                ((Action<T>)del)?.Invoke(evt);
        }
        
        
        #endregion

      
    }
}