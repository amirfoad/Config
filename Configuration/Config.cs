namespace Configuration
{
    public class Config : IConfig<string>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
    }

    public class Config<T> : IConfig<T>
    {
        public string Key { get; set; }
        public T? Value { get; set; }
        public T DefaultValue { get; set; }
    }
}