using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using ChangeAudioOutput.Hotkeys;
using System.ComponentModel;

namespace ChangeAudioOutput
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CoreAudioController audioController;

        CoreAudioDevice defaultOutput;

        Data data;

        bool close = false;

        public MainWindow()
        {
            InitializeComponent();

            HotkeysManager.SetupSystemHook();

            GlobalHotkey hotkey1 = new GlobalHotkey(ModifierKeys.Alt, Key.N, Hotkey1);
            GlobalHotkey hotkey2 = new GlobalHotkey(ModifierKeys.Alt, Key.M, Hotkey2);

            HotkeysManager.AddHotkey(hotkey1);
            HotkeysManager.AddHotkey(hotkey2);

            #region send application to tray

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("image.ico");
            ni.Visible = true;
            ni.Click +=
                delegate (object sender, EventArgs e)
                {
                    this.Show();
                    this.WindowState = System.Windows.WindowState.Normal;
                };
            ni.DoubleClick +=
                delegate (object sender, EventArgs e)
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    close = true;
                    this.Close();
                };

            #endregion

            audioController = new CoreAudioController();
            defaultOutput = audioController.DefaultPlaybackDevice;

            var OutputDevices = audioController.GetPlaybackDevices(DeviceState.Active);

            foreach(var device in OutputDevices)
            {
                dropDownMenu.Items.Add(device.FullName);
                dropDownMenu2.Items.Add(device.FullName);
            }

            data = SaveLoadManager.Load();


            if (data != null)
            { 
                dropDownMenu.Text = data.device1Name;
                dropDownMenu2.Text = data.device2Name;
            }

            AddHotKeys();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if(WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(!close)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                HotkeysManager.ShutdownSystemHook();
            }

            base.OnClosing(e);
        }

        public void Hotkey1()
        {
            var outputDevices = audioController.GetPlaybackDevices(DeviceState.Active);

            foreach (var device in outputDevices)
            {
                if (dropDownMenu.Text == device.FullName)
                {
                    audioController.DefaultPlaybackDevice = device;
                    break;
                }
            }
        }

        public void Hotkey2()
        {
            var outputDevices = audioController.GetPlaybackDevices(DeviceState.Active);

            foreach (var device in outputDevices)
            {
                if (dropDownMenu2.Text == device.FullName)
                {
                    audioController.DefaultPlaybackDevice = device;
                    break;
                }
            }
        }

        private void AddHotKeys()
        {
            //Hotkey for first audio output
            RoutedCommand firstHotKey = new RoutedCommand();
            firstHotKey.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(firstHotKey, FirstHotKeyHandler));

            //hotkey for second audio output
            RoutedCommand secondHotKey = new RoutedCommand();
            secondHotKey.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(secondHotKey, SecondHotKeyHandler));
        }
        
        private void FirstHotKeyHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var outputDevices = audioController.GetPlaybackDevices(DeviceState.Active);

            foreach (var device in outputDevices)
            {
                if(dropDownMenu.Text == device.FullName)
                {
                    audioController.DefaultPlaybackDevice = device;
                    break;
                }
            }
        }

        private void SecondHotKeyHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var outputDevices = audioController.GetPlaybackDevices(DeviceState.Active);

            foreach (var device in outputDevices)
            {
                if (dropDownMenu2.Text == device.FullName)
                {
                    audioController.DefaultPlaybackDevice = device;
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Data newData = new Data(dropDownMenu.Text, dropDownMenu2.Text);
            SaveLoadManager.Save(newData);
        }
    }
}
