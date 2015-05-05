using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Frameworks;
using FinalstreamCommons.Frameworks.Actions;

namespace Movselex.Core.Models.Actions
{
    /// <summary>
    /// 外部のプレイヤーから再生情報を収集するアクションを表します。
    /// </summary>
    class PlayerMediaCrawlerAction : BackgroundAction
    {

         private readonly MpcPlayerInfoGetter _mpcPlayerInfoGetter;

        private readonly string _exePath;

        #region Updatedイベント

        /// <summary>
        /// 再生情報が更新された時に発生するイベントです。
        /// </summary>
        public event EventHandler<PlayerMediaInfo> Updated;

        protected virtual void OnUpdated(PlayerMediaInfo arg)
        {
            var handler = this.Updated;
            if (handler != null)
            {
                handler(this, arg);
            }
        }

        #endregion


        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public PlayerMediaCrawlerAction(string exePath)
        {
            _exePath = exePath;
            _mpcPlayerInfoGetter = new MpcPlayerInfoGetter();
        }

        protected override void InvokeCoreAsync()
        {
            var info = _mpcPlayerInfoGetter.Get(_exePath);
            if (info.Title == "" && info.TimeString == "") return; // シーク中はなぜか空が返却されるのでその場合は無視する。
            OnUpdated(info);
        }
    }
}
