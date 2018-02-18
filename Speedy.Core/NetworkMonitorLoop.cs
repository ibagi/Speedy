using System;
using Speedy.Core;
using System.Net.NetworkInformation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Speedy.Core
{
    public class NetworkMonitorLoop
    {
        public async void Run(CancellationToken token)
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .ToList();

            var monitor = new NetworkMonitor(interfaces);
            monitor.Start();

            while (true)
            {
                if(token.IsCancellationRequested)
                {
                    monitor.Stop();
                    token.ThrowIfCancellationRequested();
                }

                await Task.Delay(Interval);

                var data = interfaces.Select(i => monitor.GetTransferInfo(i.Name))
                    .OrderBy(i => i.InterfaceName)
                    .ToList();

                DataAvailable?.Invoke(data);
            }
        }

        public event Action<List<NetworkTransferInfo>> DataAvailable;
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1);
    }
}