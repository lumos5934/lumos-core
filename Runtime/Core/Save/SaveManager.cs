using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    public class SaveManager : MonoBehaviour, IPreInitializable, ISaveManager
    {
        #region >--------------------------------------------------- FIELD

        
        [SerializeField] private SaveStorageType _saveType;
        [SerializeField, ShowIf("_saveType", SaveStorageType.Json)] private string _folderPath;
        [SerializeField, ShowIf("_saveType", SaveStorageType.Json)] private string _fileName;
        private ISaveStorage _saveStorage;
        
        private readonly Dictionary<Type, ISaveData> _saveDataDict = new();
        
        
        #endregion
        #region >--------------------------------------------------- INIT
        
        
        public UniTask<bool> InitAsync()
        {
            switch (_saveType)
            {
                case SaveStorageType.Json:
                    _saveStorage = new JsonSaveStorage(_folderPath, _fileName);
                    break;
            }

            GlobalService.Register<ISaveManager>(this);
            return UniTask.FromResult(true);
        }
        
        
        #endregion
        #region >--------------------------------------------------- CORE
        
        
        public async Task SaveAsync<T>(T data) where T : ISaveData
        {
            if (_saveStorage == null) return;
            
            var type = typeof(T);

            if (!_saveDataDict.ContainsKey(type))
            {
                _saveDataDict[typeof(T)] = data;
            }
            
            await _saveStorage.SaveAsync(data);
        }
        
        public async Task<T> LoadAsync<T>() where T : ISaveData
        {
            if (_saveStorage == null) return default;
            
            if (_saveDataDict.ContainsKey(typeof(T)))
            {
                return await _saveStorage.LoadAsync<T>();
            }

            return default;
        }
        
        
        #endregion
    }
}


