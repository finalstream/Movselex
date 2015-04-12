using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Movselex.Core.Models;

namespace Movselex.Core
{
    public interface IMovselexClient : IDisposable
    {
        /// <summary>
        ///     フィルタリング情報。
        /// </summary>
        ObservableCollection<FilteringItem> Filterings { get; }

        /// <summary>
        ///     ライブラリ情報。
        /// </summary>
        ObservableCollection<LibraryItem> Libraries { get; }

        /// <summary>
        ///     グループ情報。
        /// </summary>
        ObservableCollection<GroupItem> Groups { get; }

        /// <summary>
        ///     再生中情報。
        /// </summary>
        INowPlayingInfo NowPlayingInfo { get; }

        /// <summary>
        ///     アプリ設定。
        /// </summary>
        MovselexAppConfig AppConfig { get; }

        ObservableCollection<string> Databases { get; }

        IProgressInfo ProgressInfo { get; }

        string ApplicationNameWithVersion { get; }
        event EventHandler Initialized;

        /// <summary>
        ///     初期化を行います。
        /// </summary>
        void Initialize();

        void Finish();

        void ExecEmpty();

        /// <summary>
        ///     データベースを変更します。
        /// </summary>
        /// <param name="databaseName"></param>
        void ChangeDatabase(string databaseName);

        /// <summary>
        ///     ライブラリモードをスイッチします。
        /// </summary>
        void SwitchLibraryMode();

        /// <summary>
        ///     フィルタリングを変更します。
        /// </summary>
        /// <param name="filteringItem"></param>
        void ChangeFiltering(FilteringItem filteringItem);

        /// <summary>
        ///     グループを変更します。
        /// </summary>
        /// <param name="groupItem"></param>
        void ChangeGroup(GroupItem groupItem);

        /// <summary>
        ///     ライブラリをシャッフルします。
        /// </summary>
        void ShuffleLibrary();

        /// <summary>
        ///     スローします。
        /// </summary>
        /// <param name="librarySelectIndex"></param>
        void Throw(int librarySelectIndex);


        void InterruptThrow(int librarySelectIndex);

        void UpdateLibrary();
        void Refresh();
        void ModifyIsPlayed(LibraryItem item);
        void ModifyIsFavorite(LibraryItem item);
        void ModifyIsFavorite(GroupItem item);
        void ModifyIsComplete(GroupItem item);
        void RegistFiles(IEnumerable<string> enumerable);
        void MoveGroupDirectory(GroupItem currentGroup, string moveDirectory);
        void FilteringLibrary(string filteringText);
        void RegistGroup(string title, string keyword, IEnumerable<LibraryItem> libraries);
        void ModifyGroup(GroupItem group, string groupName, string keyword);
    }
}