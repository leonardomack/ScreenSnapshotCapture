
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ScreenSnapshotCapture.Library
{
    public class Recorder
    {
        private Boolean isRunning;
        private readonly Thread recorderThread;
        private Int32 filenameCount;
        private readonly String savePath;
        private readonly Int32 interval;
        
        public Recorder(String savePath, Int32 interval)
        {
            this.savePath = savePath;
            this.interval = interval;
            
            this.isRunning = false;
            this.filenameCount = 1;            
            this.recorderThread = new Thread(StartRecordingAction);
        }
        
        public void Start()
        {
            this.isRunning = true;
            this.recorderThread.Start();
        }
        
        public void Stop()
        {
            this.isRunning = false;
            this.filenameCount = 1;
        }
        
        private void StartRecordingAction()
        {
            String pathWithSubfolder = String.Empty;
            while (isRunning)
            {
                DateTime now = DateTime.Now;
                pathWithSubfolder = this.savePath + @"Captures\" + now.Hour + @"\";
                
                CheckFolder(pathWithSubfolder);
                
                using (Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
                {
                    using (Graphics graphic = Graphics.FromImage(bmpScreenCapture))
                    {
                        graphic.CopyFromScreen(
                            Screen.PrimaryScreen.Bounds.X,
                            Screen.PrimaryScreen.Bounds.Y,
                            0, 0,
                            bmpScreenCapture.Size,
                            CopyPixelOperation.SourceCopy);
                    }
                    
                    bmpScreenCapture.Save(pathWithSubfolder + this.filenameCount + ".jpg", ImageFormat.Jpeg);
                }
                
                this.filenameCount++;
                System.Threading.Thread.Sleep(this.interval);
            }
        }
        
        private void CheckFolder(String path)
        {
            Boolean exists = System.IO.Directory.Exists(path);
            if (exists == false)
            {
                System.IO.Directory.CreateDirectory(path);
                filenameCount = 1;
            }
        }
    }
}