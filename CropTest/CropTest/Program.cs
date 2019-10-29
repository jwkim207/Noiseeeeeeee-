using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace CropTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat src = Cv2.ImRead("./cc.bmp");
            Mat gray = new Mat(src.Size(), MatType.CV_8UC1);
            Mat binary = new Mat(src.Size(), MatType.CV_8UC1);
            //Mat closed = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, binary, 12, 255, ThresholdTypes.Triangle);

            //Mat kernel = Cv2.GetStructuringElement(MorphShapes.Cross,new Size(5,5));
            //Cv2.MorphologyEx(binary, closed, MorphTypes.Close, kernel,iterations:10);
            Cv2.ImShow("test", binary);


            Cv2.WaitKey(0);

        }
    }
}
