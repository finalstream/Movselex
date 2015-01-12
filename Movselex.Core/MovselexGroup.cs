using System;
using Livet;
using Movselex.Core.Models;

namespace Movselex.Core
{
    internal class MovselexGroup
    {

        public DispatcherCollection<GroupItem> GroupItems { get; private set; }

        private readonly IDatabaseAccessor _databaseAccessor;

        public MovselexGroup(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            GroupItems = new DispatcherCollection<GroupItem>(DispatcherHelper.UIDispatcher);
        }


        /// <summary>
        /// ライブラリデータをロードします。
        /// </summary>
        public void Load()
        {
            var groups = _databaseAccessor.SelectGroup();

            GroupItems.Clear();
            foreach (var groupItem in groups)
            {
                GroupItems.Add(groupItem);
            }
        }
    }
}