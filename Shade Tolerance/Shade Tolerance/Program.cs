using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace Shade_Tolerance
{
    class Program
    {
        static readonly double a = 8.209;
        static readonly double b = 0.5352;
        static readonly double c = 1.30; // competition factor 
        static readonly double rbh;
        static readonly double d;
        static readonly double b2 = 26.9;
        static readonly double b3 = 0.0538;
        static readonly double G = 275;
        static readonly double H_max = 3500;
        static readonly double dbh_max = 250;
        

        struct Tree
        {
            public double X;
            public double Y;
            public double R;
            public double FA;
        };

        static void Main(string[] args)
        {
            double r = Growth_Rate(1, 1, 1183.337524, 42.51181448);

            Console.Write(r);
            Console.Read();
        }

        static double Shaded_Area(int x1, int y1, int x2, int y2)
        {
            double d = Math.Sqrt(Math.Pow(x1 - x2,2) + Math.Pow(y1-y2,2));
            
            double rA = 3;
            double rB = 4;

            double A = .5 * Math.Pow(rA, 2) * (Math.Asin(rB / d) - (rB / d));
            double B = .5 * Math.Pow(rB, 2) * (Math.Asin(rA / d) - (rA / d));

            return A + B;
        }

        static double Shade_Tolerance(double A, double c)
        {

            return (A * Math.Log10(c));
        }

        //finding FON value
        static double Tree_FON_PCR(double phi, double r0, double r1)
        {
            double result=0;  //do integration
            result = (-2 * phi / c) * (Math.Exp(-c * (r1 - rbh)) - Math.Exp(-c * (r0 - rbh)));

            return result;
        }

        static double FA_kn(Tree TreeK, Tree TreeN)
        {
            double result = 0;

            double A = Math.Sqrt(Math.Pow(TreeK.X - TreeN.X, 2) + Math.Pow(TreeK.Y - TreeN.Y, 2));

            if(A > TreeK.R + TreeN.R)
            {
                return result;
            }
            double r0 = A - TreeK.R;
            if(r0 < 0)
            {
                r0 = -r0;
                result = Tree_FON_PCR(3.1416, 0, r0);
                if(r0 >= TreeN.R)
                {
                    return result;
                }
            }

            double r1 = Math.Min(TreeN.R, A + TreeK.R);
            r0 = Math.Abs(r0);
            double nStrides = 2;
            double dr = (r1 - r0) / nStrides;
            double r = r0 + (dr / 2);

            for(int i=0;i<nStrides;++i)
            {
                double x = (Math.Pow(A, 2) + Math.Pow(r, 2) - Math.Pow(TreeK.R, 2)) / (2 * A);
                double y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x, 2));
                double phi = Math.Atan(y / x);

                result = result + Tree_FON_PCR(phi, (r * dr) / 2, r + (dr / 2));

                r = r + dr;
            }
            return result;
            
        }

        static void CalculateFA()
        {
            int N=100; //number of tree
            int n,k;
            Tree [] tree =new Tree[N];


            for(n=0;n<N;++n)
            {
                tree[n].FA = 0;
            }

            for(k=0;k<N;++k)
            {
                for(n=k+1;n<N;++n)
                {
                    tree[k].FA = tree[k].FA + FA_kn(tree[k], tree[n]);
                    tree[n].FA = tree[n].FA + FA_kn(tree[n], tree[k]);
                }
                tree[k].FA = tree[k].FA / (3.1416 * Math.Pow(tree[k].R, 2));
            }
        }

        static double Competition(double FA)
        {
            return 1 - (2 * FA);
        }

        static double Salt_Stress_Factor(double u, double ui)
        {
            return (1 / (1 + Math.Exp(d * (ui - u))));
        }

        static double Growth_Rate(double SU, double CFa, double H, double dbh)
        {
            return ((G * dbh *(1-((dbh* H) / (dbh_max * H_max))) / (274 + (3 * b2 * dbh) - (4 * b3 * Math.Pow(dbh, 2))))) * SU * CFa;
        }

        static double Height_H(double dbh)
        {
            return (137 + (b2 * dbh) - (b3 * Math.Pow(dbh, 2)));
        }

        static double Growth_Constant_G(double ddbh_max, double dbh_max, double H_max)
        {
            return (ddbh_max * H_max / (.2 * dbh_max));
        }

        static double Constant_b2(double H_max, double dbh_max)
        {
            return (2 * (H_max - 137) / dbh_max);
        }

        static double Constant_b3(double H_max, double dbh_max)
        {
            return ((H_max - 137) / Math.Pow(dbh_max, 2));
        }

        static double Constant_a(double r, double K)
        {
            return (Math.Sqrt(21500 / ((Math.Exp(r) * 3.1416 * .005) - K)));
        }

        static double Constant_b(double K)
        {
            return (-K / 2);
        }

    }

   
}
