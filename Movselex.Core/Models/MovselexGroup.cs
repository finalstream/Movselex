using Livet;

namespace Movselex.Core.Models
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
        /// ���C�u�����f�[�^�����[�h���܂��B
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