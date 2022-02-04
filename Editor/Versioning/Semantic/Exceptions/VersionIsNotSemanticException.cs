using System;

namespace UniTools.Build.Versioning
{
    public sealed class VersionIsNotSemanticException : Exception
    {
        public VersionIsNotSemanticException(string message) : base(message)
        {
        }
    }
}