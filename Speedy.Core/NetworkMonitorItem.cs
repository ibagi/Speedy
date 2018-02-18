using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Speedy.Core
{
    public class NetworkMonitorItem
    {
        private readonly NetworkInterface _networkInterface;
        private CancellationTokenSource _cancellation;

        private long _downloadMemo;
        private long _uploadMemo;

        public NetworkMonitorItem(NetworkInterface networkInterface)
        {
            _networkInterface = networkInterface ?? throw new NullReferenceException(nameof(networkInterface));
        }

        public void Start()
        {
            _cancellation = new CancellationTokenSource();
            Task.Run(() => Loop(_cancellation.Token));
        }

        public void Stop()
        {
            if(_cancellation != null)
            {
                _cancellation.Cancel();
            }
        }

        private async void Loop(CancellationToken token)
        {
            while(true)
            {
                try 
                {
                    if(token.IsCancellationRequested)
                    {
                        DownloadRate = 0;
                        UploadRate = 0;
                        break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                    UpdateRates();
                } 
                catch(Exception)
                {
                }
            }
        }

        private void UpdateRates()
        {
            var statistics = _networkInterface.GetIPv4Statistics();

            DownloadRate =  statistics.BytesReceived - _downloadMemo;
            UploadRate = statistics.BytesSent - _uploadMemo;

            _downloadMemo = statistics.BytesReceived;
            _uploadMemo = statistics.BytesSent;
        }

        public string InterfaceName => _networkInterface?.Name ?? string.Empty;
        public long DownloadRate { get; private set; }
        public long UploadRate { get; private set;}
    }
}