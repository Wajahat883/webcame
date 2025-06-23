// Webcam Plugin Simulation for Educational Use
// Author: Kali GPT

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;

namespace DCRatSim.WebcamPlugin
{
    public class WebcamCapture
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public void StartCapture()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                Console.WriteLine("No webcam found.");
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            videoSource.Start();

            Console.WriteLine("Webcam capture started. Press ENTER to stop...");
            Console.ReadLine();

            videoSource.SignalToStop();
            videoSource.WaitForStop();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
                string base64Frame = ConvertBitmapToBase64(bitmap);
                Console.WriteLine($"Captured Frame (Base64 - first 100 chars): {base64Frame.Substring(0, 100)}...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error capturing frame: {ex.Message}");
            }
        }

        private string ConvertBitmapToBase64(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    class Program
    {
        static void Main()
        {
            WebcamCapture webcam = new WebcamCapture();
            webcam.StartCapture();
        }
    }
}
