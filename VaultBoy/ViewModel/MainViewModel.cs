using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Forms;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using VaultBoy.Messages;
using VaultBoy.ProfileCore;
using VaultBoy.Services;
using VaultBoy.ViewModel.Tabs;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace VaultBoy.ViewModel
{
    /// <summary>
    /// The powerhouse of the application!
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;
        private Database _currentDatabase;
        private string _title;
        private string _status;
        private BaseProfile _currentProfile;
        private TabModelBase _selectedTab;
        private RelayCommand<TabModelBase> _tabCloseCommand;
        private string _currentDirectory;
        private Process _currentGameProcess;

        /// <summary>
        /// Command used to ask the user for a game directory to load.
        /// </summary>
        public RelayCommand OpenCommand { get; }

        /// <summary>
        /// Command used to save the database
        /// </summary>
        public RelayCommand SaveCommand { get; }
        
        /// <summary>
        /// Command used to launch the game
        /// </summary>
        public RelayCommand PlayCommand { get; }

        public Database CurrentDatabase
        {
            get => _currentDatabase;
            set => Set(ref _currentDatabase, value);
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        public TabModelBase SelectedTab
        {
            get => _selectedTab;
            set
            {
                Set(ref _selectedTab, value);
                UpdateSelectedTab();
            }
        }

        public RelayCommand<object> OpenDBItemCommand { get; }

        public ObservableCollection<TabModelBase> Tabs { get; } = new ObservableCollection<TabModelBase>();


        /// <summary>
        /// Initializes commands and such
        /// </summary>
        public MainViewModel(ProfileService profileService, DatabaseService databaseService)
        {
            // Initialization
            _databaseService = databaseService;
            profileService.LoadProfiles();
            UpdateTitle();

            // Commands
            _tabCloseCommand = new RelayCommand<TabModelBase>(ExecuteTabCloseCommand);
            OpenCommand = new RelayCommand(ExecuteOpenCommand);
            SaveCommand = new RelayCommand(
                ExecuteSaveCommand, 
                CanExecuteSave);
            OpenDBItemCommand = new RelayCommand<object>(ExecuteOpenDBItemCommand);
            PlayCommand = new RelayCommand(ExecutePlayCommand, CanExecutePlay);

            // Messages
            Messenger.Default.Register<CurrentDatabaseChangedMessage>(this, HandleCurrentDatabaseChangedMessage);

            // Final init
            AddTab(new StartPageViewModel());
        }

        private bool CanExecutePlay()
        {
            return CurrentDatabase != null && _currentProfile.IsLaunchSupported() && _currentGameProcess == null;
        }

        private void ExecutePlayCommand()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_currentDirectory, _currentProfile.GetLaunchExecutablePath()),
                WorkingDirectory = _currentDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                Verb = "runas"
            };

            Process process = Process.Start(processStartInfo);

            if (process == null)
            {
                MessageBox.Show("Failed to launch game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _currentGameProcess = null;
                PlayCommand.RaiseCanExecuteChanged();
            }
            else
            {
                _currentGameProcess = process;
                PlayCommand.RaiseCanExecuteChanged();
                process.EnableRaisingEvents = true;
                process.Exited += (sender, args) =>
                {
                    _currentGameProcess = null;
                    Dispatcher.CurrentDispatcher.Invoke(() => PlayCommand.RaiseCanExecuteChanged());
                };
            }
        }

        private bool CanExecuteSave()
        {
            return CurrentDatabase != null && _currentProfile.IsSaveSupported();
        }

        private void ExecuteSaveCommand()
        {
            _databaseService.SaveDatabase(_currentDirectory);
            Status = "Saved database to " + _currentDirectory;
        }

        private void UpdateSelectedTab()
        {
            foreach (var tab in Tabs)
            {
                tab.IsSelected = SelectedTab != null && SelectedTab == tab;
            }
        }

        private void ExecuteOpenDBItemCommand(object obj)
        {
            if (obj is CollectionViewGroup collectionViewGroup)
            {
                if (collectionViewGroup.Name is VLTClass vltClass)
                {
                    AddTab(new ClassInfoViewModel(vltClass));
                }
            } else if (obj is VLTCollection collection)
            {
                AddTab(new CollectionEditorViewModel(collection));
            }
            else
            {
                throw new Exception();
            }
        }

        private void ExecuteTabCloseCommand(TabModelBase obj)
        {
            Tabs.Remove(obj);
        }

        private void HandleCurrentDatabaseChangedMessage(CurrentDatabaseChangedMessage msg)
        {
            CurrentDatabase = msg.Database;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string title = "VaultBoy";
            if (CurrentDatabase != null && _currentProfile != null)
            {
                title += $" - {_currentProfile.GetName()} [{CurrentDatabase.Files.Sum(f => f.Vaults.Count)} vaults]";
            }

            Title = title;
        }

        private void ExecuteOpenCommand()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.AutoUpgradeEnabled = true;
            fbd.Description = "Select game folder";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var (success, profile, files) = _databaseService.LoadDatabase(fbd.SelectedPath);

                if (!success)
                {
                    Status = "Failed to load data from: " + fbd.SelectedPath;
                }
                else
                {
                    Status = $"Loaded {files.Count} file(s) from {fbd.SelectedPath}: {string.Join(", ", files.Select(f => f.ShortPath))}";
                }

                _currentDirectory = fbd.SelectedPath;
                _currentProfile = profile;
                UpdateTitle();
                SaveCommand.RaiseCanExecuteChanged();
                PlayCommand.RaiseCanExecuteChanged();
            }
        }

        private void AddTab(TabModelBase tab)
        {
            if (!Tabs.Contains(tab))
            {
                tab.CloseCommand = _tabCloseCommand;
                Tabs.Add(tab);
                SelectedTab = tab;
            }
            else
            {
                SelectedTab = Tabs.First(t => t.Equals(tab));
            }
        }
    }
}