using System;

namespace Movselex.Core.Models.Actions
{
    internal class IncrementPlayCountAction : MovselexAction
    {

        private long _id;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public IncrementPlayCountAction(long id)
        {
            _id = id;
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="client"></param>
        public override void InvokeCore(MovselexClient client)
        {

            if (_id != -1) client.MovselexLibrary.IncrementPlayCount(_id);
        }
    }
}