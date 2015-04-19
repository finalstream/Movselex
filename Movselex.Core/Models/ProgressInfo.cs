using Livet;

namespace Movselex.Core.Models
{
    internal class ProgressInfo : NotificationObject, IProgressInfo
    {
        #region IsProgressing変更通知プロパティ

        private bool _isProgressing;

        public bool IsProgressing
        {
            get { return _isProgressing; }
            set
            {
                if (_isProgressing == value) return;
                _isProgressing = value;
                if (!value) ProgressMessage = "";
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ProgressMessage変更通知プロパティ

        private string _progressMessage;

        public string ProgressMessage
        {
            get { return _progressMessage; }
            set
            {
                if (_progressMessage == value) return;
                _progressMessage = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void UpdateProgress(bool isProgressing)
        {
            IsProgressing = isProgressing;
            ProgressMessage = "";
        }

        public void SetProgressMessage(string message)
        {
            ProgressMessage = message;
        }

        public void UpdateProgressMessage(string message, string detail, int now, int last)
        {
            SetProgressMessage(string.Format("{0}... {1} ({2} / {3})", message, detail ?? "", now, last));
        }

    }
}