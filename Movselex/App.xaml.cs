using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Livet;
using Movselex.Core.Models;
using Movselex.ViewModels;
using Movselex.Views;
using NLog;

namespace Movselex
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public static HomeViewModel ViewModelRoot { get; private set; }

        public static MovselexAppConfig Config { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherHelper.UIDispatcher = Dispatcher;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomainUnhandledException);

            ViewModelRoot = new HomeViewModel();
            Config = ViewModelRoot.AppConfig;
            this.MainWindow = new MainWindow { DataContext = ViewModelRoot };
            this.MainWindow.Show();
        }

        //集約エラーハンドラ
        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Error("Catch UnhandledException.", e.ExceptionObject as Exception);

            MessageBox.Show(
                "不明なエラーが発生しました。アプリケーションを終了します。エラーログを開発元に送付してください。",
                "Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Environment.Exit(1);
        }
    }
}
