using System;
using System.Collections.Generic;
using System.Linq;
using BansheeGz.BGDatabase;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LumosLib
{
    public class DatabaseManager : MonoBehaviour, IPreInitializable, IDatabaseManager
    {
        #region >--------------------------------------------------- FIELD
        
       
        private Dictionary<Type, Dictionary<int, BaseBGData>> _dataDict = new();
        
        
        #endregion
        #region >--------------------------------------------------- INIT

        
        public UniTask<bool> InitAsync()
        {
            GlobalService.Register<IDatabaseManager>(this);
            return UniTask.FromResult(true);
        }
         
        
        #endregion
        #region >--------------------------------------------------- REGISTER


        private void Register<T>() where T : BaseBGData
        {
            var type = typeof(T);
            var meta = BGRepo.I[type.Name];
            
            _dataDict[type] = new Dictionary<int, BaseBGData>();
            
            foreach (var entity in  meta.EntitiesToList())
            {
                var instance = (BaseBGData)Activator.CreateInstance(typeof(T), entity);
                
                _dataDict[type][instance.TableID] = instance;
            }
        }
        
        
        #endregion
        #region >--------------------------------------------------- GET


        public List<T> GetAll<T>() where T : BaseBGData
        {
            var type = typeof(T);
            
            if (!_dataDict.ContainsKey(type))
            {
                Register<T>();
            }
            
            return _dataDict[type].Values.Cast<T>().ToList();
        }
        
        public T Get<T>(int tableID) where T : BaseBGData
        {
            var type = typeof(T);
            
            if (!_dataDict.ContainsKey(type))
            {
                Register<T>();
            }
            
            return _dataDict[type][tableID] as T;
        }
        
        
        #endregion
    }
}