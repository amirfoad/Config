using LiteDB;

namespace Configuration.LiteDb
{
    public class ConfigFileStore //: IConfigStore
    {
        private ConfigRoot _root;
        private readonly string _connection;
        private readonly LiteDatabase _liteDb;

        private const string DefaultCollectionName = "Configurations";

        public string ApplicationName { get; private set; }

        public Version Version { get; private set; }

        public ConfigFileStore(string applicationName, Version version, string connection)
        {
            ApplicationName = applicationName;
            Version = version;
            _connection = connection;
            _liteDb = new LiteDatabase(connection);

            Initialize();
        }

        private void Initialize()
        {
            _root = _liteDb.GetCollection<ConfigRoot>(DefaultCollectionName).FindOne(x => x.ApplicationName == ApplicationName && x.Version == Version);
        }

        #region Reader

        public string GetConnectionString(string name)
        {
            var data = _root.GetValue<List<Configuration.ConnectionString>>("ConnectionStrings");

            if (data.Any(x => x.Name == name))
            {
                return data.First(x => x.Name == name).Value;
            }

            return string.Empty;
        }

        public string Get(string key)
        {
            return Get<string>(key);
        }

        public T Get<T>(string key)
        {
            return _root.GetValue<T>(key);
        }

        #endregion Reader

        #region Writer

        public string Add(Config config)
        {
            if (_root.Add(config))
            {
                Sync();
                return config.Key;
            }
            return default;
        }

        public string Add<T>(Config<T> config)
        {
            if (_root.Add<T>(config))
            {
                Sync();
                return config.Key;
            }
            return default;
        }

        public string Update(string key, string value)
        {
            if (_root.Update(key, value))
            {
                Sync();
                return key;
            }
            return default;
        }

        public string Update<T>(string key, T value)
        {
            if (_root.Update<T>(key, value))
            {
                Sync();
                return key;
            }
            return default;
        }

        public bool Delete(string key)
        {
            if (_root.Delete(key))
            {
                Sync();
                return true;
            }
            return false;
        }

        #endregion Writer

        #region Private Methods

        private bool Sync()
        {
            return _liteDb.GetCollection<ConfigRoot>(DefaultCollectionName).Update(_root);
        }

        #endregion Private Methods
    }
}