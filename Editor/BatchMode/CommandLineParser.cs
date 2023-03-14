namespace UniTools.Build
{
    public static class CommandLineParser
    {
        private const string KeyStart = "-";

        public static bool TryToParseValue(string commandLine, string key, out object val)
        {
            string[] args = commandLine.Split(' ');
            for (int i = 0; i < args.Length; i++)
            {
                if (IsKey(args[i]) && args[i].Equals(key))
                {
                    int next = i + 1;
                    if (next < args.Length && !IsKey(args[next]))
                    {
                        val = args[next];

                        return true;
                    }
                }
            }

            val = null;

            return false;
        }

        private static bool IsKey(string param)
        {
            return !string.IsNullOrEmpty(param) && param.StartsWith(KeyStart);
        }
    }
}