using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace galton2
{
    public partial class Form1 : Form
    {
        private Galton_erfen g;
        private int bnum;
        private int i = 0;
        private int p_range = 10;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = new Galton_erfen();
            bnum = g.ballnum;
            textBox2.Text = g.ballnum.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            g = new Galton_erfen(bnum,10);
            g.simu2();
            g.display(textBox1, Galton_erfen.searchwidth + 1);
            g.drawfig(pictureBox1, Galton_erfen.searchwidth);
            textBox3.Text = g.errorcount.ToString();
            //g.pos_range = p_range;
            //g.simu();
            //g.display(textBox1, g.erfentimes);
            //g.drawfig(pictureBox1, g.erfentimes);
        }
        private void TextBox2_Leave(object sender, EventArgs e)
        {
            try
            {
                bnum = int.Parse(textBox2.Text);
            }
            catch
            {
                textBox2.Text = bnum.ToString();
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            ++i;
           g.gimage.Save("Q:\\workplace\\"+i.ToString()+".png");
        }

        private void TextBox4_Leave(object sender, EventArgs e)
        {
            try
            {
                p_range = int.Parse(textBox4.Text);
            }
            catch
            {
                textBox4.Text = p_range.ToString();
            }
        }
    }
    unsafe class Galton_erfen
    {
        public Bitmap gimage;
        public int ballnum;
        public int erfentimes;
        public int[] result;
        public int errorcount = 0;
        private const double g = 9.8;
        public const int searchwidth = 120;
        public double r = 0.2;
        public int pos_range=10;
        //debug
        public double[] t = new double[100000];
        private Random nr=new Random();
        public Galton_erfen(int num = 1000, int times = 100)
        {
            ballnum = num;
            erfentimes = times;
            result = new int[times * 100 + 1];
            for (int i = 0; i < times + 1; i++) result[i] = 0;
        }
        private double normrnd(double mu,double sigma)
        {
            double r1 = nr.NextDouble();
            double r2 = nr.NextDouble();
            double x;
            x = Math.Sqrt(-2 * Math.Log(r1)) * Math.Cos(2 * Math.PI * r2);
            return sigma*(x+mu);
        }
        public void simu()
        {
            Random r = new Random();

            for (int i = 0; i < ballnum; i++)
            {
                int state = r.Next(pos_range) - pos_range / 2;
                for (int j = 0; j < erfentimes; j++)
                {
                    int tmp = r.Next() % 2;
                    if (tmp == 1) state += 1;

                }
                if ((state >= 0) && (state < erfentimes))
                    result[state] += 1;
            }

            //debug
            //int[] t2 = new int[erfentimes];

            //norm
            //for (int i = 0; i < ballnum; i++)
            //{
            //    t[i]=normrnd(0, pos_range / 4.0);
            //    int state =(int)t[i];
                
            //    for (int j = 0; j < erfentimes; j++)
            //    {
            //        int tmp = r.Next() % 2;
            //        if (tmp == 1) state += 1;

            //    }
            //    if ((state >= 0) && (state < erfentimes))
            //    {
            //        result[state] += 1;
            //        t2[state - (int)t[i]] += 1;
            //    }
            //}

            //debug
            //StreamWriter w1 = new StreamWriter("Q:\\workplace\\1.txt");
            //StreamWriter w2 = new StreamWriter("Q:\\workplace\\2.txt");
            //StreamWriter w3 = new StreamWriter("Q:\\workplace\\3.txt");
            //for (int k = 0; k < ballnum; ++k)
            //{
            //    w1.WriteLine(t[k].ToString(".0000"));
            //}
            //for (int k = 0; k < erfentimes; ++k)
            //{
            //    w2.WriteLine(t2[k].ToString(".0000"));
            //}
            //for (int k = 0; k < erfentimes; ++k)
            //{
            //    w3.WriteLine(result[k].ToString(".0000"));
            //}
            //w1.Close();
            //w2.Close();
            //w3.Close();
        }
        public void display(TextBox t,int tlen)
        {
            t.Text = "";
            for (int i = 0; i < tlen; i++)
                //t.Text += i + "  " + result[i] + "\r\n";
                t.Text += result[i] + "\r\n";
        }
        public void drawfig(PictureBox p,int tlen)
        {
            //图框大小
            int width = p.Width;
            int height = p.Height;
            int wnum = tlen+ 1;

            int wint = p.Width / wnum;
            float hscale = height / (float)find_max();
            //循环画点
            int penwidth = 2;
            gimage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(gimage);
            Pen pe = new Pen(Color.Black, penwidth);

            for (int k = 0; k < tlen; ++k)
            {
                PointF p1, p2;
                p1 = new PointF();
                p1.X = wint * k;
                p1.Y = height - 1 - result[k] * hscale;
                p2 = new PointF();
                p2.X = wint * (k + 1);
                p2.Y = height - 1 - result[k + 1] * hscale;

                g.DrawLine(pe, p1, p2);
            }
            g.Flush();
            statistic(tlen);
            p.Image = gimage;

        }
        private int find_max()
        {
            int max = -1;
            for (int k = 0; k < searchwidth + 1; ++k)
            {
                if (result[k] > max) max = result[k];
            }
            return max;
        }
        public void simu2()
        {
            Random rd = new Random();
            const double vxmax = 10;
            //debug
            errorcount = 0;
            int[] result_wei=new int[200];
            
            for (int k = 0; k < ballnum; ++k)
            {
                double x = double.NaN;
                while (double.IsNaN(x) || (x < 0))
                {
                    //对每个球进行模拟
                    x = 50;
                    double y = 0;
                    double vx = (rd.NextDouble() * 2 - 1) * vxmax;
                    double vy = 0;
                    while (y <= erfentimes)
                    {
                        move(ref x, ref y, ref vx, ref vy);
                    }
                    if (!double.IsNaN(x) && (x >= 0))
                        result_wei[(int)x]++;
                    
                }
            }
            for (int num = 0; num < 190; num++)
                result[num] = (result_wei[num] + result_wei[num + 1]+result_wei[num+2]+result_wei[num+3] + result_wei[num + 4] + result_wei[num + 5] + result_wei[num + 6] + result_wei[num + 7] + result_wei[num + 8] + result_wei[num + 9]) / 10;

        }
        private void move(ref double x, ref double y, ref double vx, ref double vy)
        {
            double y_ = (int)(y + 1);
            double t = (-vy + Math.Sqrt(vy * vy + 2 * (y_ - y) * g)) / g;  //虚运行的时间t
            double x_vr = x + vx * t, y_vr = y_;       //虚位置     


            double xr1, xr2;               //获取虚位置左右两个可能接触的钉子的中心位置
            if (y_ % 2 == 1)
            {
                xr1 = (int)(x_vr);
                xr2 = (int)(x_vr) + 1;
            }
            else
            {
                if (x_vr - (int)(x_vr) < 0.5)
                {
                    xr1 = (int)(x_vr) - 0.5;
                    xr2 = xr1 + 1;
                }
                else
                {
                    xr1 = (int)(x_vr) + 0.5;
                    xr2 = xr1 + 1;
                }
            }

            if (x_vr - xr1 > r && xr2 - x_vr > r)
            {
                x = x_vr;
                y = y_vr;
                vy = vy + g * t;
            }
            else
            {
                double[] answer = new double[8];
                double[] a1=new double[5];
                double[] a2 = new double[5];
                for(int i = 0; i < 5; ++i)
                {
                    a1[0] = 1000;
                    a2[0] = 1000;
                }
                int slen = 0;
                Eq_Solve e=new Eq_Solve();
                a1 = e.roots_quartic_equation(
                    1.0 / 4 * g * g,
                    vy * g,
                    vx * vx + vy * vy + y * g - g * y_,
                    2 * x * vx - 2 * xr1 * vx + 2 * y * vy - 2 * vy * y_,
                    x * x + xr1 * xr1 - 2 * x * xr1 + y * y + y_ * y_ - 2 * y * y_ - r * r);
                a2 = e.roots_quartic_equation(
                    1.0 / 4 * g * g, vy * g, vx * vx + vy * vy + y * g - g * y_,
                    2 * x * vx - 2 * xr2 * vx + 2 * y * vy - 2 * vy * y_,
                    x * x + xr2 * xr2 - 2 * x * xr2 + y * y + y_ * y_ - 2 * y * y_ - r * r
                    );
                for(int k = 0; k < a1[0]; ++k)
                {
                    answer[k] = a1[k + 1];
                }
                for (int k = 0; k < a2[0]; ++k)
                {
                    answer[(int)a1[0]+k] = a2[k + 1];
                }
                slen = (int)(a1[0] + a2[0]);
                //f4(1.0 / 4 * g * g,
                //vy* g,
                //vx *vx + vy * vy + y * g - g * y_,
                //2 * x * vx - 2 * xr1 * vx + 2 * y * vy - 2 * vy * y_,
                //x* x +xr1 * xr1 - 2 * x * xr1 + y * y + y_ * y_ - 2 * y * y_ - r * r,
                //ref answer[0], ref answer[1], ref answer[2], ref answer[3]);
                //f4(1.0 / 4 * g * g, vy * g, vx * vx + vy * vy + y * g - g * y_,
                //    2 * x * vx - 2 * xr2 * vx + 2 * y * vy - 2 * vy * y_,
                //    x * x + xr2 * xr2 - 2 * x * xr2 + y * y + y_ * y_ - 2 * y * y_ - r * r,
                //    ref answer[4], ref answer[5], ref answer[6], ref answer[7]);

                double tmin = t;
                int min = 0;
                double xr;
                for (int i = 0; i < slen; i++)
                {
                    if (answer[i] > 0 && answer[i] < tmin)
                    {
                        tmin = answer[i];
                        min = i;
                    }
                }
                if (min < a1[0]) xr = xr1;
                else xr = xr2;

                t = tmin;            //获取到碰撞时间

                x = x + vx * t;
                y = y + vy * t + 0.5 * g * t * t;
                vx = vx;
                vy = vy + g * t;

                if (x >= xr)
                {
                    double xita1;
                    if (vx >= 0)
                        xita1 = 180 - Math.Atan(vy / vx);
                    else
                    {
                        double tem = vx * (-1.0);
                        xita1 = Math.Atan(vy / tem);
                    }
                    double xita2 = Math.Acos((x - xr) / r);
                    double xita = 2 * xita2 - xita1;
                    if (double.IsNaN(xita))
                    {
                        ++errorcount;
                    }
                    double v = Math.Sqrt(vx * vx + vy * vy);
                    vx = v * Math.Cos(xita);
                    vy = v * Math.Sin(xita);
                    //if (xita > 0) vy = -vy;
                }
                //if (double.IsNaN(vx))
                //{
                //    int asdczx = 3;
                //}
                //if (double.IsNaN(vy))
                //{
                //    int asdczx = 3;
                //}
                else
                {
                    double xita1;
                    if (vx >= 0)
                        xita1 = Math.Atan(vy / vx);
                    else
                    {
                        double tem = vx * (-1.0);
                        xita1 = 180 - Math.Atan(vy / tem);
                    }
                    double xita2 = Math.Acos((xr-x) / r);
                    double xita = 2 * xita2 - xita1;
                    if (double.IsNaN(xita))
                    {
                        ++errorcount;
                    }

                    double v = Math.Sqrt(vx * vx + vy * vy);
                    vx = v * Math.Cos(xita);
                    vy = v * Math.Sin(xita);
                    //if (xita > 0) vy = -vy;
                }

            }

        }
        
        private void f4(double a, double b, double c, double d, double e, ref double x1, ref double x2, ref double x3, ref double x4)
        {
            double delta1 = c * c - 3 * b * d + 12 * a * e;
            double delta2 = 2 * c * c * c - 9 * b * c * d +
                27 * a * d * d + 27 * b * b * e - 72 * a * c * e;
            double delta = Math.Pow(2, 1.0 / 3) * delta1 /
                (3 * a * Math.Pow(delta2 + Math.Sqrt((-4) * delta1 * delta1 * delta1 + delta2 * delta2), 1.0 / 3))
                + Math.Pow(delta2 + Math.Sqrt((-4) * delta1 * delta1 * delta1 + delta2 * delta2), 1.0 / 3)
                / 3.0 / Math.Pow(2, 1.0 / 3) / a;

            double s1, s2, s3;
            s1 = Math.Pow(b, 2) / (4 * Math.Pow(a, 2)) -
                2 * c / (3 * a) + delta;
            s1 = Math.Sqrt(s1);
            s2 = Math.Pow(b, 2) / (2 * Math.Pow(a, 2)) -
                 4 * c / (3 * a) - delta -
                 (-Math.Pow(b / a, 3) + 4 * b * c / Math.Pow(a, 2) - 8 * d / a) /
                 4 * s1;
            s2 = Math.Sqrt(s2);
            s3 = Math.Pow(b, 2) / (2 * Math.Pow(a, 2)) -
                 4 * c / (3 * a) - delta +
                 (-Math.Pow(b / a, 3) + 4 * b * c / Math.Pow(a, 2) - 8 * d / a) /
                 4 * s1;
            s3 = Math.Sqrt(s3);
            x1 = -b / (4 * a) - 0.5 * s1 - 0.5 * s2;
            x2 = -b / (4 * a) - 0.5 * s1 + 0.5 * s2;
            x3 = -b / (4 * a) + 0.5 * s1 - 0.5 * s3;
            x4 = -b / (4 * a) + 0.5 * s1 + 0.5 * s3;

            //x1 = (-b) / 4 / a
                //- 0.5 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta)
                //- 0.5 * Math.Sqrt(b * b / 2 / a / a - 4 * c / 3 / a - delta - ((-1) * b * b * b / a / a / a + 4 * b * c / a / a - 8 * d / a) / (4 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta)));
            //x2 = (-b) / 4 / a - 0.5 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta) + 0.5 * Math.Sqrt(b * b / 2 / a / a - 4 * c / 3 / a - delta - ((-1) * b * b * b / a / a / a + 4 * b * c / a / a - 8 * d / a) / (4 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta)));
            //x3 = (-b) / 4 / a + 0.5 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta) - 0.5 * Math.Sqrt(b * b / 2 / a / a - 4 * c / 3 / a + delta - ((-1) * b * b * b / a / a / a + 4 * b * c / a / a - 8 * d / a) / (4 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta)));
            //x4 = (-b) / 4 / a + 0.5 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta) + 0.5 * Math.Sqrt(b * b / 2 / a / a - 4 * c / 3 / a + delta - ((-1) * b * b * b / a / a / a + 4 * b * c / a / a - 8 * d / a) / (4 * Math.Sqrt(b * b / 4 / a / a - 2 * c / 3 / a + delta)));

        }
        /*private void f4_2(double a, double b, double c, double d, double e, ref double x1, ref double x2, ref double x3, ref double x4)
        {
            double D = 3.0 * b - 8.0 * a * c;
            double E = -b + 4.0 * a * b * c - 8.0 * a * d;
            double F = 3.0 * b + 16.0 * a * c - 16.0 * a * b * c + 16.0 * a * b * d - 64.0 * a * e;
            double A = D - 3.0 * F;
            double B = D * F - 9.0 * E;
            double C = F - 3.0 * D * E;
            double delta = B * B - 4.0 * A * C;

            if (D == 0 && E == 0 && F == 0)
                x1 = x2 = x3 = x4 = (-1.0) * b / 4 / a;
            if (D * E * F != 0 && A == 0 && B == 0 && C == 0)
            {
                x1 = ((-1.0) * b * D + 9 * E) / 4.0 / a / D;
                x2 = x3 = x4 = ((-1.0) * b * D - 3 * E) / 4.0 / a / D;
            }
            if (E == 0 && F == 0 && D >= 0)
            {
                x1 = x2 = ((-1.0) * b + Math.Sqrt(D)) / 4.0 / a;
                x3 = x4 = ((-1.0) * b - Math.Sqrt(D)) / 4.0 / a;
            }
            if (E == 0 && F == 0 && D < 0)
            {
                x1 = x2 = ((-1.0) * b + Math.Sqrt(D)) / 4.0 / a;
                x3 = x4 = ((-1.0) * b - Math.Sqrt(D)) / 4.0 / a;
            }
            if (A * B * C != 0 && delta == 0)
            {

            }
        }*/

        private void statistic(int tlen)
        {
            double avgx = 0, avgx2 = 0;
            double sigma = 0;
            for(int k = 0; k < tlen; ++k)
            {
                avgx += k*result[k];
            }
            avgx /= ballnum;
            for (int k = 0; k < tlen; ++k)
            {
                avgx2 += k*k*result[k];
            }
            avgx2 /= ballnum;
            sigma = avgx2 - avgx * avgx;
            Graphics g = Graphics.FromImage(gimage);
            Brush br = Brushes.Black;
            Font f = new Font("黑体",15);
            g.DrawString("μ=" + avgx.ToString(".0000"), f, br, 0, 0);
            g.DrawString("σ²=" + sigma.ToString(".0000"), f, br, 0, 30);
            g.Flush();

        }
    }
    
};