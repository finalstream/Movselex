using System;
using System.Collections.Generic;
using Firk.Core.Actions;

namespace Movselex.Core.Models.Actions
{
    internal class ModifyLibraryAction : IGeneralAction<MovselexClient>
    {

        private LibraryItem _library;

        private Dictionary<string, object> _modDataDic;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="library"></param>
        /// <param name="modDataDic"></param>
        public ModifyLibraryAction(LibraryItem library, Dictionary<string, object> modDataDic)
        {
            _library = library;
            _modDataDic = modDataDic;
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="client"></param>
        public void Invoke(MovselexClient client)
        {
            client.MovselexLibrary.ModifyLibrary(_library, _modDataDic);
        }
    }
}