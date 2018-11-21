using System;
using Speedy.Core;

namespace Speedy.UI.ViewModels
{
    public class NetworkInfoItemViewModel
    {
        public double MinWidth { get; set; } = 50;
        public double MaxWidth { get; set; } = 380;
        public NetworkTransferInfo Info { get; private set; }

        public NetworkInfoItemViewModel(NetworkTransferInfo info) => 
            Info = info;

        public string Name => 
            Info.InterfaceName.ToUpper();

        public double DownloadBarWidth => 
            GetBarWidth(Info.DownloadInKiloBit);

        public double UploadBarWidth =>
            GetBarWidth(Info.UploadInKiloBit);

        public string DownloadText =>
            $"{Info.DownloadInKiloBit} {GetBarCaption(Info.DownloadInKiloBit)}";

        public string UploadText =>
            $"{Info.UploadInKiloBit} {GetBarCaption(Info.UploadInKiloBit)}";

        private string GetBarCaption(double value) =>
            value <= 1024 ? "Kbit/s" : "Mbit/s";

        private double GetBarWidth(double value)
        {
            var scaleMin = Math.Log(1);
            var scaleMax = Math.Log(1e-5);
            var scale = (scaleMax - scaleMin) / (MinWidth - MaxWidth);
            return (Math.Log(Math.Max(1, value)) - scaleMin) / scale + MinWidth;
        }
    }
}