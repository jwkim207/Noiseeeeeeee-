using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.IO;

namespace Tiled_image_correction
{
    class Program
    {
        public static double calculate_angle(Point[] point_rect)
        {

            double y_change = point_rect[1].X - point_rect[0].X;
            double x_change = point_rect[1].Y - point_rect[0].Y;

            double angle = Math.Atan2(y_change, x_change) * 180 / Math.PI; ;
            Console.WriteLine("Atan에서 계산된 {0}", angle);
            if (0 < angle)
                angle = -angle;
            else if (angle == 0)
                return 0;
            else
                angle = 180 - (90 - angle);


            return angle;
        }

        //이미지의 각도를 조절하는 함수
        public static Mat Rotate_rect(Mat src, double angle)
        {
            Mat rotate = src.Clone();
            var imageCenter = new Point2f(src.Cols / 2f, src.Rows / 2f);
            Mat matrix = Cv2.GetRotationMatrix2D(imageCenter, -angle, 1);
            Cv2.WarpAffine(src, rotate, matrix, src.Size());
            return rotate;
        }


        public static Mat make_ROI(Mat src, int min_y, int max_y, int min_x, int max_x, Point[] point_rect)
        {

            for (int i = 0; i < point_rect.Length; i++)
            {
                if (point_rect[i].X > max_x)
                    max_x = point_rect[i].X;

                if (point_rect[i].X < min_x)
                    min_x = point_rect[i].X;
            }

            for (int i = 0; i < point_rect.Length; i++)
            {
                if (point_rect[i].Y > max_y)
                    max_y = point_rect[i].Y;

                if (point_rect[i].Y < min_y)
                    min_y = point_rect[i].Y;
            }
            Mat Roi = src.SubMat(min_y, max_y, min_x, max_x);

            return Roi;
        }


        static void Main(string[] args)
        {

            // 파일읽어오는 부분
            //string path = @"C:\Users\scaf7\Downloads\sample3";

            //DirectoryInfo dir = new DirectoryInfo(path);

            //foreach (var item in dir.GetFiles())
            //{
            //    string file_path = item.FullName;

            //    Mat src = new Mat();
            //}
            Mat src = Cv2.ImRead("./samples2/21.bmp");


            Mat src28 = Cv2.ImRead("./samples2/28.bmp");
            Mat src29 = Cv2.ImRead("./samples2/29.bmp");
            Mat dst2 = new Mat();
            Mat gray = new Mat(src.Size(), MatType.CV_8UC1);
            Mat binary = new Mat(src.Size(), MatType.CV_8UC1);
            Mat dst = src.Clone();
            Mat rotate = new Mat();
            Mat Roi = new Mat();

            double angle = default;

            Point[][] contours;
            HierarchyIndex[] hier;



            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));

            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, binary, 30, 255, ThresholdTypes.Binary);
            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel, new Point(-1, -1), 5);
            Cv2.MorphologyEx(binary, binary, MorphTypes.Erode, kernel, new Point(-1, -1), 20);
            Cv2.MorphologyEx(binary, binary, MorphTypes.Dilate, kernel, new Point(-1, -1), 20);


            Cv2.FindContours(binary, out contours, out hier, RetrievalModes.External, ContourApproximationModes.ApproxTC89KCOS);


            Point[] point_rect = new Point[4];

            for (int i = 0; i < contours.Length; i++)
            {
                double perimeter = Cv2.ArcLength(contours[i], true);
                double epsilon = perimeter * 0.04;

                Point[] approx = Cv2.ApproxPolyDP(contours[i], epsilon, true);

                approx[1].Y = approx[0].Y;
                approx[2].X = approx[1].X;
                approx[3].Y = approx[2].Y;
                approx[0].X = approx[3].X;

                Point[][] draw_approx = new Point[][] { approx };


                // Cv2.DrawContours(dst, contours, -1, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);
                Cv2.DrawContours(dst, draw_approx, -1, new Scalar(255, 0, 0), 2, LineTypes.AntiAlias);


                // 좌표를 표시하는 부분
                for (int j = 0; j < approx.Length; j++)
                {
                    Cv2.Circle(dst, approx[j], 1, new Scalar(0, 0, 255), 3);

                    point_rect[j] = approx[j];
                    Console.WriteLine(point_rect[j]);
                }
            }

            //ROI를 검출하는 로직  x의 min, max 과 y의 min,max 찾기
            //여기서 중심점도 찾는다.
            int max_x = 0;
            int min_x = int.MaxValue;
            int max_y = 0;
            int min_y = int.MaxValue;



            angle = calculate_angle(point_rect);
            //Roi = make_ROI(dst, min_y, max_y, min_x, max_x, point_rect);
            //rotate = Rotate_rect(Roi, angle);
            rotate = Rotate_rect(dst, angle);
            Roi = make_ROI(rotate, min_y, max_y, min_x, max_x, point_rect);


            Console.WriteLine(angle);

            Mat mask = null;
            int dtype = -1;
            Cv2.Subtract(src, src, dst2, mask, dtype);
            //Cv2.ImShow("src", src);
            //Cv2.ImShow("binary", binary);
            //Cv2.ImShow("dst", dst);
            //Cv2.ImShow("Rotate", rotate);
            Cv2.ImShow("Roi", Roi);
            //Cv2.ImShow("result", dst2);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
            //하위
        }
    }
}