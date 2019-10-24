using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
namespace AlgorithmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat m = new Mat("./samples2/0.bmp");
            Cv2.ImShow("hi",m);

            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
