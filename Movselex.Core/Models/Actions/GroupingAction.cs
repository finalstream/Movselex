using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;

namespace Movselex.Core.Models.Actions
{
    /// <summary>
    /// ライブラリをグループに登録します。
    /// </summary>
    internal class GroupingAction : MovselexActionBase
    {
        private readonly string _title;
        private readonly string _keyword;
        private readonly IEnumerable<LibraryItem> _libraries;


        public GroupingAction(string title, string keyword, IEnumerable<LibraryItem> libraries)
        {
            _title = title;
            _keyword = keyword;
            _libraries = libraries;
        }


        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexGroup.JoinGroup(_title, _keyword, _libraries);
            client.MovselexGroup.Load();
        }
    }
}