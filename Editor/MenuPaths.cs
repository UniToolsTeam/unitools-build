namespace UniTools.Build
{
    public static class MenuPaths
    {
        public const string Core = nameof(UniTools) + "/";
        public const string IO = Core + "IO/";
        public const string Pipelines = Core + "Pipelines/";

        public const string Build = Core + "Build/";
        public const string Defines = Build + "Defines/";
        public const string Versioning = Build + "Versioning/";

        private const string Platforms = Build + "Platforms/";
        public const string Android = Platforms + "Android/";
        public const string IOS = Platforms + "iOS/";
        public const string Standalone = Platforms + "Standalone/";
        public const string WebGL = Platforms + "WebGL/";
    }
}