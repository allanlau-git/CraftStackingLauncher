using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Shell;

namespace CraftStackingLauncher
{
    public partial class MainWindow : Window
    {
        private readonly string[] _knownCraftApps = { 
            "PhotoCraft", "LightCraft", "FilmCraft", "DesignCraft", "VectorCraft", 
            "PdfCraft", "EffectCraft", "WordCraft", "DeckCraft", "GridCraft", 
            "CADCraft", "SoundCraft", "FigCraft" 
        };

        private readonly string _programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private FileSystemWatcher? _watcher;

        public MainWindow()
        {
            InitializeComponent();
            UpdateJumpList();
            StartMonitoringInstalls();
        }

        private void UpdateJumpList()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                JumpList jumpList = new JumpList();
                
                foreach (var appName in _knownCraftApps)
                {
                    string appPath = Path.Combine(_programFiles, appName, $"{appName}.exe");
                    
                    if (File.Exists(appPath))
                    {
                        JumpTask task = new JumpTask
                        {
                            Title = appName,
                            ApplicationPath = appPath,
                            IconResourcePath = appPath,
                            CustomCategory = "Craft App Suite"
                        };
                        jumpList.JumpItems.Add(task);
                    }
                }

                jumpList.ShowFrequentCategory = false;
                jumpList.ShowRecentCategory = false;
                JumpList.SetJumpList(Application.Current, jumpList);
            });
        }

        private void StartMonitoringInstalls()
        {
            _watcher = new FileSystemWatcher(_programFiles)
            {
                NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            _watcher.Created += (s, e) => UpdateJumpList();
            _watcher.Deleted += (s, e) => UpdateJumpList();
        }
    }
}