using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphic1
{
    public class Point3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Point3D()
        {

        }

        public Point3D(PointF point, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Point3D(Point3D point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.W = point.W;
        }

        public Point3D(float X, float Y, float Z, float W = 1)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        public static explicit operator PointF(Point3D point)
        {
            //point = ToIsometric(point);
            return new PointF(point.X / point.W, point.Y / point.W);
        }

        public override string ToString()
        {
            return X.ToString() + ", " + Y.ToString() + ", " + Z.ToString();
        }

        public static explicit operator Point3D(string str)
        {
            string[] temp = str.Split(',');
            if 
                (temp.Length != 3) throw new ArgumentException();
            else
                return new Point3D((float)Convert.ToDouble(temp[0]), (float)Convert.ToDouble(temp[1]), (float)Convert.ToDouble(temp[2]));
        }
       
        public bool Equals(Point3D obj)
        {
            return this.X == obj.X && this.Y == obj.Y && this.Z == obj.Z && this.W == obj.W;
        }

        
    }
}
