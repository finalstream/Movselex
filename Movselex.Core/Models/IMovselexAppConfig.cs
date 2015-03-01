namespace Movselex.Core.Models
{
    public interface IMovselexAppConfig
    {
        string MpcExePath { get; }
        int ScreenNo { get; }
        bool IsFullScreen { get; }
    }
}