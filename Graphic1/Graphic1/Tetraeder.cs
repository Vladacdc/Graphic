using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphic1
{
    class Tetraeder
    {
        Point3D[] arrP;
        double Size { get; set; }

        Tetraeder()
        {
            arrP = new Point3D[4];
        }
        Tetraeder(Tetraeder t)
        {
            arrP = new Point3D[4];
            for (int i = 0; i < this.arrP.Length; i++)
            {
                this.arrP[i] = t.arrP[i];
            }
            this.Size = t.Size;
        }
        Tetraeder(Point3D point, double Size)
        {
            this.arrP[0] = new Point3D(point.X, point.Y, point.Z, point.W);
            this.arrP[1] = new Point3D(point.X + (float)Size, point.Y, point.Z, point.W);
            this.arrP[2] = new Point3D(point.X + (float)Size / 2, point.Y, point.Z + (float)(Size * Math.Sqrt(3)) / 2, point.W);
            this.arrP[3] = new Point3D(point.X + (float)Size / 2, (float)(point.Y + Math.Sqrt(2 / 3) * Size), (float)(point.Z + Size * Math.Sqrt(3) / 6), point.W);
        }

        public static void Draw(Tetraeder Tetr, Graphics g, Pen pen, Point3D vector = null)
        {
            Tetraeder temp = new Tetraeder(Tetr);
            temp = Isometric(temp);
            if (vector != null)
            {
                temp = Tetraeder.MoveOnVector(temp, vector.X, vector.Y, vector.Z);
            }

            pen.Color = Color.Red;
            g.DrawLine(pen, (PointF)temp.arrP[0], (PointF)temp.arrP[1]);
            g.DrawLine(pen, (PointF)temp.arrP[1], (PointF)temp.arrP[3]);
            g.DrawLine(pen, (PointF)temp.arrP[0], (PointF)temp.arrP[3]);
            pen.Color = Color.Blue;
            g.DrawLine(pen, (PointF)temp.arrP[0], (PointF)temp.arrP[2]);
            g.DrawLine(pen, (PointF)temp.arrP[1], (PointF)temp.arrP[2]);
            pen.Color = Color.Green;
            g.DrawLine(pen, (PointF)temp.arrP[2], (PointF)temp.arrP[3]);
        }

        public static Tetraeder AngleX(Tetraeder Tetr, float angle)
        {
            Tetraeder temp = new Tetraeder(Tetr);
            for (int i = 0; i < Tetr.arrP.Length; i++)
            {
                temp.arrP[i] = MatrixManager.OnAngleX(temp.arrP[i], angle);
            }
            return temp;
        }

        public static Tetraeder AngleY(Tetraeder Tetr, float angle)
        {
            Tetraeder temp = new Tetraeder(Tetr);
            for (int i = 0; i < Tetr.arrP.Length; i++)
            {
                temp.arrP[i] = MatrixManager.OnAngleY(temp.arrP[i], angle);
            }
            return temp;
        }

        public static Tetraeder AngleZ(Tetraeder Tetr, float angle)
        {
            Tetraeder temp = new Tetraeder(Tetr);
            for (int i = 0; i < Tetr.arrP.Length; i++)
            {
                temp.arrP[i] = MatrixManager.OnAngleZ(temp.arrP[i], angle);
            }
            return temp;
        }

        public static Tetraeder MoveOnVector(Tetraeder Tetr, float a, float b, float c)
        {
            Tetraeder temp = new Tetraeder(Tetr);
            for (int i = 0; i < Tetr.arrP.Length; i++)
            {
                temp.arrP[i] = MatrixManager.MoveOnVector(temp.arrP[i], a, b, c);
            }
            return temp;
        }

        public static Tetraeder Isometric(Tetraeder Tetr)
        {
            Tetraeder temp = new Tetraeder(Tetr);           
            temp = AngleY(temp, (float)120 * 180 / (float)Math.PI);
            temp = AngleX(temp, (float)135 * 180 / (float)Math.PI);
            return temp;
        }
    }
}
