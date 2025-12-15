using UnityEngine;

namespace LumosLib
{
    public abstract class BaseObjective
    {
        public string key;
        public abstract void Evaluate<T>(T targetEvent) where T : IGameEvent;
        public abstract bool IsComplete();

        public BaseObjective(string key)
        {
            this.key = key;
        }
    }
}
