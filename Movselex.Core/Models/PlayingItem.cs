using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Movselex.Core.Models
{
    public class PlayingItem : NotificationObject
    {

        #region StartTime変更通知プロパティ

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime == value) return;
                _startTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region EndTime変更通知プロパティ

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Height変更通知プロパティ

        private int _height;

        public int Height
        {
            get { return _height; }
            set
            {
                if (_height == value) return;
                _height = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public LibraryItem Item { get; private set; }

        public PlayingItem(LibraryItem library)
        {
            Item = library;
            StartTime = DateTime.Now;
            EndTime = StartTime.Add(library.Duration);
            Height = CreateHeight(library.Duration);
        }

        public PlayingItem(LibraryItem library, PlayingItem before)
        {
            Item = library;
            StartTime = before != null? before.EndTime : DateTime.Now;
            EndTime = StartTime.Add(library.Duration);
            Height = CreateHeight(library.Duration);
        }

        public int CreateHeight(TimeSpan duration)
        {
            return (int) (Math.Ceiling(duration.TotalMinutes/30d) * 65);
        }
    }
}
