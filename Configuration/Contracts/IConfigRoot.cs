
namespace Configuration
{
    public interface IConfigStore
    {
        Config<T> GetConfig<T>(string key);

        Task<Config<T>> GetConfigAsync<T>(string key);

        bool Write<T>(IConfig<T> config);

        Task<bool> WriteAsync<T>(IConfig<T> config);

        bool Update<T>(IConfig<T> config);

        Task<bool> UpdateAsync<T>(IConfig<T> config);

        bool SetValue<T>(string key, T value);

        Task<bool> SetValueAsync<T>(string key, T value);
    }
}