using System.Windows.Controls;
using Movselex.ViewModels.Pages.Settings;

namespace Movselex.Views.Pages.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            this.DataContext = new AboutViewModel();
        }
    }
}
