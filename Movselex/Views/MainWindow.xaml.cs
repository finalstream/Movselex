using System;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using Movselex.ViewModels;

namespace Movselex.Views
{
    /* 
     * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : ModernWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            var windowBounds = App.Config.WindowBounds;
            this.Left = windowBounds.Left;
            this.Top = windowBounds.Top;
            this.Width = windowBounds.Width;
            this.Height = windowBounds.Height;

            Closed += (sender, args) =>
            {
                App.Config.WindowBounds = new Rect(this.Left, this.Top, this.Width, this.Height);
                App.Client.SaveConfig();
            };
        }

    }
}
