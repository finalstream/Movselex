using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FinalstreamUIComponents.Controls
{
    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register(
        "Value",
        typeof(int),
        typeof(NumericUpDown),
        new PropertyMetadata(0,PropertyChangedCallback, CoerceValueCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var nud = dependencyObject as NumericUpDown;
            if (nud == null) return;
            nud.OnPropertyChanged("Value");
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty DeltaValueProperty =
            DependencyProperty.Register(
                "DeltaValue",
                typeof (int),
                typeof (NumericUpDown),
                new PropertyMetadata(1));

        private static object CoerceValueCallback(DependencyObject dependencyObject, object baseValue)
        {
            var value = (int)baseValue;
            var nud = dependencyObject as NumericUpDown;
            if (nud == null) return value;
            
            if (value < nud.MinValue)
            {
                return nud.MinValue;
            }
            if (value > nud.MaxValue)
            {
                return nud.MaxValue;
            }
            return value;
        }

        public int DeltaValue
        {
            get { return (int)GetValue(DeltaValueProperty); }
            set { SetValue(DeltaValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
    DependencyProperty.Register(
        "MinValue",
        typeof(int),
        typeof(NumericUpDown),
        new PropertyMetadata(int.MinValue));

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
    DependencyProperty.Register(
        "MaxValue",
        typeof(int),
        typeof(NumericUpDown),
        new PropertyMetadata(int.MaxValue));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }


        private void UpButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value += DeltaValue;
        }

        private void DownButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value-= DeltaValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
