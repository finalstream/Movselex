using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Movselex.ViewModels.Pages.Settings;

namespace Movselex.Views.Pages.Settings
{
    /// <summary>
    /// Interaction logic for ImportExport.xaml
    /// </summary>
    public partial class ImportExport : UserControl
    {
        public ImportExport()
        {
            InitializeComponent();
            this.DataContext = new ImportExportViewModel();
        }
    }
}
