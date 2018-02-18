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
        private long _downloadMemo;
        private long _uploadMemo;

        public NetworkMonitorItem(NetworkInterface networkInterface)
        {
            _networkInterface = networkInterface ?? throw new NullReferenceException(nameof(networkInterface));
        }

        public void Measure()
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