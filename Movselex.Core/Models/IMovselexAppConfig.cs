using FinalstreamCommons.Frameworks;

namespace Movselex.Core.Models
{
    public interface IMovselexAppConfig : IAppConfig
    {
        string MpcExePath { get; }
        int ScreenNo { get; }
        bool IsFullScreen { get; }
    }
}