namespace Configuration
{
    public interface IConfig<T>
    {
        string Key { get; set; }

        T? Value { get; set; }

        T DefaultValue { get; set; }
    }
}