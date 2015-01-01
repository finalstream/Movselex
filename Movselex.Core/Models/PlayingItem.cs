using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    public class PlayingItem
    {

        public string Time { get; private set; }

        public string Title { get; private set; }

        public PlayingItem(string time, string title)
        {
            Time = time;
            Title = title;
        }
    }
}
