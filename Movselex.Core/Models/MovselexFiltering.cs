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
        public ObservableSynchronizedCollection<FilteringItem> FilteringItems { get; private set; } 


        public MovselexFiltering()
        {
            FilteringItems = new ObservableSynchronizedCollection<FilteringItem>();
        }

        /// <summary>
        /// フィルタ条件をロードします。
        /// </summary>
        public void Load()
        {
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDefinitions.DefaultFilteringFilePath);

            var filters = JsonConvert.DeserializeObject<List<FilteringItem>>(
                File.ReadAllText(absolutePath));

            FilteringItems.Clear();
            foreach (var filter in filters)
            {
                FilteringItems.Add(filter);
            }
        }

    }
}
