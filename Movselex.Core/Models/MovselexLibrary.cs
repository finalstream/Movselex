using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Movselex.Core.Models
{
    /// <summary>
    /// Movselexで扱うライブラリを表します。
    /// </summary>
    class MovselexLibrary : NotificationObject
    {
        public ObservableCollection<LibraryItem> LibraryItems { get; private set; }

        private readonly IDatabaseAccessor _databaseAccessor;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="databaseAccessor"></param>
        public MovselexLibrary(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            LibraryItems = new ObservableCollection<LibraryItem>();
        }

        /// <summary>
        /// ライブラリデータをロードします。
        /// </summary>
        /// <param name="libCondition"></param>
        public void Load(LibraryCondition libCondition)
        {
            var libraries = _databaseAccessor.SelectLibrary(libCondition);

            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                LibraryItems.Add(libraryItem);
            }
        }

        public void Shuffle(int limitNum)
        {
            var libraries = _databaseAccessor.ShuffleLibrary(limitNum);

            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                LibraryItems.Add(libraryItem);
            }
        }
    }
}
