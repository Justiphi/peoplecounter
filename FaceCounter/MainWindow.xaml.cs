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
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FaceCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            long responseTime;
            int count;
            //Mat mat = CvInvoke.Imread("C:\\Users\\Travis\\source\\repos\\FaceCounter\\FaceCounter\\camera4.jpg", Emgu.CV.CvEnum.ImreadModes.AnyColor);
            Mat mat = CvInvoke.Imread("C:\\Users\\Travis\\source\\repos\\FaceCounter\\FaceCounter\\testImage.jpeg", Emgu.CV.CvEnum.ImreadModes.AnyColor);
            //Mat mat = CvInvoke.Imread("C:\\Users\\Travis\\source\\repos\\FaceCounter\\FaceCounter\\testing.jpg", Emgu.CV.CvEnum.ImreadModes.AnyColor);
            Image<Bgr, Byte> img = mat.ToImage<Bgr, Byte>();
            Image<Bgr, Byte> response = ImageProcessor.Find(img, out responseTime, out count);
            BitmapSource responseSource = ToBitmapSource(response);
            itemLabel.Content = $"{count} objects found in {responseTime}ms";
            imageBox.Source = responseSource;
            //showPicture(response);
        }

        private void showPicture(Image<Bgr, Byte> image)
        {
            ImageViewer.Show(image, "Tester");
        }


        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
    }
}
