using Microsoft.AspNetCore.Mvc;
using ElectronNET.API;
using Speedy.Core;
using System.Linq;
using System.Collections.Generic;

namespace Speedy.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly NetworkMonitorLoop _loop;
        private List<NetworkTransferInfo> _currentInfo = new List<NetworkTransferInfo>();

        public HomeController(NetworkMonitorLoop loop)
        {
            _loop = loop;
            _loop.DataAvailable += data => _currentInfo = data;
        }

        public IActionResult Index()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("req-refresh", (ctx) =>
                {
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, "res-refresh", _currentInfo);
                });
            }

            return View();
        }
    }
}