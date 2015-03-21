using Livet;

namespace Movselex.Core.Models
{
    internal class ProgressInfo : NotificationObject, IProgressInfo
    {
        #region IsProgressing�ύX�ʒm�v���p�e�B

        private bool _isProgressing;

        public bool IsProgressing
        {
            get { return _isProgressing; }
            set
            {
                if (_isProgressing == value) return;
                _isProgressing = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void UpdateProgress(bool isProgressing)
        {
            IsProgressing = isProgressing;
        }
    }
}