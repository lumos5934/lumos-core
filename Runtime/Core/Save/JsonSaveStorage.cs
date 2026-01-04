using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LumosLib
{
    public class JsonSaveStorage : ISaveStorage
    {
        private readonly string _path;
        private readonly JsonSerializerSettings _settings;

        public JsonSaveStorage(string folderPath, string fileName)
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
        }

        public async Task SaveAsync<T>(T data) where T : ISaveData
        {
            JObject root = LoadRoot();

            root[data.GetType().Name] = JToken.FromObject(data, JsonSerializer.Create(_settings));

            string json = root.ToString();
            
            await File.WriteAllTextAsync(_path, json);
        }

        public async Task<T> LoadAsync<T>() where T : ISaveData
        {
            if (!File.Exists(_path)) return default;
            
            JObject root = LoadRoot();
            string key = typeof(T).Name;

            if (!root.TryGetValue(key, out JToken token)) return default;
              
            return token.ToObject<T>();
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