namespace Graphic1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Drawing;

    class Cube
    {
        public double Size { get; private set; }

        public Point3D[] ArrF { get; set; }
        public Point3D[] ArrB { get; set; }

        private Cube()
        {
            ArrF = new Point3D[4];
            ArrB = new Point3D[4];
        }

        public Cube(Point3D a, double Size)
        {
            ArrF = new Point3D[4];
            ArrB = new Point3D[4];

            this.Size = Size;

            ArrF[0] = a;
            ArrB[0] = new Point3D(ArrF[0].X, ArrF[0].Y, ArrF[0].Z + (float)Size);

            ArrF[1] = new Point3D(ArrF[0].X + (float)Size, ArrF[0].Y, ArrF[0].Z);
            ArrF[2] = new Point3D(ArrF[1].X, ArrF[1].Y + (float)Size, ArrF[0].Z);
            ArrF[3] = new Point3D(ArrF[0].X, ArrF[2].Y, ArrF[0].Z);


            ArrB[1] = new Point3D(ArrB[0].X + (float)Size, ArrB[0].Y, ArrB[0].Z);
            ArrB[2] = new Point3D(ArrB[1].X, ArrB[1].Y + (float)Size, ArrB[0].Z);
            ArrB[3] = new Point3D(ArrB[0].X, ArrB[2].Y, ArrB[0].Z);
        }

        public Cube(Cube a)
        {
            ArrF = new Point3D[4];
            ArrB = new Point3D[4];

            for (int i=0; i<4;i++)
            {
                ArrF[i] = a.ArrF[i];
                ArrB[i] = a.ArrB[i];
            }
            Size = a.Size;
        }

        public static void Draw(Cube cube, Graphics g, Pen pen, Point3D vector=null)
        {
            Cube temp = new Cube(cube);
            temp = Isometric(temp);
            if (vector!=null)
            {
                temp = Cube.MoveOnVector(temp, vector.X, vector.Y, vector.Z);
            }
                      
            for (int i = 1; i < cube.ArrF.Length; i++)
            {
                pen.Color = Color.Red;
                g.DrawLine(pen, (PointF)temp.ArrF[i - 1], (PointF)temp.ArrF[i]);
                pen.Color = Color.Blue;
                g.DrawLine(pen, (PointF)temp.ArrB[i - 1], (PointF)temp.ArrB[i]);
                pen.Color = Color.Green;
                g.DrawLine(pen, (PointF)temp.ArrF[i - 1], (PointF)temp.ArrB[i - 1]);
            }

            pen.Color = Color.Red;
            g.DrawLine(pen, (PointF)temp.ArrF[0], (PointF)temp.ArrF[3]);
            pen.Color = Color.Blue;
            g.DrawLine(pen, (PointF)temp.ArrB[0], (PointF)temp.ArrB[3]);
            pen.Color = Color.Green;
            g.DrawLine(pen, (PointF)temp.ArrF[3], (PointF)temp.ArrB[3]);            

        }

        public static Cube AngleX(Cube cube, float angle)
        {
            Cube temp = new Cube(cube);
            for (int i = 0; i < cube.ArrF.Length; i++)
            {
                temp.ArrF[i] = MatrixManager.OnAngleX(temp.ArrF[i], angle);
                temp.ArrB[i] = MatrixManager.OnAngleX(temp.ArrB[i], angle);
            }
            return temp;
        }

        public static Cube AngleY(Cube cube, float angle)
        {
            Cube temp = new Cube(cube);
            for (int i = 0; i < cube.ArrF.Length; i++)
            {
                temp.ArrF[i] = MatrixManager.OnAngleY(temp.ArrF[i], angle);
                temp.ArrB[i] = MatrixManager.OnAngleY(temp.ArrB[i], angle);
            }
            return temp;
        }

        public static Cube AngleZ(Cube cube, float angle)
        {
            Cube temp = new Cube(cube);
            for (int i = 0; i < cube.ArrF.Length; i++)
            {
                temp.ArrF[i] = MatrixManager.OnAngleZ(temp.ArrF[i], angle);
                temp.ArrB[i] = MatrixManager.OnAngleZ(temp.ArrB[i], angle);
            }
            return temp;
        }

        public static Cube MoveOnVector(Cube cube, float a, float b, float c)
        {
            Cube temp = new Cube(cube);
            for (int i = 0; i < cube.ArrF.Length; i++)
            {
                temp.ArrF[i] = MatrixManager.MoveOnVector(temp.ArrF[i], a, b, c);
                temp.ArrB[i] = MatrixManager.MoveOnVector(temp.ArrB[i], a, b, c);
            }
            return temp;
        }

        public static Cube Isometric(Cube cube)
        {
            Cube temp = new Cube(cube);
            //temp = MoveOnVector(temp, -temp.ArrF[0].X, -temp.ArrF[0].Y, -temp.ArrF[0].Z);
            temp = AngleY(temp, (float)120 * 180 / (float)Math.PI);
            temp = AngleX(temp, (float)135 * 180 / (float)Math.PI);                       
            //temp = MoveOnVector(temp, cube.ArrF[0].X, temp.ArrF[0].Y, temp.ArrF[0].Z);
            return temp;
        }
    }
}
