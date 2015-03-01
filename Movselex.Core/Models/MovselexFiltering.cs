using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Extentions;
using Livet;
using Newtonsoft.Json;

namespace Movselex.Core.Models
{
    /// <summary>
    /// Movselexのフィルタ条件を表します。
    /// </summary>
    internal class MovselexFiltering : NotificationObject, IMovselexFiltering
    {
        public ObservableCollection<FilteringItem> FilteringItems { get; private set; } 

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public MovselexFiltering()
        {
            FilteringItems = new ObservableCollection<FilteringItem>();
        }

        /// <summary>
        /// フィルタ条件をロードします。
        /// </summary>
        public void Load()
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDefinitions.DefaultFilteringFilePath);

            var filters = JsonConvert.DeserializeObject<List<FilteringItem>>(
                File.ReadAllText(absolutePath));

            FilteringItems.DiffUpdate(filters, new FilteringItemComparer());

            //FilteringItems.Clear();
            //foreach (var filter in filters)
            //{
            //    FilteringItems.Add(filter);
            //}
        }

    }
}
