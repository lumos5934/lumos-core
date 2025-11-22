using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LumosLib
{
    public abstract class BaseDataManager : MonoBehaviour, IPreInitializer, IDataManager
    {
        #region >--------------------------------------------------- PROPERTIE
        
        
        public int PreInitOrder => (int)PreInitializeOrder.Data;
        
        
        #endregion
        #region >--------------------------------------------------- FIELD
        
        
        protected Dictionary<Type, Dictionary<int, BaseData>> _loadDatas = new();
        
        
        #endregion
        #region >--------------------------------------------------- UNITY

        
        protected virtual void Awake()
        {
            GlobalService.Register<IDataManager>(this);
            
            DontDestroyOnLoad(gameObject);
        }
        

        #endregion
        #region >--------------------------------------------------- INIT

        
        public abstract IEnumerator InitAsync();
        
        
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