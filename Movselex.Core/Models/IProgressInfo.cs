namespace Movselex.Core.Models
{
    public interface IProgressInfo
    {
        bool IsProgressing { get; }

        string ProgressMessage { get; }

        void UpdateProgress(bool isProgressing);
        void SetProgressMessage(string message);
        void UpdateProgressMessage(string message, string detail, int now, int last);
    }
}