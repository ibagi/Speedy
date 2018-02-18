namespace Speedy.Core
{
    public class NetworkTransferInfo
    {
        public string InterfaceName { get; set; }
        public long Download { get; set; }
        public long Upload { get; set; }
        public long DownloadInKiloBit => Download / 1024 * 8;
        public long UploadInKiloBit => Upload / 1024 * 8;
        public NetworkTransferInfo()
        {
            
        }

        internal NetworkTransferInfo(NetworkMonitorItem monitorItem)
        {
            InterfaceName = monitorItem.InterfaceName;
            Download = monitorItem.DownloadRate;
            Upload = monitorItem.UploadRate;
        }
    }
}