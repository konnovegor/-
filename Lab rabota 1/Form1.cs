using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_rabota_1
{
    public partial class Form1 : Form

    {
        class Point
        {
            public double x;
            public double y;
            public double z;
            public double H;

            static public Point Parse(string str)
            {
                Point point = new Point();

                string[] st = str.Split(' ');

                point.x = double.Parse(st[0]);
                point.y = double.Parse(st[1]);
                point.z = double.Parse(st[2]);
                point.H = double.Parse(st[3]);

                return point;
            }
        }

        class Line
        {
            public int begin;
            public int end;

            static public Line Parse(string str)
            {
                Line line = new Line();

                string[] a = str.Split(' ');

                line.begin = int.Parse(a[0]);
                line.end = int.Parse(a[1]);

                return line;
            }
        }

        List<Point> fp = new List<Point>();
        List<Line> fl = new List<Line>();


        Graphics g;

        void Reading()
        {
            StreamReader a = new StreamReader("C:\\Users\\konno\\Desktop\\работы в с\\Програмирование\\кг\\points.txt");

            var str = a.ReadLine();
            while (true)
            {

                if (a.EndOfStream) break;
                if (str == "*") break;
                fp.Add(Point.Parse(str));
                str = a.ReadLine();
            }
            str = a.ReadLine();
            while (true)
            {

                if (a.EndOfStream) break;
                if (str == "*") break;
                fl.Add(Line.Parse(str));
                str = a.ReadLine();
            }

        }

        void Accommodation()
        {
            double max1 = fp[0].x;
            double min1 = fp[0].x;
            double max2 = fp[0].y;
            double min2 = fp[0].y;

            for (int i = 0; i < fp.Count; i++)
            {
                if (fp[i].x > max1) max1 = fp[i].x;
                if (fp[i].y > max2) max2 = fp[i].y;

                if (fp[i].x < min1) min1 = fp[i].x;
                if (fp[i].y < min2) min2 = fp[i].y;
            }

            double q;

            if (pictureBox1.Height / (max2 - min2) > pictureBox1.Width / (max1 - min1))
                q = pictureBox1.Width / (max1 - min1);
            else q = pictureBox1.Height / (max2 - min2);


            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].x -= min1;
                fp[i].y -= min2;
                
                fp[i].x *= q;
                fp[i].y *= q;
                fp[i].z *= q;
            }
        }

        void Drowing()
        {
            g.Clear(Color.White);

            {
                for (int i = 0; i < fl.Count; i++)
                {
                    g.DrawLine(new Pen(Color.Black, 2),
                        (float)fp[fl[i].begin - 1].x, (float)fp[fl[i].begin - 1].y,
                        (float)fp[fl[i].end - 1].x, (float)fp[fl[i].end - 1].y);
                }
            }
            pictureBox1.Invalidate();
        }

        void Displacement(double x1, double y1, double z1)
        {

            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].x = fp[i].x + fp[i].H * x1;
                fp[i].y = fp[i].y + fp[i].H * y1;
                fp[i].z = fp[i].z + fp[i].H * z1;
            }
        }

        void Scaling(double x2, double y2, double z2)
        {

            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].x = fp[i].x * x2;
                fp[i].y = fp[i].y * y2;
                fp[i].z = fp[i].z * z2;
            }
        }

        void Turn(double alp, double bet, double gam)
        {

            double newX, newY, newZ;
            for (int i = 0; i < fp.Count; i++)
            {
                newY = fp[i].y * Math.Cos(alp * 180 / Math.PI) - fp[i].z * Math.Sin(alp * 180 / Math.PI);
                newZ = fp[i].y * Math.Sin(alp * 180 / Math.PI) + fp[i].z * Math.Cos(alp * 180 / Math.PI);
                fp[i].y = newY;
                fp[i].z = newZ;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                newX = fp[i].x * Math.Cos(bet * 180 / Math.PI) + fp[i].z * Math.Sin(bet * 180 / Math.PI);
                newZ = -fp[i].x * Math.Sin(bet * 180 / Math.PI) + fp[i].z * Math.Cos(bet * 180 / Math.PI);
                fp[i].x = newX;
                fp[i].z = newZ;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                newY = fp[i].x * Math.Sin(gam * 180 / Math.PI) + fp[i].y * Math.Cos(gam * 180 / Math.PI);
                newX = fp[i].x * Math.Cos(gam * 180 / Math.PI) - fp[i].y * Math.Sin(gam * 180 / Math.PI);
                fp[i].y = newY;
                fp[i].x = newX;
            }
        }

        void Movement(double xy, double xz, double yz, double zx, double yx, double zy)
        {
            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].x = fp[i].x + fp[i].y * xy;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].x = fp[i].x + fp[i].z * xz;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].y = fp[i].y + fp[i].x * yx;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].y = fp[i].y + fp[i].z * yz;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].z = fp[i].z + fp[i].x * zx;
            }
            for (int i = 0; i < fp.Count; i++)
            {
                fp[i].x = fp[i].z + fp[i].y * zy;
            }
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            Reading();

            Accommodation();

            Drowing();
        }

        private void Масштаб_Click(object sender, EventArgs e)
        {
            Scaling(Double.Parse(tbx.Text),
                Double.Parse(tby.Text),
                Double.Parse(tbz.Text));
            Drowing();
        }

        private void Поворот_Click(object sender, EventArgs e)
        {
            Turn(Double.Parse(tba.Text),
                  Double.Parse(tbb.Text),
                  Double.Parse(tbg.Text));
            Drowing();
        }

        private void Сдвиг_Click(object sender, EventArgs e)
        {
            Movement(Double.Parse(tbxy.Text),
                              Double.Parse(tbyx.Text),
                              Double.Parse(tbxz.Text),
                              Double.Parse(tbzx.Text),
                              Double.Parse(tbyz.Text),
                              Double.Parse(tbzy.Text));
            Drowing();
        }

        private void Перенос_Click(object sender, EventArgs e)
        {
            Displacement(Double.Parse(textbxx.Text),
                Double.Parse(textbxy.Text),
                Double.Parse(textbxz.Text));
            Drowing();
        }

        private void Вмещение_Click(object sender, EventArgs e)
        {
           
            Accommodation();

            Drowing();
            
        }

    }
}
