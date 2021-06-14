using System;

namespace UniTools.Build
{
    public sealed class PreBuildStepFailedException : Exception
    {
        public PreBuildStepFailedException(string message) : base(message)
        {
        }
    }
}