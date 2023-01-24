using System;

namespace UniTools.Build
{
    public sealed class BuildStepFailedException : Exception
    {
        public BuildStepFailedException(string message) : base(message)
        {
        }
    }
}