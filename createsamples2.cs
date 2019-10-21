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
            Mat src = Cv2.ImRead("clip2.bmp", ImreadModes.Grayscale);

            Mat pry1 = new Mat();
            Mat binary = new Mat();
            Mat roi = new Mat();
            int start_x = 3365;
            int start_y = 274;
            int width = 1015;
            int height = 499;
            int cnt = 0;

            for (int i = start_y; i < src.Height - height; i += height)
            {
                for (int j = start_x-width; j >= 0; j -= width)
                {
                    roi = new Mat(src, new Rect(j-10, i-10, width+10, height+10));
                    Cv2.ImWrite("./samples2/" + cnt + ".bmp", roi);
                    if (cnt == 98)
                    {
                        height = 493;
                    }
                    else if (cnt == 145)
                    {
                        height = 497;
                    }
                    else if (cnt == 175)
                    {
                        width = 1012;
                        height = 501;
                    }
                    else if (cnt == 190)
                    {
                        width = 1005;
                    }
                    else if (cnt == 204)
                    {
                        width = 975;
                    }
                    cnt++;
                }
            }

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
