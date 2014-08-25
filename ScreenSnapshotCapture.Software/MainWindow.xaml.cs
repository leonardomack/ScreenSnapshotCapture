
#region LICENSE

//The MIT License (MIT)
//Copyright (c) 2014 Leonardo Mack
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScreenSnapshotCapture.Library;

namespace ScreenSnapshotCapture.Software
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private String currentApplicationPath;
        private Recorder recorder;
        private String selectedPath;
        
        public MainWindow()
        {
            InitializeComponent();
            
            InitializeApplication();
        }
        
        private void InitializeApplication()
        {
            //Get application root path
            String projectName = Assembly.GetEntryAssembly().GetName().Name;
            this.currentApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory.Substring(0, System.AppDomain.CurrentDomain.BaseDirectory.IndexOf(projectName) + projectName.Length) + "//";
            
            //Adjusting SystemTray
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            this.notifyIcon.BalloonTipText = "";
            this.notifyIcon.BalloonTipTitle = "";
            this.notifyIcon.Text = "ScreenSnapshotCapture";
            this.notifyIcon.Icon = new System.Drawing.Icon(this.currentApplicationPath + @"\Resources\Images\Ico.ico");
            this.notifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);
        }
        
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = WindowState.Normal;
        }
        
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    {
                        break;
                    }
                case WindowState.Minimized:
                    {
                        Hide();
                        if (this.notifyIcon != null)
                        {
                            this.notifyIcon.Visible = true;
                        }
                        break;
                    }
                case WindowState.Normal:
                    {
                        this.notifyIcon.Visible = false;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
        private void ButtonChoseFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            
            this.selectedPath = dialog.SelectedPath;
        }
        
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            //Checking if it's a valid time in milliseconds
            if (IsValidTimeMilliseconds(TextBoxTimeMilliseconds.Text))
            {
                //New instance of a recorder
                this.recorder = new Recorder(this.selectedPath + @"\", Convert.ToInt32(TextBoxTimeMilliseconds.Text));
                
                //Disable Enable button
                ButtonStop.IsEnabled = true;
                ButtonStart.IsEnabled = false;
                
                //Start recording
                this.recorder.Start();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Wrong input for time in milliseconds");
            }
        }
        
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            //Disable Enable button
            ButtonStop.IsEnabled = false;
            ButtonStart.IsEnabled = true;
            
            //Top recorder
            this.recorder.Stop();
        }
        
        private static Boolean IsValidTimeMilliseconds(String timeMilliseconds)
        {
            try
            {
                Convert.ToInt32(timeMilliseconds);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}