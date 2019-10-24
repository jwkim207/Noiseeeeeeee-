using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmTest
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
            Console.WriteLine("Hi");
            Console.WriteLine("야생");
            Console.WriteLine("야생마 권용백");
            Console.WriteLine("hi");
        }
    }
}