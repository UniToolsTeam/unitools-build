using System.Collections.Generic;
using System.Linq;

namespace UniTools.Build
{
    public static class Cli
    {
        static Cli()
        {
            AllTools = new Dictionary<string, BaseCliTool>();
            Refresh();
        }

        private static readonly Dictionary<string, BaseCliTool> AllTools = default;

        public static IEnumerable<BaseCliTool> All => AllTools.Values;

        public static BaseCliTool Tool(string executable)
        {
            return AllTools[executable];
        }

        public static TTool Tool<TTool>() where TTool : BaseCliTool
        {
            return AllTools.Values.OfType<TTool>().FirstOrDefault();
        }

        public static void Refresh()
        {
            AllTools.Clear();

            AllTools.Add(nameof(CommandLine), CommandLine.Default);
            AllTools.Add(nameof(PathResolver), PathResolver.Default);

            List<BaseCliToolAttribute> declared = ReflectionHelper.Find<BaseCliToolAttribute>().ToList();
            foreach (BaseCliToolAttribute attribute in declared)
            {
                AllTools.Add(attribute.Executable, attribute.Create());
            }
        }
    }
}