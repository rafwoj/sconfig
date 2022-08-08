using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using SeevoConfig.Communications;
using SeevoConfig.Devices;
using SeevoConfig.Errors;
using SeevoConfig.Projects;

namespace SeevoConfig.Windows.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly MainWindowVM viewModel;
        public readonly Communication communication;
        public object lockProject = new object();

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowVM();
            communication = new Communication();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = viewModel;
            communication.SeevoConfigReceived += Communication_SeevoConfigReceived;
            Logger.LoggerEvent += Logger_LoggerEvent;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            communication.Dispose();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Logger_LoggerEvent(LoggerEventArgs e)
        {
            viewModel.LogTextWrite += e.Message + Environment.NewLine;
        }

        private void Communication_SeevoConfigReceived(SeevoConfigReceivedEventArgs e)
        {
            if (viewModel.Project == null)
            {
                lock (lockProject)
                {
                    if (viewModel.Project == null)
                    {
                        viewModel.Project = ProjectService.New();
                    }
                }
            }

            viewModel.Project.Devices.AddDevice(e.DeviceConfig);
        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            lock (lockProject)
            {
                viewModel.Project = ProjectService.New();
            }
        }

        private void ExampleDataButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LogTextWrite = null;
            Logger.LogDebug("Button Example Data");
            communication.LoadExampleData();
        }

        private void DiscoveryButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LogTextWrite = null;
            Logger.LogDebug("Discovery");
            communication.Discovery();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            communication.SendToDevices(viewModel.Project.Devices);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                CheckFileExists = true,
                DefaultExt = "json",
                Filter = "JSON (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 0,
                Multiselect = false,
                Title = "Open project",
            };

            if (openDialog.ShowDialog() == true)
            {
                lock (lockProject)
                {
                    var project = ProjectService.Load(openDialog.FileName);
                    if (project == null) { return; }
                    viewModel.Project = project;
                }
            }
        }

        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Project == null) { return; }

            var saveDialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "json",
                FileName = "Project Seevo",
                Filter = "JSON (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 0,
                OverwritePrompt = true,
                Title = "Save project",
            };

            if (!string.IsNullOrEmpty(viewModel.Project.FilePath))
            {
                saveDialog.InitialDirectory = Path.GetDirectoryName(viewModel.Project.FilePath);
                saveDialog.FileName = Path.GetFileName(viewModel.Project.FilePath);
            }

            if (saveDialog.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(viewModel.Project.Description))
                {
                    viewModel.Project.Description = ProjectService.DefaultDescription;
                }
                if (viewModel.Project.Created == default)
                {
                    viewModel.Project.Created = DateTime.Now;
                }
                viewModel.Project.Updated = DateTime.Now;
                ProjectService.Save(saveDialog.FileName, viewModel.Project);
            }
        }

        private void DevicesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DeviceSelection();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DeviceSelection();
        }

        private void DeviceSelection()
        {
            if (viewModel.SelectedDevice == null) { return; }
            viewModel.EditDevice = new SeevoModel();
            viewModel.EditDevice.CopyFrom(viewModel.SelectedDevice);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.EditDevice == null) { return; }
            if (viewModel.EditDevice.HasChanged == false) { return; }
            viewModel.SelectedDevice.CopyFrom(viewModel.EditDevice);
        }

        private void EditTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (viewModel.EditDevice == null) { return; }
            viewModel.EditDevice.HasChanged = true;
        }

        private void EventsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (viewModel.EditDevice == null) { return; }
            viewModel.EditDevice.HasChanged = true;
        }
    }
}
