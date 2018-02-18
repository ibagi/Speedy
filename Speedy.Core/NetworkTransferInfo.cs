namespace Speedy.Core
{
    public class NetworkTransferInfo
    {
        public string InterfaceName { get; set; }
        public long Download { get; set; }
        public long Upload { get; set; }
        public int DownloadInKiloBit => (int)(Download / 1024f * 8);
        public int UploadInKiloBit => (int)(Upload / 1024f * 8);
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