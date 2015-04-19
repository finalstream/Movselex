using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FinalstreamCommons.Models;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using Movselex.Models;

namespace Movselex.ViewModels.Pages.Settings
{
    public class AboutViewModel : ViewModel
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

        public string ApplicationName { get { return App.Client.ApplicationNameWithVersion; } }

        public void Initialize()
        {
            
        }

        public List<DynamicLinkLibraryInfo> DynamicLinkLibraries { get; private set; } 

        public AboutViewModel()
        {
            DynamicLinkLibraries = new List<DynamicLinkLibraryInfo>();

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "Movselex.Core.dll",
                "Movselex Core",
                "Core Feature",
                null,
                "Copyright (c) 2015 FINALSTREAM",
                "http://www.finalstream.net/"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "FinalstreamCommons.dll",
                "Finalstream Commons",
                "Framework",
                "OSS Coming Soon...",
                "Copyright (c) 2015 FINALSTREAM",
                "http://www.finalstream.net/"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "FinalstreamUIComponents.dll",
                "Finalstream UI Components",
                "UI Component",
                "OSS Coming Soon...",
                "Copyright (c) 2015 FINALSTREAM",
                "http://www.finalstream.net/"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "Livet.dll",
                "Livet",
                "WPF MVVM Infrastructure",
                "zlib/libpng License",
                "Copyright (c) 2010-2011 Livet Project",
                "https://github.com/ugaya40/Livet"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "FirstFloor.ModernUI.dll",
                "Modern UI for WPF",
                "UI Framework",
                "Microsoft Public License",
                null,
                "https://github.com/firstfloorsoftware/mui"));


            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "System.Reactive.Core.dll",
                "Reactive Extensions",
                "LINQ Library",
                "Apache License 2.0",
                "Copyright (c) Microsoft Corporation",
                "http://rx.codeplex.com/"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "System.Data.SQLite.dll",
                "System.Data.SQLite",
                "Database Engine",
                "Public Domain License",
                null,
                "http://system.data.sqlite.org/"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "Dapper.dll",
                "Dapper",
                "Micro ORM",
                "Apache License 2.0",
                null,
                "https://github.com/StackExchange/dapper-dot-net"));


            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "taglib-sharp.dll",
                "TagLib Sharp",
                "TagAnalyzer",
                "GNU Lesser General Public License 2.1",
                null,
                "https://github.com/mono/taglib-sharp"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "migemo.dll",
                "C/Migemo",
                "Incremental Search Engine",
                "MIT License",
                "Copyright (c) 2003-2007 MURAOKA Taro (KoRoN)",
                "https://github.com/koron/cmigemo"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "NLog.dll",
                "NLog",
                "Logging",
                "BSD License",
                "Copyright (c) 2004-2011 Jaroslaw Kowalski",
                "http://nlog-project.org/"));

            DynamicLinkLibraries.Add(new DynamicLinkLibraryInfo(
                "Newtonsoft.Json.dll",
                "Json.NET",
                "Json Parser",
                "MIT License",
                "Copyright (c) 2007 James Newton-King",
                "http://www.newtonsoft.com/json"));
        }
    }
}
