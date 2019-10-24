using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace OpenCVConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat src = Cv2.ImRead("2.bmp",ImreadModes.Grayscale);
            Mat pry1 = new Mat();
            Mat binary = new Mat();
            Mat roi = new Mat();
            int start_x = 5232 ;
            int start_y = 21295;
            int width = 2256;
            int height = 2608;
            int cnt = 0;
            //しししし

            for(int i = start_y; i < src.Height - height; i+=height)
            {
                for(int j = start_x; j >= 0; j -= width)
                {
                    roi = new Mat(src, new Rect(j, i, width, height));
                    Cv2.ImWrite("./samples/"+cnt+".bmp", roi);
                    cnt++;
                }
            }

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
