namespace UniTools.Build
{
    public abstract class BuildParameterPresenter
    {
        
        public abstract string CliKey { get; }
        
        public abstract void Draw(bool duplicated);
    }
}