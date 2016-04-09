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
    internal class GroupingAction : MovselexAction
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
            // TODO: 同じキーワードの登録はエラーとする。
            //var movGroup = client.MovselexGroup;
            //var sameKeywordGroups = movGroup.GetMatchKeywordGroups(_keyword);
            //if (ownGroup != null) sameKeywordGroups = sameKeywordGroups.Where(x => x.GID != ownGroup.Gid);  // グループ更新のときは自分を除く。
            //if (sameKeywordGroups.Any())
            //{
            //    // 同じキーワードのグループがすでに存在したらエラー
            //    throw new MovselexException(string.Format("すでに同じキーワードのグループが登録されています。\nキーワード：{0}", keyword.ToLower()));
            //}
            var movGroup = client.MovselexGroup;
            movGroup.JoinGroup(_title, _keyword, _libraries);
            movGroup.Load();
        }
    }
}