using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace FinalstreamUIComponents.Behaviors
{
    public class FileDropBehavior : Behavior<FrameworkElement>
    {
        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof (ICommand), typeof (FileDropBehavior)
                , new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewDragOver += OnPreviewDragOver;
            AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragOver -= OnPreviewDragOver;
            AssociatedObject.Drop -= OnDrop;
        }

        private void OnPreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent("UniformResourceLocator"))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private IEnumerable<string> ToFileList(IDataObject data)
        {
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                return (data.GetData(DataFormats.FileDrop) as string[]);
            }
            return Enumerable.Empty<string>();
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (Command == null) return;
            if (!Command.CanExecute(e)) return;
            IEnumerable<string> urllist = ToFileList(e.Data);
            if (urllist == null) return;
            Command.Execute(urllist);
        }
    }
}