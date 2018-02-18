using System;
using Speedy.Core;
using System.Linq;
using System.Threading;

namespace Speedy.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            using(var cancellation = new CancellationTokenSource())
            {
                var loop = new NetworkMonitorLoop();
                
                loop.DataAvailable += data => 
                {
                    Console.Clear();
                    data.ForEach(Render);                       
                };

                loop.Run(cancellation.Token);
                Console.ReadKey();
            }
        }

        static void Render(NetworkTransferInfo info)
        {
            Console.WriteLine($"↓ {info.DownloadInKiloBit} kB/s ↑ {info.UploadInKiloBit} kB/s - {info.InterfaceName}"); 
        }
    }
}
