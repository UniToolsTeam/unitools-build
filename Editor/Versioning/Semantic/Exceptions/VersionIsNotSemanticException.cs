using System;

namespace UniTools.Build.Versioning.Semantic
{
    public sealed class VersionIsNotSemanticException : Exception
    {
        public VersionIsNotSemanticException(string message) : base(message)
        {
        }
    }
}