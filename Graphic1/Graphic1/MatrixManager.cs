using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic1
{
    static class MatrixManager
    {

        public static float[,] GetCleanMatrix()
        {
            float[,] res = new float[4, 4];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i, i] = (float)1;
            }
            return res;
        }

        public static Point3D Mult3D(float[,] matrix, Point3D point)
        {
            float[] arr = { 0, 0, 0, 0 };
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    switch (i)
                    {
                        case 0:
                            {
                                arr[j] += point.X * matrix[i, j];
                                break;
                            }
                        case 1:
                            {
                                arr[j] += point.Y * matrix[i, j];
                                break;
                            }
                        case 2:
                            {
                                arr[j] += point.Z * matrix[i, j];
                                break;
                            }
                        case 3:
                            {
                                arr[j] += point.W * matrix[i, j];
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            return new Point3D(arr[0], arr[1], arr[2], arr[3]);
        }

        public static Point3D OnAngleX(Point3D point, float angle)
        {
            angle = angle * (float)Math.PI / 180;
            float[,] matrix = GetCleanMatrix();
            matrix[1, 1] = (float)Math.Cos(angle);
            matrix[2, 2] = matrix[1, 1];
            matrix[1, 2] = (float)Math.Sin(angle);
            matrix[2, 1] = matrix[1, 2] * (-1);
            return Mult3D(matrix, point);
        }

        public static Point3D OnAngleY(Point3D point, float angle)
        {
            angle = angle * (float)Math.PI / 180;
            float[,] matrix = GetCleanMatrix();
            matrix[0, 0] = (float)Math.Cos(angle);
            matrix[2, 2] = matrix[0, 0];
            matrix[0, 2] = (float)Math.Sin(angle) * (-1);
            matrix[2, 0] = matrix[0, 2] * (-1);
            return Mult3D(matrix, point);
        }

        public static Point3D OnAngleZ(Point3D point, float angle)
        {
            angle = angle * (float)Math.PI / 180;
            float[,] matrix = GetCleanMatrix();
            matrix[0, 0] = (float)Math.Cos(angle);
            matrix[1, 1] = matrix[0, 0];
            matrix[0, 1] = (float)Math.Sin(angle);
            matrix[1, 0] = matrix[0, 1] * (-1);
            return Mult3D(matrix, point);
        }

        public static Point3D MoveOnVector(Point3D point, float a, float b, float c)
        {
            float[,] matrix = GetCleanMatrix();
            matrix[3, 0] = a;
            matrix[3, 1] = b;
            matrix[3, 2] = c;
            return Mult3D(matrix, point);
        }

        public static Point3D PointsToStart(Point3D point1, Point3D point2)
        {
            Point3D res = new Point3D(-point1.X / (point2.X - point1.X), -point1.Y / (point2.Y - point1.Y), -point1.Z / (point2.Z - point1.Z));
            if (res.X == res.Y && res.X == res.Z)
            {
                return new Point3D(0, 0, 0);
            }
            else
            {
                return new Point3D(-point1.X, -point1.Y, -point1.Z);
            }
        }

        public static Point3D Isometric(Point3D point, Point3D mover = null)
        {
            //120 135 in rad
            point = OnAngleY(point, (float)120 * 180 / (float)Math.PI);
            point = OnAngleX(point, (float)135 * 180 / (float)Math.PI);
            if (mover != null)
            {
                point = MoveOnVector(point, mover.X, mover.Y, mover.Z);
            }
            return point;
        }

    }
}
