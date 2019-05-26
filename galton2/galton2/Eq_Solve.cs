using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace galton2
{
    unsafe class Eq_Solve
    {
        public double[] roots_quadratic_equation(double a, double b, double c)
        {
            //the first element is the number of the real roots, and other elements are the real roots.
            double[] roots = new double[3];
            if (a == 0.0)
            {
                if (b == 0.0)
                {
                    roots[0] = 0.0;
                }
                else
                {
                    roots[1] = -c / b;
                    roots[0] = 1.0;
                }
            }
            else
            {
                double d = b * b - 4 * a * c;
                if (d < 0.0)
                {
                    roots[0] = 0.0;
                }
                else
                {
                    roots[1] = (-b + Math.Sqrt(d)) / (2 * a);
                    roots[2] = (-b - Math.Sqrt(d)) / (2 * a);
                    roots[0] = 2.0;
                }
            }
            return roots;
        }
        public double[] roots_cubic_equation(double a, double b, double c, double d)
        {
            //the first element is the number of the real roots, and other elements are the real roots.
            //Shengjin's formula
            double[] roots = new double[4];
            if (a == 0)
            {
                roots = roots_quadratic_equation(b, c, d);
            }
            else
            {
                double A = b * b - 3 * a * c;
                double B = b * c - 9 * a * d;
                double C = c * c - 3 * b * d;
                double deita = B * B - 4 * A * C;

                if ((A == B) && (A == 0))
                {
                    //the three roots are the same
                    if (a != 0)
                    {
                        roots[1] = -b / (3 * a);
                    }
                    else
                    {
                        if (b != 0)
                        {
                            roots[1] = -c / b;
                        }
                        else
                        {
                            if (c != 0)
                            {
                                roots[1] = -3 * d / c;
                            }
                        }
                    }
                    roots[2] = roots[1];
                    roots[3] = roots[1];
                    roots[0] = 3;
                }
                else if (deita > 0)
                {
                    //only one real root
                    double y1 = A * b + (3 * a / 2) * (-B + Math.Sqrt(deita));
                    double y2 = A * b + (3 * a / 2) * (-B - Math.Sqrt(deita));
                    double pow_y1, pow_y2;
                    if (y1 < 0)
                    {
                        //for pow(a,b), when b is not int, a should not be negative.
                        pow_y1 = -Math.Pow(-y1, 1.0 / 3.0);
                    }
                    else
                    {
                        pow_y1 = Math.Pow(y1, 1.0 / 3.0);
                    }
                    if (y2 < 0)
                    {
                        pow_y2 = -Math.Pow(-y2, 1.0 / 3.0);
                    }
                    else
                    {
                        pow_y2 = Math.Pow(y2, 1.0 / 3.0);
                    }
                    roots[1] = (-b - pow_y1 - pow_y2) / (3 * a);
                    roots[0] = 1;
                }
                else if (deita == 0)
                {
                    //three real roots and two of them are the same
                    double K = B / A;
                    roots[1] = -b / a + K;
                    roots[2] = -K / 2;
                    roots[3] = -K / 2;
                    roots[0] = 3;
                }
                else if (deita < 0)
                {
                    
                    //three different real roots
                    double theta = Math.Acos((2 * A * b - 3 * a * B) / (2 * Math.Pow(A, 1.5)));
                    roots[1] = (-b - 2 * Math.Sqrt(A) * Math.Cos(theta / 3)) / (3 * a);
                    roots[2] = (-b + Math.Sqrt(A) * (Math.Cos(theta / 3) + Math.Sqrt(3) * Math.Sin(theta / 3))) / (3 * a);
                    roots[3] = (-b + Math.Sqrt(A) * (Math.Cos(theta / 3) - Math.Sqrt(3) * Math.Sin(theta / 3))) / (3 * a);
                    roots[0] = 3;
                }
            }
            return roots;
        }
        public double[] roots_quartic_equation(double a, double b, double c, double d, double e)
        {
            //the first element is the number of the real roots, and other elements are the real roots.
            //Ferrari's solution.
            double[] roots = new double[5];
            if (a == 0)
            {
                roots = roots_cubic_equation(b, c, d, e);
            }
            else
            {
                double b1 = b / a;
                double c1 = c / a;
                double d1 = d / a;
                double e1 = e / a;
                if ((b1 == 0) && (c1 == 0) && (d1 == 0))
                {
                    //in this special case, such as a=1, b=c=d=0, e=-1, the roots should be +1 and -1
                    if (e1 > 0)
                    {
                        roots[0] = 0.0;
                    }
                    else
                    {
                        roots[1] = Math.Sqrt(Math.Sqrt(-e1));
                        roots[2] = -Math.Sqrt(Math.Sqrt(-e1));
                        roots[0] = 2.0;
                    }
                }
                else
                {
                    double[] roots_y = new double[4];
                    roots_y = roots_cubic_equation(-1.0, c1, 4 * e1 - b1 * d1, d1 * d1 + e1 * b1 * b1 - 4 * e1 * c1);
                    double y = roots_y[1];
                    double B1, B2, C1, C2;
                    if (b1 * b1 - 4 * c1 + 4 * y == 0)
                    {
                        B1 = b / 2;
                        B2 = b / 2;
                        C1 = y / 2;
                        C2 = y / 2;
                    }
                    else
                    {
                        B1 = b / 2 - Math.Sqrt(b1 * b1 - 4 * c1 + 4 * y) / 2;
                        B2 = b / 2 + Math.Sqrt(b1 * b1 - 4 * c1 + 4 * y) / 2;
                        C1 = y / 2 - (b1 * y - 2 * d1) / (2 * Math.Sqrt(b1 * b1 - 4 * c1 + 4 * y));
                        C2 = y / 2 + (b1 * y - 2 * d1) / (2 * Math.Sqrt(b1 * b1 - 4 * c1 + 4 * y));
                    }
                    double[] roots_x1 = new double[3];
                    double[] roots_x2 = new double[3];
                    roots_x1 = roots_quadratic_equation(1.0, B1, C1);
                    roots_x2 = roots_quadratic_equation(1.0, B2, C2);
                    if (roots_x1[0] != 0)
                    {
                        for (int i = 1; i < roots_x1[0] + 1; i++)
                        {
                            roots[i] = roots_x1[i];
                        }
                    }
                    if (roots_x2[0] != 0)
                    {
                        int roots_x1_number = (int)(roots_x1[0]);
                        for (int j = 1; j < roots_x2[0] + 1; j++)
                        {
                            roots[roots_x1_number + j] = roots_x2[j];
                        }
                    }
                    roots[0] = roots_x1[0] + roots_x2[0];
                }
            }
            return roots;
        }
    }
}
