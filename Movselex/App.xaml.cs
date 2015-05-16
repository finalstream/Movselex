using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using Livet;
using Movselex.Core;
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

        public static IMovselexClient Client { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherHelper.UIDispatcher = Dispatcher;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomainUnhandledException);

            ViewModelRoot = new HomeViewModel();
            Client = ViewModelRoot.Client;
            Config = ViewModelRoot.AppConfig;
            this.MainWindow = new MainWindow { DataContext = ViewModelRoot };
            this.MainWindow.Show();
        }

        //集約エラーハンドラ
        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Error("Catch UnhandledException.", e.ExceptionObject as Exception);

            ModernDialog.ShowMessage(Movselex.Properties.Resources.MessageFatalError, "Critical Error",
               MessageBoxButton.OK);

            Environment.Exit(1);
        }
    }
}
