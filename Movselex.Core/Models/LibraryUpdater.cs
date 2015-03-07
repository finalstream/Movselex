using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    /// <summary>
    /// ライブラリを更新するものを表します。
    /// </summary>
    class LibraryUpdater
    {

        private readonly string[] searchDirectoryPaths;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public LibraryUpdater()
        {
            
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="searchDirectoryPath"></param>
        public LibraryUpdater(string searchDirectoryPath)
        {
            this.searchDirectoryPaths = new []{searchDirectoryPath};
        }

        /// <summary>
        /// ライブラリを更新します。
        /// </summary>
        public void Update()
        {
            
        }

    }
}
