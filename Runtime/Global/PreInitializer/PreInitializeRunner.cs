using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Lumos.DevKit
{
    public class PreInitializeRunner : MonoBehaviour
    {
        public void Run()
        {
            StartCoroutine(InitAsync());
        }
        
        private IEnumerator InitAsync()
        {
            var startTime = Time.realtimeSinceStartup;
            
            var targetAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name == "Assembly-CSharp" 
                            || a.GetName().Name.StartsWith(GetType().Assembly.GetName().Name))
                .ToArray();
            
            //Get IPreInitialize Types
            var preIniTypes = targetAssemblies
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
                })
                .Where(t => typeof(IPreInitialize).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();
            
            
            DebugUtil.Log($" IPreInitialize ( {( Time.realtimeSinceStartup - startTime) * 1000f:F3} ms )", $" GET ASSEMBLIES ");
            
            
            var preInstances = preIniTypes
                // Create All Instances
                .Select(type =>
                {
                    IPreInitialize instance = null;

                    try
                    {
                        if (typeof(MonoBehaviour).IsAssignableFrom(type))
                        {
                            var go = new GameObject(type.Name);
                            go.AddComponent(type);
                            instance = go.GetComponent<IPreInitialize>();
                        }
                        else
                        {
                            instance = (IPreInitialize)Activator.CreateInstance(type);
                        }
                    }
                    catch (Exception e)
                    {
                        _ = e;
                        DebugUtil.LogWarning($"{type.Name}", " FAIL CREATE INSTANCE ");
                    }

                    return instance;
                })
                .Where(x => x != null)
                
                // Grouped ID
                .GroupBy(x => x.PreID)
                
                // Get Highest Order In Grouped ID
                .Select(g =>
                {
                    var selected = g.OrderByDescending(x => x.PreInitOrder).First();

                    foreach (var duplicate in g.Where(x => x != selected))
                    {
                        if (duplicate is MonoBehaviour mono)
                        {
                            Destroy(mono.gameObject);
                        }
                    }

                    return selected;
                })
                .OrderBy(x => x.PreInitOrder)
                .ToList();

            
            //Initialize
            for (int i = 0; i < preInstances.Count; i++)
            {
                var initStartTime = Time.realtimeSinceStartup;
                var target = preInstances[i];
                
                target.PreInit();

                if (!target.PreInitialized)
                {
                    yield return new WaitUntil(() => target.PreInitialized); 
                }
                
                DebugUtil.Log($" { target.GetType().Name } ( {(Time.realtimeSinceStartup - initStartTime) * 1000f:F3} ms )", $" INITIALIZED ");
            }


            var totalElapsed = Time.realtimeSinceStartup - startTime;
            DebugUtil.Log($" All ( {totalElapsed * 1000f:F3} ms )", " INITIALIZED ");
            
            PreInitializer.SetInitialized(true);
            
            Destroy(gameObject);
        }
    }
}