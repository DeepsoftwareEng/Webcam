using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Threading;
using System.Windows.Media;
using System.Drawing;
using System.Threading;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Webcam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool iscapture = false;
        VideoCapture capture = new VideoCapture();
        Image<Bgr, byte> image = null;
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a timer to update the webcam feed
            capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 200);
            capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 200);
            timer.Interval = TimeSpan.FromMilliseconds(144);
            timer.Tick += (s, ev) =>
            {
                    Mat frame = new Mat();
                    capture.Read(frame);
                    // Convert the image to a BitmapSource that can be displayed in the Image control
                    Image<Bgr, byte> images = frame.ToImage<Bgr, byte>();
                    BitmapSource bitmap = BitmapSourceConvert.ToBitmapSource(images);
                    // Set the Image control's Source property to the BitmapSource
                    webcam.Source = bitmap;
            };
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            iscapture = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            iscapture = true;
            using (capture)
            {
                // Create a face detector
                CascadeClassifier faceDetector = new CascadeClassifier("D:\\Lap trinh C#\\test1\\Webcam\\Webcam\\haarcascade_frontalface_default.xml");

                // Capture images from the webcam
                while (true)
                {
                    Mat frame = new Mat();
                    capture.Read(frame);
                    image = frame.ToImage<Bgr, byte>();

                    // Detect faces in the image
                    Rectangle[] faces = faceDetector.DetectMultiScale(image.Convert<Gray, byte>(), 1.2, 10, System.Drawing.Size.Empty);

                    // Draw rectangles around the faces
                    foreach (Rectangle face in faces)
                    {
                        image.Draw(face, new Bgr(System.Drawing.Color.Red), 2);
                    }

                    // Update the image control with the latest image
                    result.Source = BitmapSourceConvert.ToBitmapSource(image);
                    // Wait for the user to click on the image control
                    if (iscapture == true)
                    {
                         // Crop the face image
                         Rectangle face = faces[0];
                         Image<Gray, byte> faceImage = image.Convert<Gray, byte>().Copy(face);
                        faceImage.Save(@"C:\\Users\\minhv\\Desktop\\test.png");

                        MessageBox.Show("Worker added successfully.");
                    }
                    break;
                }
            }
        }
    }
}
