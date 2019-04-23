using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Diagnostics;
using Emgu.CV.Dpm;

namespace FaceCounter
{
    public static class ImageProcessor
    {
        /// <summary>
        /// Find the pedestrian in the image
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="processingTime">The pedestrian detection time in milliseconds</param>
        /// <returns>The image with pedestrian highlighted.</returns>
        public static Image<Bgr, Byte> Find(Image<Bgr, Byte> image, out long processingTime, out int count)
        {
            Stopwatch watch;
            //MCvObjectDetection[] regions2;
            Rectangle[] regions;

            ////check if there is a compatible GPU to run pedestrian detection
            //if (DpmInvoke.HasCuda)
            //{  //this is the GPU version
            //    using (GpuHOGDescriptor des = new GpuHOGDescriptor())
            //    {
            //        des.SetSVMDetector(GpuHOGDescriptor.GetDefaultPeopleDetector());

            //        watch = Stopwatch.StartNew();
            //        using (GpuImage<Bgr, Byte> gpuImg = new GpuImage<Bgr, byte>(image))
            //        using (GpuImage<Bgra, Byte> gpuBgra = gpuImg.Convert<Bgra, Byte>())
            //        {
            //            regions = des.DetectMultiScale(gpuBgra);
            //        }
            //    }
            //}
            //else
            //{  //this is the CPU version
            //using (HOGDescriptor des = new HOGDescriptor())
            //{
            //    des.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            //    watch = Stopwatch.StartNew();
            //    regions2 = des.DetectMultiScale(image);
            //}
            //}
            using (CascadeClassifier des = new CascadeClassifier("C:\\Emgu\\emgucv-windesktop 3.4.3.3016\\etc\\haarcascades\\haarcascade_frontalface_default.xml"))
            //using (CascadeClassifier des = new CascadeClassifier("C:\\Emgu\\emgucv-windesktop 3.4.3.3016\\etc\\haarcascades\\haarcascade_frontalface_alt.xml"))
            //using (CascadeClassifier des = new CascadeClassifier("C:\\Emgu\\emgucv-windesktop 3.4.3.3016\\etc\\haarcascades\\haarcascade_fullbody.xml"))
            //using (CascadeClassifier des = new CascadeClassifier("C:\\Users\\Travis\\Desktop\\HS.xml"))
            {
                var imageClone = image.Convert<Gray, byte>().Clone();
                watch = Stopwatch.StartNew();
                regions = des.DetectMultiScale(imageClone, 1.1, 3);
            }

            watch.Stop();

            processingTime = watch.ElapsedMilliseconds;
            count = regions.Length;

            //foreach (MCvObjectDetection pedestrain in regions2)
            //{
            //    image.Draw(pedestrain.Rect, new Bgr(Color.Blue), 5);
            //}
            foreach (Rectangle pedestrain in regions)
            {
                image.Draw(pedestrain, new Bgr(Color.Red), 5);
            }
            return image;
        }
    }
}
