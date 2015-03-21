using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly MovselexLibrary _movselexLibrary;
        private readonly List<string> _searchDirectoryPaths = new List<string>();
        private readonly string[] _supportExts;

        public LibraryUpdater(MovselexLibrary movselexLibrary, string[] supportExts)
        {
            _movselexLibrary = movselexLibrary;
            _supportExts = supportExts.Select(x=> x.ToLower()).ToArray();
        }

        public IReadOnlyCollection<string> SearchDirectoryPaths {get { return _searchDirectoryPaths; }} 

        /// <summary>
        /// ライブラリを更新します。
        /// </summary>
        public void Update()
        {
            if (SearchDirectoryPaths.Count == 0)
            {
                // 一番使用されているパスを検索対象とする。
                var mostUseDirectoryPath = _movselexLibrary.SelectMostUseDirectoryPath();
                if (!string.IsNullOrEmpty(mostUseDirectoryPath)) _searchDirectoryPaths.Add(mostUseDirectoryPath);
            }

            foreach (var searchDirectoryPath in SearchDirectoryPaths)
            {
                // 検索対象ディレクトリからサポートしている拡張子のファイルだけ抜く。
                var registFiles = Directory.GetFiles(searchDirectoryPath, "*", SearchOption.TopDirectoryOnly)
                    .Where(x => _supportExts.Contains(Path.GetExtension(x).ToLower()));

                _movselexLibrary.Regist(registFiles);
            }

            // 不完全なものを再スキャン
            _movselexLibrary.ReScan(_movselexLibrary.GetInCompleteIds());
        }

    }
}
