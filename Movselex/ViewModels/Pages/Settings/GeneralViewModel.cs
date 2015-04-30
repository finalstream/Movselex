﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using FinalstreamCommons.Frameworks;
using FinalstreamCommons.Utils;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using Movselex.Models;

namespace Movselex.ViewModels.Pages.Settings
{
    public class GeneralViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        public void Initialize()
        {
            
        }

        public GeneralViewModel()
        {
            _players = new DispatcherCollection<DisplayableItem<string>>(DispatcherHelper.UIDispatcher);
            _players.Add(new DisplayableItem<string>("mpc", "Media General Classic"));
            _screens = new DispatcherCollection<DisplayableItem<int>>(DispatcherHelper.UIDispatcher);
            var i = 1;
            foreach (Screen screen in Screen.AllScreens)
            {
                _screens.Add(new DisplayableItem<int>(i++, ScreenUtils.GetDisplayDevice(screen.DeviceName).DeviceString));
            }
            CurrentScreen = App.Config.ScreenNo;
        }

        #region Players変更通知プロパティ

        private DispatcherCollection<DisplayableItem<string>> _players;

        public DispatcherCollection<DisplayableItem<string>> Players
        {
            get { return _players; }
            set
            {
                if (_players == value) return;
                _players = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Players変更通知プロパティ

        private DispatcherCollection<DisplayableItem<int>> _screens;

        public DispatcherCollection<DisplayableItem<int>> Screens
        {
            get { return _screens; }
            set
            {
                if (_screens == value) return;
                _screens = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ExePath変更通知プロパティ

        private string _exePath;

        public string ExePath
        {
            get { return App.Config.MpcExePath; }
            set
            {
                if (_exePath == value) return;
                _exePath = value;
                App.Config.MpcExePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LimitNum変更通知プロパティ

        private int _limitNum;

        public int LimitNum
        {
            get { return App.Config.LimitNum; }
            set
            {
                if (_limitNum == value) return;
                _limitNum = value;
                App.Config.LimitNum = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsFullScreen変更通知プロパティ

        private bool _isFullScreen;

        public bool IsFullScreen
        {
            get { return App.Config.IsFullScreen; }
            set
            {
                if (_isFullScreen == value) return;
                _isFullScreen = value;
                App.Config.IsFullScreen = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CurrentScreen変更通知プロパティ

        private int _currentScreen;

        public int CurrentScreen
        {
            get { return _currentScreen; }
            set
            {
                if (_currentScreen == value) return;
                _currentScreen = value;
                App.Config.ScreenNo = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void OpenDialogExePath()
        {
            var exepath = DialogUtils.ShowFileDialog("MPC-HC.exeを指定してください。", "exeファイル(*.exe)|*.exe");
            if (exepath != null) ExePath = exepath;
        }
    }
}
