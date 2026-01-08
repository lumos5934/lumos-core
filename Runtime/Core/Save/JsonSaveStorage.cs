using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace LumosLib
{
    public class JsonSaveStorage : BaseSaveStorage
    {
        [SerializeField] private string _folderPath;
        [SerializeField] private string _fileName;
        
        private bool _isInitialized;
        private string _path;
        private JsonSerializerSettings _settings;

        public override async UniTask SaveAsync<T>(T data) 
        {
            if(!_isInitialized) Init(_folderPath, _fileName);
            
            JObject root = LoadRoot();

            root[data.GetType().Name] = JToken.FromObject(data, JsonSerializer.Create(_settings));

            string json = root.ToString();
            
            await File.WriteAllTextAsync(_path, json);
        }

        public override UniTask<T> LoadAsync<T>()
        {
            if (!File.Exists(_path))  
                return UniTask.FromResult<T>(default);
            
            JObject root = LoadRoot();
            string key = typeof(T).Name;

            if (!root.TryGetValue(key, out JToken token)) return default;
              
            return UniTask.FromResult(token.ToObject<T>());
        }

        private void Init(string folderPath, string fileName)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            _path = Path.Combine(folderPath, fileName);

            
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, "{}");
            }

            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
            
            _isInitialized = true;
        }
        
        private JObject LoadRoot()
        {
            if (!File.Exists(_path)) return new JObject();

            string json = File.ReadAllText(_path);
            
            if (string.IsNullOrEmpty(json)) return new JObject();

            return JObject.Parse(json);
        }
        
    }
}