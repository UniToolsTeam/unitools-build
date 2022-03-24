using System;

namespace UniTools.Build
{
    public sealed class VersionIsNotSemanticException : Exception
    {
        public VersionIsNotSemanticException(string message) : base(message)
        {
        }
    }
}