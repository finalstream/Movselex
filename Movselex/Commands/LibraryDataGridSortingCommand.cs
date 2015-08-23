using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using FinalstreamUIComponents.Commands;
using Movselex.Comparers;

namespace Movselex.Commands
{
    /// <summary>
    ///     キャンセル可能なソート処理を実装するSortingコマンドを表します。
    /// </summary>
    /// <remarks>DataGridのヘッダクリックのソートは昇順→降順のループなので昇順→降順→ソートなしのループになります。</remarks>
    public class LibraryDataGridSortingCommand : CancelableDataGridSortingCommand
    {
        public LibraryDataGridSortingCommand(object source, Action<DataGridSortingEventArgs> afterAction = null)
            : base(source, args =>
            {
                var lcv = CollectionViewSource.GetDefaultView(source) as ListCollectionView;
                if (lcv != null )
                {
                    if (args.Column.Header.ToString() == "No" && !args.Handled)
                    {
                        args.Handled = true;
                        ListSortDirection direction = (args.Column.SortDirection != ListSortDirection.Ascending)
                            ? ListSortDirection.Ascending
                            : ListSortDirection.Descending;
                        args.Column.SortDirection = direction;
                        lcv.CustomSort = new LibararyNoNaturalComparer(direction);
                    }
                    else
                    {
                        lcv.CustomSort = null;
                    }
                }

                if(afterAction != null) afterAction.Invoke(args);

            })
        {

        }
    }
}