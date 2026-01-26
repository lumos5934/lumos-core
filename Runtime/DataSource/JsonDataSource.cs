using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    [CreateAssetMenu(fileName = "Json_DataSource", menuName = "SO/Data Source/Json")]
    public class JsonDataSource : BaseDataSource
    {
        [InfoBox("Path : C:\\Users\\%UserName%\\AppData\\LocalLow\\%Company%\\%Project%\\FileName.json")]
        [SerializeField] private string _fileName;
      
        private bool _isInitialized;
        private string _path;
        private JsonSerializerSettings _settings;

        public override async UniTask WriteAsync<T>(T data)
        {
            if (!_isInitialized) Init();
            
            JObject root = LoadRoot();

            root[data.GetType().Name] = JToken.FromObject(data, JsonSerializer.Create(_settings));

            string json = root.ToString();
            
            await File.WriteAllTextAsync(_path, json);
            
            DebugUtil.Log(_path, "PATH");
        }

        public override UniTask<T> ReadAsync<T>()
        {
            if (!_isInitialized) Init();
            
            if (!File.Exists(_path))  
                return UniTask.FromResult<T>(default);
            
            JObject root = LoadRoot();
            string key = typeof(T).Name;

            if (!root.TryGetValue(key, out JToken token)) return default;
              
            return UniTask.FromResult(token.ToObject<T>());
        }

        private void Init()
        {
            string path = Application.persistentDataPath;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!_fileName.EndsWith(".json"))
                _fileName += ".json";
            
            _path = Path.Combine(path, _fileName);

            if (!File.Exists(_path))
                File.WriteAllText(_path, "{}");

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