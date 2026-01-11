using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lumos
{
    public class SaveManager : MonoBehaviour, IPreInitializable, ISaveManager
    {
        #region >--------------------------------------------------- FIELD

        
        [SerializeField] private BaseDataSource _saveDataSource;
        
        private readonly Dictionary<Type, object> _saveDataDict = new();
        
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            GlobalService.Register<ISaveManager>(this);
            return UniTask.FromResult(true);
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE
        
        
        public async UniTask SaveAsync<T>(T data)
        {
            if (_saveDataSource == null)
            {
                DebugUtil.LogWarning("data source not found", "");
                return;
            }
            
            var type = typeof(T);

            if (!_saveDataDict.ContainsKey(type))
            {
                _saveDataDict[typeof(T)] = data;
            }

            try
            {
                await _saveDataSource.WriteAsync(data);
                DebugUtil.Log("SAVE", "SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            
            if (_saveDataDict.ContainsKey(typeof(T)))
            {
                try
                {
                    var result = await _saveDataSource.ReadAsync<T>();
                    DebugUtil.Log("LOAD", "SUCCESS");

                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            DebugUtil.LogWarning("LOAD : save data not found", "FAIL");
            return default;
        }
        
        
        #endregion
    }
}


