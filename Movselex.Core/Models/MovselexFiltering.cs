using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinalstreamCommons.Collections;
using FinalstreamCommons.Extensions;
using Livet;
using Newtonsoft.Json;

namespace Movselex.Core.Models
{
    /// <summary>
    /// Movselexのフィルタ条件を表します。
    /// </summary>
    internal class MovselexFiltering : NotificationObject, IMovselexFiltering
    {
        public SelectableObservableCollection<FilteringItem> FilteringItems { get; private set; } 

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public MovselexFiltering()
        {
            FilteringItems = new SelectableObservableCollection<FilteringItem>();
        }

        /// <summary>
        /// フィルタ条件をロードします。
        /// </summary>
        /// <param name="language"></param>
        public void Load(string language)
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDefinitions.DefaultFilteringFilePath);

            var filters = JsonConvert.DeserializeObject<List<FilteringConfig>>(
                File.ReadAllText(absolutePath)).Select(x => new FilteringItem(x.Value, x.DisplayValue[language])).ToArray();

            FilteringItems.DiffUpdate(filters, new FilteringItemComparer());

            //FilteringItems.Clear();
            //foreach (var filter in filters)
            //{
            //    FilteringItems.Add(filter);
            //}
        }

        /// <summary>
        /// ALLMovieが選択状態であるかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsSelectAllMovie()
        {
            var select = FilteringItems.Where(x => x.IsSelected).Select(x => x.Value).FirstOrDefault();
            if (select == null) return false;
            return select.Sql == "";
        }
    }
}
