using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LumosLib
{
    public class ObjectiveManager : MonoBehaviour, IPreInitializer
    {
        public int PreInitOrder => 0;

        private Dictionary<string, BaseObjective> _objectiveDict = new();


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            GlobalService.Register(this);
        }

        public IEnumerator InitAsync()
        {
            yield break;
        }

        public void Register<T>(T objective) where T : BaseObjective
        {
            Debug.Log(objective);
            _objectiveDict[objective.key] = objective;
        }

        public void Unregister<T>(T objective) where T : BaseObjective
        {
            _objectiveDict.Remove(objective.key);
        }

        public void Evaluate(IGameEvent gameEvent)
        {
            List<BaseObjective> clearObjectives = new();
            
            foreach (var objKvp in _objectiveDict)
            {
                objKvp.Value.Evaluate(gameEvent);

                if (objKvp.Value.IsComplete())
                {
                    GlobalService.GetInternal<IEventBus>().Publish(objKvp.Value);
                    
                    Debug.Log(2);
                    clearObjectives.Add(objKvp.Value);
                }
            }

            foreach (var objective in clearObjectives)
            {
                Unregister(objective);
            }
        }
    }
}

