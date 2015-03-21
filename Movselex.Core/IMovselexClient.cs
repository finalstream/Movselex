﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FinalstreamCommons.Models;
using Livet;
using Movselex.Core.Models;

namespace Movselex.Core
{
    public interface IMovselexClient : IDisposable
    {

        event EventHandler Initialized;

        /// <summary>
        /// フィルタリング情報。
        /// </summary>
        ObservableCollection<FilteringItem> Filterings { get; }

        /// <summary>
        /// ライブラリ情報。
        /// </summary>
        ObservableCollection<LibraryItem> Libraries { get; }

        /// <summary>
        /// グループ情報。
        /// </summary>
        ObservableCollection<GroupItem> Groups { get; }

        /// <summary>
        /// 再生中情報。
        /// </summary>
        INowPlayingInfo NowPlayingInfo { get; }

        /// <summary>
        /// アプリ設定。
        /// </summary>
        MovselexAppConfig AppConfig { get; }

        ObservableCollection<string> Databases { get; }

        IProgressInfo ProgressInfo { get; }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        void Initialize();

        void Finish();

        void ExecEmpty();

        /// <summary>
        /// データベースを変更します。
        /// </summary>
        /// <param name="databaseName"></param>
        void ChangeDatabase(string databaseName);

        /// <summary>
        /// ライブラリモードをスイッチします。
        /// </summary>
        void SwitchLibraryMode();

        /// <summary>
        /// フィルタリングを変更します。
        /// </summary>
        /// <param name="filteringItem"></param>
        void ChangeFiltering(FilteringItem filteringItem);

        /// <summary>
        /// ライブラリをシャッフルします。
        /// </summary>
        void ShuffleLibrary();

        /// <summary>
        /// スローします。
        /// </summary>
        /// <param name="librarySelectIndex"></param>
        void Throw(int librarySelectIndex);


        void InterruptThrow(int librarySelectIndex);

        void UpdateLibrary();
        void Refresh();
        void ModifyIsPlayed(LibraryItem item);
        void ModifyIsFavorite(LibraryItem item);
    }
}