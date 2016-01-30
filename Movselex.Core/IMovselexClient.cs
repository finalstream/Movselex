using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FinalstreamCommons.Collections;
using Firk.Core;
using Movselex.Core.Models;

namespace Movselex.Core
{
    public interface IMovselexClient : IAppClient
    {
        event EventHandler<Exception> ExceptionThrowed;

        /// <summary>
        ///     フィルタリング情報。
        /// </summary>
        SelectableObservableCollection<FilteringItem> Filterings { get; }

        /// <summary>
        ///     ライブラリ情報。
        /// </summary>
        ObservableCollection<LibraryItem> Libraries { get; }

        /// <summary>
        ///     グループ情報。
        /// </summary>
        SelectableObservableCollection<GroupItem> Groups { get; }

        ObservableCollection<PlayingItem> Playings { get; }

        /// <summary>
        ///     再生中情報。
        /// </summary>
        INowPlayingInfo NowPlayingInfo { get; }


        /// <summary>
        ///     アプリ設定。
        /// </summary>
        new MovselexAppConfig AppConfig { get; }

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
        /// <param name="searchText"></param>
        void ChangeFiltering(FilteringItem filteringItem, string searchText);

        /// <summary>
        ///     グループを変更します。
        /// </summary>
        /// <param name="groupItem"></param>
        void ChangeGroup(GroupItem groupItem);

        /// <summary>
        ///     ライブラリをシャッフルします。
        /// </summary>
        /// <param name="librarySelectIndex"></param>
        /// <param name="isShuffle"></param>
        void TrimmingLibrary(int librarySelectIndex, bool isShuffle);

        /// <summary>
        ///     スローします。
        /// </summary>
        /// <param name="librarySelectIndex"></param>
        void Throw(IEnumerable<LibraryItem> librarySelectIndex);


        void InterruptThrow(LibraryItem library);

        void UpdateLibrary();
        void Refresh();
        void ModifyIsPlayed(LibraryItem item);
        void ModifyIsFavorite(LibraryItem item);
        void ModifyIsFavorite(GroupItem item);
        void ModifyIsComplete(GroupItem item);
        void RegistFiles(IEnumerable<string> enumerable);
        void MoveGroupDirectory(GroupItem currentGroup, string moveDirectory);
        void FilteringLibrary(string filteringText);
        void Grouping(string title, string keyword, IEnumerable<LibraryItem> libraries);
        void ModifyGroup(GroupItem group, string groupName, string keyword);
        void UnGroupLibrary(LibraryItem[] selectLibraries);
        void SaveConfig();
        void DeleteLibrary(LibraryItem[] selectLibraries, bool isDeleteFile);
        void GetCandidateGroupName(string groupName, Action<IEnumerable<string>> action);
        void OpenLibraryFolder(LibraryItem libraryItem);
        void MoveLibraryFile(string moveDestDirectory, LibraryItem[] selectLibraries);
        void ReloadFiltering();
        void DiffRefresh();
        void InterruptThrow(long id);
        void ResetBackgroundWorker();
        void ResetLibraryUpdater();
        void ModifyLibrary(LibraryItem library, Dictionary<string, object> modDataDic);
        void DeleteGroup(GroupItem group, bool isDeleteFile);
    }
}