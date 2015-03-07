using Livet;

namespace Movselex.Core.Models
{
    internal class NowPlayingInfo : NotificationObject, INowPlayingInfo
    {
        public NowPlayingInfo()
        {
            _title = "";
            _playTime = "";
        }

        #region Title変更通知プロパティ

        private string _title;

        public string Title
        {
            get { return string.IsNullOrEmpty(_title) ? "" : string.Format("{0} {1}", Properties.Resources.NowPlayingLabel , _title); }
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChanged();
            }
        }

        
        #endregion

        #region PlayTime変更通知プロパティ

        private string _playTime;

        public string PlayTime
        {
            get { return _playTime; }
            set
            {
                if (_playTime == value) return;
                _playTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="title"></param>
        public void Update(string title, string playtime)
        {
            Title = title;
            PlayTime = playtime;
        }

    }
}