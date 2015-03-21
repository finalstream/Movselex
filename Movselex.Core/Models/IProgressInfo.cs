namespace Movselex.Core.Models
{
    public interface IProgressInfo
    {
        bool IsProgressing { get; }

        void UpdateProgress(bool isProgressing);
    }
}