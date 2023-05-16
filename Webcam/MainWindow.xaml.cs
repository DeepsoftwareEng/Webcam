using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Threading;
using System.Windows.Media;

namespace Webcam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Create a capture object to capture images from the webcam
            VideoCapture capture = new VideoCapture();
            capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 200);
            capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 200);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(33); // Update the image control 30 times per second
            timer.Tick += (s, ev) =>
            {
                // Query the capture object for the latest image from the webcam
                Mat frame = new Mat();
                capture.Read(frame);

                // Convert the image to a BitmapSource that can be displayed in the Image control
                Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
                BitmapSource bitmap = BitmapSourceConvert.ToBitmapSource(image);
                // Set the Image control's Source property to the BitmapSource
                webcam.Source = bitmap;
            };

            // Start the timer
            timer.Start();
        }
    }
}
