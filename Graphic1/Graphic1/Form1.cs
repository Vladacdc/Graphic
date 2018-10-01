using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Graphic1
{
    public partial class Form1 : Form
    {
        Tetraeder tet;
        Cube cube;
        Point3D A;
        Point3D B;

        bool isRotating = false;
        bool isCube = true;

        public Form1()
        {
            InitializeComponent();

            A = new Point3D(0, 0, 0);
            B = new Point3D(100, 100, 100);
            cube = new Cube(new Point3D(100, 100, 100), 100);
            tet = new Tetraeder(new Point3D(100, 100, 100), 100);

            comboBox1.Items.AddRange(new[] { "Cube", "Tetraeder" });
            comboBox1.SelectedIndex = 0;

            textBox1.Text = cube.ArrF[0].ToString();
            textBox2.Text = cube.Size.ToString();

            //textBox1.Text = tet.arrP[0].ToString();
            //textBox2.Text = cube.Size.ToString();

            textBox3.Text = A.ToString();
            textBox4.Text = B.ToString();
            textBox5.Text = trackBar1.Value.ToString();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 2);
            Point3D mover = new Point3D(panel1.Width / 2, panel1.Height / 2, 0);

            graphics.Clear(Color.White);

            #region OXYZ
            pen.Color = Color.DarkOrange;
            graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(new Point3D(1000, 0, 0), mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, 0), mover));
            graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(new Point3D(-1000, 0, 0), mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, 0), mover));

            pen.Color = Color.Brown;
            graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(new Point3D(0, 1000, 0), mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, 0), mover));
            graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(new Point3D(0, -1000, 0), mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, 0), mover));

            pen.Color = Color.DeepPink;
            graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(new Point3D(0, 0, 1000), mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, 0), mover));
            graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(new Point3D(0, 0, -1000), mover), (PointF)MatrixManager.Isometric(new Point3D(0, 0, 0), mover));
            #endregion

            #region Line
            pen.DashStyle = DashStyle.Dot;
            pen.Color = Color.Black;
            //////////////
            if (!A.Equals(B))
            {
                Point3D tempA = new Point3D(A);
                Point3D tempB = new Point3D(B);


                while (Math.Abs(tempA.X) < 1000 && Math.Abs(tempA.Y) < 1000 && Math.Abs(tempA.Z) < 1000)
                {
                    tempA = MatrixManager.MoveOnVector(tempA, B.X - A.X, B.Y - A.Y, B.Z - A.Z);
                }

                while (Math.Abs(tempB.X) < 1000 && Math.Abs(tempB.Y) < 1000 && Math.Abs(tempB.Z) < 1000)
                {
                    tempB = MatrixManager.MoveOnVector(tempB, -(B.X - A.X), -(B.Y - A.Y), -(B.Z - A.Z));
                }

                graphics.DrawLine(pen, (PointF)MatrixManager.Isometric(tempA, mover), (PointF)MatrixManager.Isometric(tempB, mover));
            }
            /////////////
            pen.DashStyle = DashStyle.Solid;
            #endregion

            if (isCube)
            {
                Cube.Draw(cube, graphics, pen, mover);
            }
            else
            {
                Tetraeder.Draw(tet, graphics, pen, mover);
            }
        }
      
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            A = (Point3D)textBox3.Text;
            B = (Point3D)textBox4.Text;

            if (isCube)
            {
                cube = new Cube((Point3D)textBox1.Text, Convert.ToDouble(textBox2.Text));
            }
            else
            {
                tet = new Tetraeder((Point3D)textBox1.Text, Convert.ToDouble(textBox2.Text));
            }

            panel1_Paint(sender, new PaintEventArgs(CreateGraphics(), panel1.ClientRectangle));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (A.Equals(B)) return;

            isRotating = !isRotating;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = true;

            trackBar1.Enabled = false;

            textBox5.Enabled = false;

            comboBox1.Enabled = false;
            
            backgroundWorker1.RunWorkerAsync(trackBar1.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isRotating = !isRotating;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;

            trackBar1.Enabled = true;

            textBox5.Enabled = true;

            comboBox1.Enabled = true;
        }

        private void Rotate(object sender, EventArgs e, int value)
        {
            Point3D tempA = new Point3D(A);
            Point3D tempB = new Point3D(B);
            Point3D toStart = MatrixManager.PointsToStart(A, B);
            Cube tempCube = new Cube(cube);
            Tetraeder tempTet = new Tetraeder(tet);

            if (!toStart.Equals(new Point3D(0, 0, 0)))
            {
                tempA = MatrixManager.MoveOnVector(tempA, toStart.X, toStart.Y, toStart.Z);
                tempB = MatrixManager.MoveOnVector(tempB, toStart.X, toStart.Y, toStart.Z);

                if (isCube) tempCube = Cube.MoveOnVector(tempCube, toStart.X, toStart.Y, toStart.Z);
                else tempTet = Tetraeder.MoveOnVector(tempTet, toStart.X, toStart.Y, toStart.Z);
            }
            else
            {
                tempA = toStart;
            }

            //At this moment tempA is on 0,0,0
            //B can be anywere
            //XY->angle to Z
            //XZ->angle to Y
            //YZ->angle to X

            if (tempB.X == 0 && tempB.Z == 0)
            {
                //Line is OY
                if (isCube) tempCube = Cube.AngleY(tempCube, value);
                else tempTet = Tetraeder.AngleY(tempTet, value);
            }
            else if (tempB.Y == 0 && tempB.Z == 0)
            {
                //Line is OX
                if (isCube) tempCube = Cube.AngleX(tempCube, value);
                else tempTet = Tetraeder.AngleX(tempTet, value);
            }
            else if (tempB.X == 0 && tempB.Y == 0)
            {
                //Line is OZ
                if (isCube) tempCube = Cube.AngleZ(tempCube, value);
                else tempTet = Tetraeder.AngleZ(tempTet, value);
            }
            else if (tempB.X != 0 && tempB.Y != 0 && tempB.Z != 0)
            {
                float angleX = (float)Math.Atan2(tempB.Y, tempB.Z) * 180 / (float)Math.PI;

                tempB = MatrixManager.OnAngleX(tempB, angleX);

                float angleY = (float)Math.Atan2(tempB.Z, tempB.X) * 180 / (float)Math.PI;

                if (isCube)
                {
                    tempCube = Cube.AngleX(tempCube, angleX);
                    tempCube = Cube.AngleY(tempCube, angleY);

                    tempCube = Cube.AngleX(tempCube, value);

                    tempCube = Cube.AngleY(tempCube, -angleY);
                    tempCube = Cube.AngleX(tempCube, -angleX);
                }
                else
                {
                    tempTet = Tetraeder.AngleX(tempTet, angleX);
                    tempTet = Tetraeder.AngleY(tempTet, angleY);

                    tempTet = Tetraeder.AngleX(tempTet, value);

                    tempTet = Tetraeder.AngleY(tempTet, -angleY);
                    tempTet = Tetraeder.AngleX(tempTet, -angleX);
                }
            }
            else if (tempB.X == 0)
            {
                float angleX = (float)Math.Atan2(tempB.Y, tempB.Z) * 180 / (float)Math.PI;
                if (isCube)
                {
                    tempCube = Cube.AngleX(tempCube, angleX);

                    tempCube = Cube.AngleZ(tempCube, value);

                    tempCube = Cube.AngleX(tempCube, -angleX);
                }
                else
                {
                    tempTet = Tetraeder.AngleX(tempTet, angleX);

                    tempTet = Tetraeder.AngleZ(tempTet, value);

                    tempTet = Tetraeder.AngleX(tempTet, -angleX);
                }
            }
            else if (tempB.Y == 0)
            {
                float angleY = (float)Math.Atan2(tempB.Z, tempB.X) * 180 / (float)Math.PI;

                if (isCube)
                {
                    tempCube = Cube.AngleY(tempCube, angleY);

                    tempCube = Cube.AngleZ(tempCube, value);

                    tempCube = Cube.AngleY(tempCube, -angleY);
                }
                else
                {
                    tempTet = Tetraeder.AngleY(tempTet, angleY);

                    tempTet = Tetraeder.AngleZ(tempTet, value);

                    tempTet = Tetraeder.AngleY(tempTet, -angleY);
                }
            }
            else if (tempB.Z == 0)
            {
                float angleZ = (float)Math.Atan2(tempB.Y, tempB.X) * 180 / (float)Math.PI;
                if (isCube)
                {
                    tempCube = Cube.AngleZ(tempCube, angleZ);

                    tempCube = Cube.AngleY(tempCube, value);

                    tempCube = Cube.AngleZ(tempCube, -angleZ);
                }
                else
                {
                    tempTet = Tetraeder.AngleZ(tempTet, angleZ);

                    tempTet = Tetraeder.AngleY(tempTet, value);

                    tempTet = Tetraeder.AngleZ(tempTet, -angleZ);
                }
            }

            if (isCube) cube = Cube.MoveOnVector(tempCube, -toStart.X, -toStart.Y, -toStart.Z);
            else tet = Tetraeder.MoveOnVector(tempTet, -toStart.X, -toStart.Y, -toStart.Z);
            panel1_Paint(sender, new PaintEventArgs(CreateGraphics(), panel1.DisplayRectangle));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox5.Text = trackBar1.Value.ToString();
        }
      
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != string.Empty)
            {
                trackBar1.Value = Convert.ToInt32(textBox5.Text);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000 / 30);
                int value = (int)e.Argument;
                Rotate(sender, e, value);
                if (!isRotating)
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                isCube = true;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                isCube = false;
            }
            panel1_Paint(sender, new PaintEventArgs(CreateGraphics(), panel1.ClientRectangle));            
        }
    }
}
