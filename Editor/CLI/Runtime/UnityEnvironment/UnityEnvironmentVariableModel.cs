namespace UniTools.CLI
{
    public sealed class UnityEnvironmentVariableModel
    {
        public string Name { get; }

        public string Value { get; }

        public UnityEnvironmentVariableModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(UnityEnvironmentVariableModel)}: {nameof(Name)}:{Name},{nameof(Value)}:{Value}";
        }
    }
}