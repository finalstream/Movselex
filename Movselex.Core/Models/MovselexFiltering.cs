using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Newtonsoft.Json;

namespace Movselex.Core.Models
{
    /// <summary>
    /// Movselexのフィルタ条件を表します。
    /// </summary>
    internal class MovselexFiltering : NotificationObject, IMovselexFiltering
    {
        private const string DefaultFilteringFilePath = "sql\\default.json";

        private readonly ObservableSynchronizedCollection<FilteringItem> _filteringItems;
        public ObservableSynchronizedCollection<FilteringItem> FilteringItems { get { return _filteringItems; } } 

        public MovselexFiltering()
        {
            _filteringItems = new ObservableSynchronizedCollection<FilteringItem>();
        }

        /// <summary>
        /// フィルタ条件をロードします。
        /// </summary>
        public void Load()
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultFilteringFilePath);

            var filters = JsonConvert.DeserializeObject<List<FilteringItem>>(
                File.ReadAllText(absolutePath));

            foreach (var filter in filters)
            {
                FilteringItems.Add(filter);
            }
        }

    }
}
