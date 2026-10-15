using System;
using System.Threading;
using System.Windows;

namespace CraftStackingLauncher
{
    public partial class App : Application
    {
        private static Mutex? _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "CraftStackingLauncher_SingleInstance";
            
            // Request ownership of the named mutex
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                // An instance is already running, shut down immediately
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }
    }
}