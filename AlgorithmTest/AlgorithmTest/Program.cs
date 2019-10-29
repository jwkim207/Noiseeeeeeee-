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
                angle = -(90 - angle);


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

        //static Mat pre_Prosessing()
        //{
        //    Mat fdfdfd= new Mat();

        //    return fdfdfd;  이미지와 배열을 리턴해줘야한다.
        //}//전처리하는부분 

        static void Main(string[] args)
        {
            Mat src = Cv2.ImRead("./samples2/209.bmp");
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
            Cv2.Threshold(gray, binary, 40, 255, ThresholdTypes.Binary);
            Cv2.ImShow("aa", binary);

            // 존좋탱
            //Point[][] contour;
            //HierarchyIndex[] hieracy;

            //Cv2.FindContours(binary, out contour, out hieracy, RetrievalModes.CComp, ContourApproximationModes.ApproxSimple);
            //Cv2.DrawContours(src, contour, -1, new Scalar(0, 0, 255), 3);
            //Cv2.ImShow("cc", src);

            // 진우탱 : 레이블을 계산 - 2개 초과이면 불량품
            Mat ds = gray.Clone();
            Mat noise = new Mat();
            Cv2.Threshold(ds, ds, 195, 255, ThresholdTypes.Binary);
            Cv2.BitwiseXor(binary, ds, noise);
            Cv2.BitwiseXor(noise, ds, noise);
            Cv2.ImShow("noise", noise);

            Point[][] contour;
            HierarchyIndex[] hieracy;

            Cv2.FindContours(ds, out contour, out hieracy, RetrievalModes.CComp, ContourApproximationModes.ApproxNone);
            Cv2.DrawContours(src, contour, -1, new Scalar(0, 0, 255), 3);
            Cv2.ImShow("linecheck", src);

            Console.WriteLine("개수 : " + contour.Length);
            if (contour.Length > 2)
                Console.WriteLine("불량품");
            else
                Console.WriteLine("양품");


            //Cv2.ImShow("bb", binary - gray);
            //Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel, new Point(-1, -1), 5);
            //Cv2.MorphologyEx(binary, binary, MorphTypes.Erode, kernel, new Point(-1, -1), 20);
            //Cv2.MorphologyEx(binary, binary, MorphTypes.Dilate, kernel, new Point(-1, -1), 20);


            //Cv2.FindContours(binary, out contours, out hier, RetrievalModes.External, ContourApproximationModes.ApproxTC89KCOS);


            //Point[] point_rect = new Point[4];

            //for (int i = 0; i < contours.Length; i++)
            //{
            //    double perimeter = Cv2.ArcLength(contours[i], true);
            //    double epsilon = perimeter * 0.04;

            //    Point[] approx = Cv2.ApproxPolyDP(contours[i], epsilon, true);
            //    Point[][] draw_approx = new Point[][] { approx };

            //    // Cv2.DrawContours(dst, contours, -1, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);
            //    Cv2.DrawContours(dst, draw_approx, -1, new Scalar(255, 0, 0), 2, LineTypes.AntiAlias);


            //    // 좌표를 표시하는 부분
            //    for (int j = 0; j < approx.Length; j++)
            //    {
            //        Cv2.Circle(dst, approx[j], 1, new Scalar(0, 0, 255), 3);

            //        point_rect[j] = approx[j];
            //        Console.WriteLine(point_rect[j]);
            //    }
            //}

            ////ROI를 검출하는 로직  x의 min, max 과 y의 min,max 찾기
            //int max_x = 0;
            //int min_x = int.MaxValue;
            //int max_y = 0;
            //int min_y = int.MaxValue;

            //angle = calculate_angle(point_rect);
            //Roi = make_ROI(dst, min_y, max_y, min_x, max_x, point_rect);
            //rotate = Rotate_rect(Roi, angle);

            //Console.WriteLine(angle);

            //Cv2.ImShow("src", src);
            //Cv2.ImShow("binary", binary);
            //Cv2.ImShow("dst", dst);
            //Cv2.ImShow("Roi", Roi);
            //Cv2.ImShow("Rotate", rotate);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();


        }

    }
}