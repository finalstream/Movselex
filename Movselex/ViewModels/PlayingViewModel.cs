using System;
using Livet;
using Movselex.Core.Models;

namespace Movselex.ViewModels
{
    public class PlayingViewModel : ViewModel
    {
        private readonly PlayingItem _model;


        public PlayingViewModel(PlayingItem model)
        {
            _model = model;
        }

        public PlayingItem Model
        {
            get { return _model; }
        }

        public void Initialize()
        {
        }
    }
}