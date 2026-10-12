using System;
using System.Collections.Generic;
using System.Text;

namespace UniTools.Build
{
    public static class CommandLineParser
    {
        public static bool TryToParseValue(string commandLine, string key, out object val)
        {
            return TryToParseValue(Tokenize(commandLine), key, out val);
        }

        // Use OS-tokenized arguments for real batch invocation; keep the string API for callers.
        public static bool TryToParseValue(IEnumerable<string> arguments, string key, out object val)
        {
            val = null;
            if (arguments == null || string.IsNullOrWhiteSpace(key)) return false;
            using (IEnumerator<string> iterator = arguments.GetEnumerator())
            {
                while (iterator.MoveNext())
                {
                    string argument = iterator.Current;
                    if (argument != null && argument.StartsWith(key + "=", StringComparison.Ordinal))
                    {
                        val = argument.Substring(key.Length + 1);
                        return true;
                    }
                    if (!string.Equals(argument, key, StringComparison.Ordinal)) continue;
                    if (!iterator.MoveNext()) return false;
                    string value = iterator.Current;
                    if (value == null || (value.StartsWith("-", StringComparison.Ordinal)
                        && (value.Length < 2 || !char.IsDigit(value[1])))) return false;
                    val = value;
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<string> Tokenize(string commandLine)
        {
            var tokens = new List<string>();
            if (string.IsNullOrWhiteSpace(commandLine)) return tokens;
            var token = new StringBuilder();
            char quote = '\0';
            bool started = false;
            for (int i = 0; i < commandLine.Length; i++)
            {
                char current = commandLine[i];
                if (current == '\\' && quote != '\'')
                {
                    int start = i;
                    while (i < commandLine.Length && commandLine[i] == '\\') i++;
                    int count = i - start;
                    if (i < commandLine.Length && commandLine[i] == '"')
                    {
                        token.Append('\\', count / 2);
                        if (count % 2 != 0) token.Append('"');
                        else quote = quote == '"' ? '\0' : '"';
                    }
                    else { token.Append('\\', count); i--; }
                    started = true;
                }
                else if ((current == '"' || current == '\'') && (quote == '\0' || quote == current))
                {
                    quote = quote == current ? '\0' : current;
                    started = true;
                }
                else if (char.IsWhiteSpace(current) && quote == '\0')
                {
                    if (started) { tokens.Add(token.ToString()); token.Clear(); started = false; }
                }
                else { token.Append(current); started = true; }
            }
            if (quote != '\0') throw new FormatException("Unterminated command-line quote.");
            if (started) tokens.Add(token.ToString());
            return tokens;
        }
    }
}
