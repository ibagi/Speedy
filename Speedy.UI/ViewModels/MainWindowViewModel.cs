using System;
using System.Collections.Generic;
using System.Text;
using Speedy.Core;
using ReactiveUI;
using System.Linq;

namespace Speedy.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly NetworkMonitorLoop _monitorLoop;

        public MainWindowViewModel(NetworkMonitorLoop monitorLoop)
        {
            _monitorLoop = monitorLoop;
            _monitorLoop.DataAvailable += OnDataAvailable;
        }

        private void OnDataAvailable(List<NetworkTransferInfo> data)
        {
            Data = data.Select(i => new NetworkInfoItemViewModel(i))
                .ToList();
        }

        private List<NetworkInfoItemViewModel> _data;
        public List<NetworkInfoItemViewModel> Data 
        { 
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }
    }
}
