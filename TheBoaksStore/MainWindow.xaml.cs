using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace TheBoaksStore
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AppListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppListBox.SelectedItem is ListBoxItem selectedItem)
            {
                AppNameTextBlock.Text = selectedItem.Content.ToString();
                AppDescriptionTextBlock.Text = "Description for " + selectedItem.Content.ToString();
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppListBox.SelectedItem is ListBoxItem selectedItem)
            {
                string appName = selectedItem.Tag.ToString();
                InstallApp(appName);
            }
        }

        private void InstallApp(string appName)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Start-Process cmd -ArgumentList '/c winget install -e --id {appName} --silent' -Verb runAs\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(psi))
            {
                process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                process.ErrorDataReceived += (sender, args) => Console.WriteLine("ERROR: " + args.Data);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}
