using System;

namespace Lumos.Core
{
    public interface IEventManager
    {
        void Subscribe<T>(Action<T> listener) where T : IGameEvent; 
        void Unsubscribe<T>(Action<T> listener) where T : IGameEvent;
        void Publish<T>(T evt) where T : IGameEvent;
    }
}
