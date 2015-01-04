﻿using System;
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
        public DispatcherCollection<LibraryItem> LibraryItems { get; private set; }

        private readonly IDatabaseAccessor _databaseAccessor;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="databaseAccessor"></param>
        public MovselexLibrary(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            LibraryItems = new DispatcherCollection<LibraryItem>(DispatcherHelper.UIDispatcher);
        }

        /// <summary>
        /// ライブラリデータをロードします。
        /// </summary>
        public void Load()
        {
            var libraries = _databaseAccessor.SelectLibrary();

            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                LibraryItems.Add(libraryItem);
            }
        }

    }
}
