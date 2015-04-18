using System;
using System.Diagnostics;
using FinalstreamCommons.Models;

namespace Movselex.Core.Models.Actions
{
    /// <summary>
    /// カウントアップを監視するアクションを表します。
    /// </summary>
    internal class PlayMonitoringAction : BackgroundAction
    {
        #region CountUpTimePlayedイベント

        // Event object
        public event EventHandler<long> CountUpTimePlayed;

        protected virtual void OnCountUpTimePlayed(long id)
        {
            var handler = this.CountUpTimePlayed;
            if (handler != null)
            {
                handler(this, id);
            }
        }

        #endregion

        #region SwitchTitleイベント

        // Event object
        public event EventHandler<string> SwitchTitle;

        protected virtual void OnSwitchTitle(string title)
        {
            var handler = this.SwitchTitle;
            if (handler != null)
            {
                handler(this, title);
            }
        }

        #endregion

        private INowPlayingInfo _nowPlayingInfo;
        private Stopwatch _playCountStopwatch;
        private string _keepPlayTitle;
        private double _keepPlayTime;

        public PlayMonitoringAction(INowPlayingInfo nowPlayingInfo)
        {
            _nowPlayingInfo = nowPlayingInfo;
            _playCountStopwatch = new Stopwatch();
        }

        protected override void InvokeCoreAsync()
        {
            string nowTitle = _nowPlayingInfo.Title;
            if (string.IsNullOrEmpty(nowTitle)) return;
            if (_keepPlayTitle != nowTitle)
            {
                // タイトル更新された
                _keepPlayTitle = nowTitle;
                _keepPlayTime = _nowPlayingInfo.PlayTimeSeconds;
                _playCountStopwatch.Restart();
                OnSwitchTitle(nowTitle);
            }
            else
            {
                // タイトル継続
                if (_keepPlayTime > 0
                    && _playCountStopwatch.ElapsedMilliseconds > 0 
                    && (_playCountStopwatch.ElapsedMilliseconds / 1000.0) >
                _keepPlayTime * 0.5D)
                {
                    // 半分再生したらカウントアップ
                    if (_nowPlayingInfo.Title != _keepPlayTitle) return;
                    OnCountUpTimePlayed(_nowPlayingInfo.Id);
                    _playCountStopwatch.Reset();
                }
            }

        }
    }
}