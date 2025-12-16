using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    public class DataTableManager : MonoBehaviour, IPreInitializable, IDataTableManager
    {
        #region >--------------------------------------------------- FIELD
        
       
        [SerializeField] private DataTableType _dataTableType;
        [SerializeField, HideIf("_dataTableType", DataTableType.None)] private string _tablePath;
        
        private Dictionary<Type, Dictionary<int, BaseData>> _loadDatas = new();
        
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public IEnumerator InitAsync(Action<bool> onComplete)
        {
            var tableLoader = GetLoader();
            if (tableLoader == null)
            {
                onComplete?.Invoke(true);
                yield break;
            }

            tableLoader.SetPath(_tablePath);

            yield return tableLoader.LoadJsonAsync();
            if (tableLoader.Json == "")
            {
                onComplete?.Invoke(true);
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
                    onComplete?.Invoke(false);
                }
            }
            
            GlobalService.Register<IDataTableManager>(this);
            DontDestroyOnLoad(gameObject);
            
            onComplete?.Invoke(true);
        }
         
        
        #endregion
        #region >--------------------------------------------------- GET

        
        private BaseDataLoader GetLoader()
        {
            return _dataTableType switch
            {
                DataTableType.GoogleSheet => new GoogleSheetLoader(),
                _ => null,
            };
        }

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
                if (dict.TryGetValue(id, out var value))
                {
                    return value as T;
                }
            }

            DebugUtil.LogError($" haven't data '{typeof(T).Name}' ", " GET FAIL ");
            return null;
        }
        
        
        #endregion
    }
}