namespace UniTools.Build
{
    public sealed class BatchModeParameters
    {
        /// <summary>
        /// The name of the pipeline from the command line
        /// </summary>
        [CommandLineParameter] public string Pipeline = default;
    }
}