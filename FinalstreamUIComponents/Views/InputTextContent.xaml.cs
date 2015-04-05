using System.Collections.Generic;
using System.Windows.Controls;
using FinalstreamUIComponents.Models;
using FinalstreamUIComponents.ViewModels;

namespace FinalstreamUIComponents.Views
{
    /// <summary>
    /// InputTextContent.xaml の相互作用ロジック
    /// </summary>
    public partial class InputTextContent : UserControl
    {
        public InputTextContent()
        {
            InitializeComponent();
        }

        public Dictionary<string, InputParam> InputParamDictionary { get { return _viewModel.InputParamDictionary; } }

        private readonly InputTextContentViewModel _viewModel;

        public InputTextContent(string messge, Dictionary<string, InputParam> paramDic) : this()
        {
            _viewModel = new InputTextContentViewModel();
            _viewModel.Message = messge;
            _viewModel.InputParamDictionary = paramDic;
            this.DataContext = _viewModel;
        }
    }
}
