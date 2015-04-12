namespace Movselex.Core.Models
{
    public interface IProgressInfo
    {
        bool IsProgressing { get; }

        string ProgressMessage { get; }

        void UpdateProgress(bool isProgressing);
        void SetProgressMessage(string message);
    }
}