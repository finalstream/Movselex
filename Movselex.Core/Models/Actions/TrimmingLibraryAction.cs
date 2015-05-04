using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class TrimmingLibraryAction : MovselexAction
    {
        private readonly int _librarySelectIndex;
        private readonly bool _isShuffle;


        public TrimmingLibraryAction(int librarySelectIndex, bool isShuffle)
        {
            _librarySelectIndex = librarySelectIndex;
            _isShuffle = isShuffle;
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="client"></param>
        public override void InvokeCore(MovselexClient client)
        {
            var appConfig = client.AppConfig;

            if (_isShuffle)
            {
                client.MovselexLibrary.Shuffle(appConfig.LimitNum);
            }
            else
            {
                client.MovselexLibrary.Trimming(_librarySelectIndex, appConfig.LimitNum);
            }

            client.MovselexGroup.Load(client.MovselexLibrary.LibraryItems.Select(x=>x.Gid).Distinct());
        }
    }
}
