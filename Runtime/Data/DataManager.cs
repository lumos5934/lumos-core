using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace LumosLib
{
    public class DataManager : MonoBehaviour, IPreInitialize, IDataManager
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public int PreInitOrder => (int)PreInitializeOrder.Data;
        public bool PreInitialized { get; private set; }
        
        
        #endregion
        #region >--------------------------------------------------- FIELD
        
        
        private Dictionary<Type, Dictionary<int, BaseData>> _loadDatas = new();
        
        
        #endregion
        #region >--------------------------------------------------- UNITY

        
        private void Awake()
        {
            StartCoroutine(LoadDataAsync());
            
            DontDestroyOnLoad(gameObject);
            
            Global.Register<IDataManager>(this);
        }
        

        #endregion
        #region >--------------------------------------------------- INIT
        
        
         private IEnumerator LoadDataAsync()
        {
            var tableLoader = GetLoader();
            if (tableLoader == null)
            {
                PreInitialized = true;
                yield break;
            }

            tableLoader.SetPath(PreInitializer.Instance.Config.TablePath);

            yield return tableLoader.LoadJsonAsync();
            if (tableLoader.Json == "")
            {
                PreInitialized = true;
                yield break;
            }
           
            
            var allSheets = JsonConvert.DeserializeObject<Dictionary<string, object[]>>(tableLoader.Json);

            var targetAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => 
                    a.GetName().Name == "Assembly-CSharp" || 
                    a.GetName().Name.StartsWith(GetType().Assembly.GetName().Name))
                .ToArray();
            
            var dataTypes = targetAssemblies
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
                })
                .Where(t => typeof(BaseData).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            
            foreach (var type in dataTypes)
            {
                if (allSheets.TryGetValue(type.Name, out var sheetJson))
                {
                    string sheetJsonStr = JsonConvert.SerializeObject(sheetJson);
                    var listType = typeof(List<>).MakeGenericType(type);
                    var list = (IList)JsonConvert.DeserializeObject(sheetJsonStr, listType);

                    var dict = new Dictionary<int, BaseData>();
                    
                    foreach (var item in list)
                    {
                        var data = (BaseData)item;
                        dict[data.ID] = data;
                    }
                    
                    _loadDatas[type] = dict;
                }
                else
                {
                    DebugUtil.LogWarning($" haven't sheet '{type.Name}'" , " LOAD FAIL ");
                }
            }
            
            PreInitialized = true;
        }
         
        
        #endregion
        #region >--------------------------------------------------- GET

        
        public List<T> GetAll<T>() where T : BaseData
        {
            if (_loadDatas.TryGetValue(typeof(T), out var dict))
            {
                return dict.Values.Cast<T>().ToList();
            }

            DebugUtil.LogError($" haven't data '{typeof(T).Name}' ", " GET FAIL ");
            return null;
        }
        
        public T Get<T>(int id) where T : BaseData
        {
            if (_loadDatas.TryGetValue(typeof(T), out var dict))
            {
                if (dict.ContainsKey(id))
                {
                    return dict[id] as T;
                }
            }

            DebugUtil.LogError($" haven't data '{typeof(T).Name}' ", " GET FAIL ");
            return null;
        }
        
        private BaseDataLoader GetLoader()
        {
            return PreInitializer.Instance.Config.SelectedTableType switch
            {
                PreInitializeConfigSO.TableType.GoogleSheet => new GoogleSheetLoader(),
                _ => null,
            };
        }
        

        #endregion
    }
}