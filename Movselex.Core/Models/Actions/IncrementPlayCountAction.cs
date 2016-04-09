using System;

namespace Movselex.Core.Models.Actions
{
    internal class IncrementPlayCountAction : MovselexAction
    {

        private long _id;

        /// <summary>
        /// �V�����C���X�^���X�����������܂��B
        /// </summary>
        public IncrementPlayCountAction(long id)
        {
            _id = id;
        }

        /// <summary>
        /// �A�N�V���������s���܂��B
        /// </summary>
        /// <param name="client"></param>
        public override void InvokeCore(MovselexClient client)
        {

            if (_id != -1) client.MovselexLibrary.IncrementPlayCount(_id);
        }
    }
}