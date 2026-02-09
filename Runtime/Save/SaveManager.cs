using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LumosLib
{
    public class SaveManager : MonoBehaviour, IPreInitializable, ISaveManager
    {
        [SerializeField] private BaseDataSource _saveDataSource;
        
        private readonly Dictionary<Type, object> _saveDataDict = new();
        
        public UniTask<bool> InitAsync()
        {
            GlobalService.Register<ISaveManager>(this);
            return UniTask.FromResult(true);
        }
        
        
        public async UniTask SaveAsync<T>(T data)
        {
            if (_saveDataSource == null)
            {
                DebugUtil.LogWarning("SAVE : data source not found", "");
                return;
            }

            var type = typeof(T);

            _saveDataDict[type] = data;

            try
            {
                await _saveDataSource.WriteAsync(data);
                DebugUtil.Log("SAVE", $"SUCCESS ({type.Name})");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }
        
        public async UniTask<T> LoadAsync<T>()
        {
            if (_saveDataSource == null)
            {
                DebugUtil.LogWarning("data source not found", "");
                return default;
            }
            
            if (_saveDataDict.TryGetValue(typeof(T), out var cached))
                return (T)cached;
            
            var type = typeof(T);
            
            try
            {
                var result = await _saveDataSource.ReadAsync<T>();

                if (!EqualityComparer<T>.Default.Equals(result, default))
                {
                    _saveDataDict[type] = result;
                    DebugUtil.Log("LOAD", $"SUCCESS ({type.Name})");
                }
                else
                {
                    DebugUtil.LogWarning("LOAD : save data not found", type.Name);
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }
    }
}


