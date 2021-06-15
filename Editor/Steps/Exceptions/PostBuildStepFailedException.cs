using System;

namespace UniTools.Build
{
    public sealed class PostBuildStepFailedException : Exception
    {
        public PostBuildStepFailedException(string message) : base(message)
        {
        }
    }
}