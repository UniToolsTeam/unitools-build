namespace UniTools.Build
{
    public static class MenuPaths
    {
        public const string Core = nameof(UniTools) + "/";
        public const string IO = Steps + "IO/";
        public const string Parameters  = Core + "Parameters/";

        public const string Steps = Core + "Steps/";
        public const string Defines = Steps + "Defines/";
        public const string Versioning = Steps + "Versioning/";

        private const string Platforms = Steps + "Platforms/";
        public const string Android = Platforms + "Android/";
        public const string IOS = Platforms + "iOS/";
        public const string Standalone = Platforms + "Standalone/";
        public const string WebGL = Platforms + "WebGL/";
    }
}