using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;

namespace Speedy.Core
{
    public class NetworkMonitor
    {
        private readonly List<NetworkMonitorItem> _monitors;

        public NetworkMonitor(IEnumerable<NetworkInterface> networkInterfaces)
        {
            if(networkInterfaces == null)
            {
                throw new NullReferenceException(nameof(networkInterfaces));
            }

            _monitors = networkInterfaces
                .Select(i => new NetworkMonitorItem(i))
                .ToList();               
        }

        public void Measure()
        {
            _monitors.ForEach(i => i.Measure());
        }

        public NetworkTransferInfo GetTransferInfo(string interfaceName)
        {
            var monitorItem = GetMonitorByName(interfaceName);
            return new NetworkTransferInfo(monitorItem);
        }

        private NetworkMonitorItem GetMonitorByName(string interfaceName)
        {
            return _monitors.SingleOrDefault(i => i.InterfaceName == interfaceName);
        }
    }
}