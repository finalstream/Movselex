using System.Collections.ObjectModel;
using Livet;

namespace Movselex.Core.Models
{
    internal class MovselexGroup : IMovselexGroup
    {

        public ObservableCollection<GroupItem> GroupItems { get; private set; }

        private readonly IDatabaseAccessor _databaseAccessor;

        public MovselexGroup(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            GroupItems = new ObservableCollection<GroupItem>();
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