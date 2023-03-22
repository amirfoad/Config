using Newtonsoft.Json;

namespace Configuration
{
    public class ConfigRoot
    {
        public string ApplicationName { get; set; }

        public Version Version { get; set; }

        public List<Config> Configs { get; private set; }

        public ConfigRoot()
        {
            Configs = new List<Config>();
        }

        public ConfigRoot(string applicationName, Version version) : this()
        {
            ApplicationName = applicationName;
            Version = version;
        }

        public bool Add(Config config)
        {
            try
            {
                if (Configs.Any(x => x.Key == config.Key))
                    return false;

                Configs.Add(config);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add<T>(Config<T> config)
        {
            try
            {
                if (Configs.Any(x => x.Key == config.Key))
                    return false;

                Config configToAdd = new()
                {
                    Key = config.Key,
                    Value = JsonConvert.SerializeObject(config.Value),
                    DefaultValue = JsonConvert.SerializeObject(config.Value),
                };

                Configs.Add(configToAdd);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Config<T> Get<T>(string key)
        {
            if (Configs.Any(t => t.Key == key))
            {
                var c = Configs.FirstOrDefault(t => t.Key == key);
                var config = new Config<T>
                {
                    Key = key,
                    DefaultValue = JsonConvert.DeserializeObject<T>(c.DefaultValue),
                    Value = JsonConvert.DeserializeObject<T>(c.Value)
                };
                return config;
            }

            return null;
        }

        public Config Get(string key)
        {
            if (Configs.Any(t => t.Key == key))
            {
                var config = Configs.FirstOrDefault(t => t.Key == key);

                return config;
            }

            return null;
        }

        public string GetValue(string key)
        {
            if (Configs.Any(c => c.Key == key))
                return Configs.SingleOrDefault(x => x.Key == key).Value;
            return string.Empty;
        }

        public T GetValue<T>(string key)
        {
            if (Configs.Any(c => c.Key == key))
            {
                var value = Configs.SingleOrDefault(x => x.Key == key).Value;
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public bool Update(string key, string value)
        {
            var result = false;

            if (Configs.Any(c => c.Key == key))
            {
                var data = Configs.Single(x => x.Key == key);
                Configs.Remove(data);
                data.Value = value;
                Configs.Add(data);
                result = true;
            }

            return result;
        }

        public bool Update<T>(string key, T value)
        {
            var result = false;

            if (Configs.Any(c => c.Key == key))
            {
                var data = Configs.Single(x => x.Key == key);
                Configs.Remove(data);
                data.Value = JsonConvert.SerializeObject(value);
                Configs.Add(data);
                result = true;
            }

            return result;
        }

        public bool Delete(string key)
        {
            var result = false;

            if (Configs.Any(c => c.Key == key))
            {
                var data = Configs.Single(x => x.Key == key);
                Configs.Remove(data);
                result = true;
            }

            return result;
        }
    }
}