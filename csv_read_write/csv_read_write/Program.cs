using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace csv_read_write
{
    class Program
    {
        public static double dbh_max = 250;
        public static double H_max = 3500;
        public static double ddbh_max = 3.92857;
        public static double d = 1; //constant for salt effect on growth
        public static double Ui = 1;    //constant salt effect on growth
        public static double U = 1;     //constant salinity at stem position


        class Tree
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double R { get; set; }
            public double H { get; set; }
            public double a { get; set; }
            public double b { get; set; }
            public double dbh { get; set; }
            public double rbh { get; set; }
            public double AGE { get; set; }
            public double Species { get; set; }
            public double ZOI { get; set; }
            public double FA { get; set; }
            public double Crown_Radius { get; set; }
            public double Shaded_Area{get; set;}
            public double CFa { get; set; }

            public static Tree FromLine(string line)
            {
                var data = line.Split(',');

                return new Tree()
                {
                    X = double.Parse(data[0]),
                    Y = double.Parse(data[1]),
                    Species = double.Parse(data[2]),
                    dbh = double.Parse(data[3]),
                    rbh = double.Parse(data[3]) / 2,
                    AGE = double.Parse(data[4]),
                    a = double.Parse(data[5]),
                    b = double.Parse(data[6]),
                    H = double.Parse(data[7]),
                    R = Get_R(double.Parse(data[5]), double.Parse(data[6]), double.Parse(data[3])),
                    //1.1331 +(15.233* double.Parse(data[3])),
                    Crown_Radius = (1.1331 + (15.233 * double.Parse(data[3]))) / 2,

                };
            }
           
            static double Get_R(double a,double b,double dbh)
            {
                
                return (a * (Math.Sqrt(dbh/2)));
            }
            
        }


        static void Main(string[] args)
        {
            //args = new[] { "../../../DATA/input.csv" };
            //var trees = ReadTrees(args[0]);
            //var zoi_trees = ZOI_Trees(trees);
            //var FA_trees = CalculateFA(trees);

            Growth();
           /* var text = new StringBuilder();
            var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", "X", "Y","species","dbh","rbh","AGE", "a", "b", "H","R","ZOI","FA","Crown Radius","Shaded Area");
            text.AppendLine(header);

            for (int i=0;i<trees.Count();i++)
            {
                var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", trees[i].X, trees[i].Y, trees[i].Species, trees[i].dbh,trees[i].rbh, trees[i].AGE, trees[i].a, trees[i].b, trees[i].H, trees[i].R, zoi_trees[i].ZOI,FA_trees[i].FA,trees[i].Crown_Radius,FA_trees[i].Shaded_Area);
                text.AppendLine(newLine);
            }
            
            File.WriteAllText("../../../DATA/output.csv", text.ToString());*/

           
            Console.WriteLine("DONE");
            Console.Read();

        }
        static IList<Tree> ReadTrees(string path, bool hasheaders=true)
        {
            var list = new List<Tree>();
            
            foreach(var line in File.ReadLines(path).Skip(hasheaders ? 1:0))
            {
                list.Add(Tree.FromLine(line));
            }
            return list;
        }

        static IList<Tree> ZOI_Trees(IList<Tree> trees)
        {
            var list = new List<Tree>();
            foreach(var n in trees)
            {
                Tree tree = new Tree()
                {
                    ZOI = (3.1416 * Math.Pow(n.a, 2)*n.rbh),
                };
                list.Add(tree);
            }
            return list;
        }

        static IList<Tree> CalculateFA(IList<Tree> trees)
        {
            var list = new List<Tree>();
            
            for (int k = 0; k < trees.Count(); k++)
            {
                
                //Console.WriteLine(trees[k].FA);
                for (int n = k + 1; n < trees.Count(); n++)
                {   //FA calculation
                    trees[k].FA = trees[k].FA + FA_kn(trees[k], trees[n]);
                    trees[n].FA = trees[n].FA + FA_kn(trees[n], trees[k]);

                    //shade tolerance 
                    trees[k].Shaded_Area += Shaded_Area(trees[k], trees[n]);
                    trees[n].Shaded_Area += Shaded_Area(trees[n], trees[k]);
                }
               
                trees[k].FA = trees[k].FA / (3.1416 * Math.Pow(trees[k].R, 2));
               // Console.WriteLine(trees[k].Shaded_Area);
                trees[k].Shaded_Area = trees[k].Shaded_Area / (3.1416 * Math.Pow(trees[k].Crown_Radius, 2));
                

                Tree tree = new Tree()
                {
                    FA = trees[k].FA,
                };
                list.Add(tree);
            }
            return list;    //FA of all trees
        }

        static double FA_kn(Tree TreeK, Tree TreeN)
        {
            double result = 0;

            double A = Math.Sqrt(Math.Pow(TreeK.X - TreeN.X, 2) + Math.Pow(TreeK.Y - TreeN.Y, 2));

            if (A > TreeK.R + TreeN.R)
            {
                
                return result;
            }
            double r0 = A - TreeK.R;
            if (r0 < 0)
            {
                r0 = -r0;
                result = Tree_FON_PCR(3.1416, 0, r0) * Calculate_FON(r0,TreeN);
                if (r0 >= TreeN.R)
                {
                    
                    return result;
                }
            }

            double r1 = Math.Min(TreeN.R, A + TreeK.R);
            r0 = Math.Abs(r0);
            double nStrides = 2;
            double dr = (r1 - r0) / nStrides;
            double r = r0 + (dr / 2);

            for (int i = 0; i < nStrides; i++)
            {
                double x = (Math.Pow(A, 2) + Math.Pow(r, 2) - Math.Pow(TreeK.R, 2)) / (2 * A);
                double y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x, 2));
               // x = Math.Abs(x);
                double phi = Math.Atan(y / x);
               // Console.WriteLine(phi);
                result = result + Tree_FON_PCR(phi, (r * dr) / 2, r + (dr / 2)) * Calculate_FON(r,TreeN);

                r = r + dr;
            }
            return result;

        }

        static double Tree_FON_PCR(double phi, double r0, double r1)
        {
            double result = 0;  //do integration
           
            result = 2 * phi * (r1 - r0);

            return result;
        }

        static double Calculate_FON(double r, Tree TreeN)
        {
            if (r <= TreeN.rbh)
                return 1;
            else
            {
                double c = Math.Abs(Math.Log(.1)) / (TreeN.R - TreeN.rbh);
                double result = Math.Exp(-c* (r - TreeN.rbh));
                return result;
            }

        }
        // shade tolerance calculation
        static double Shaded_Area(Tree TreeK, Tree TreeN)
        {
            double result = 0;

            double A = Math.Sqrt(Math.Pow(TreeK.X - TreeN.X, 2) + Math.Pow(TreeK.Y - TreeN.Y, 2));

            if (A > TreeK.Crown_Radius + TreeN.Crown_Radius || TreeK.H > TreeN.H)
            {
               
                return result;
            }
            double r0 = A - TreeK.Crown_Radius;
            if (r0 < 0)
            {
                r0 = -r0;
                result = Tree_FON_PCR(3.1416, 0, r0) * Calculate_Shade_Area(r0, TreeN);
                //Console.WriteLine(Calculate_Shade_Area(r0, TreeN));
                if (r0 >= TreeN.Crown_Radius)
                {
                   
                    return result;
                }
            }

            double r1 = Math.Min(TreeN.Crown_Radius, A + TreeK.Crown_Radius);
            r0 = Math.Abs(r0);
            double nStrides = 2;
            double dr = (r1 - r0) / nStrides;
            double r = r0 + (dr / 2);

            for (int i = 0; i < nStrides; i++)
            {
                double x = (Math.Pow(A, 2) + Math.Pow(r, 2) - Math.Pow(TreeK.Crown_Radius, 2)) / (2 * A);
                double y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x, 2));
                x = Math.Abs(x);
                double phi = Math.Atan(y / x);
                //Console.WriteLine(phi);
                result = result + Tree_FON_PCR(phi, (r * dr) / 2, r + (dr / 2)) * Calculate_Shade_Area(r, TreeN);
                //Console.WriteLine(Tree_FON_PCR(phi, (r * dr) / 2, r + (dr / 2)));
                //result = Math.Abs(result);
                r = r + dr;
            }
           // Console.WriteLine(result);
            return result;
        }

        static double Calculate_Shade_Area(double r, Tree TreeN)
        {
            if (r <= TreeN.rbh)
                return 1;
            else
            {
                double c = Math.Abs(Math.Log(.1)) / (TreeN.Crown_Radius - TreeN.rbh);
                double result = Math.Exp(-c * (r - TreeN.rbh));
                return result;
            }

        }

        //calculation of tree height
        static double Height_H(double dbh)
        {
            double b2 = Constant_b2();
            double b3 = Constant_b3();

            return (137 + (b2 * dbh) - (b3 * Math.Pow(dbh, 2)));
        }

        //getting Growth Constant
        static double Growth_Constant_G()
        {
            return (ddbh_max * H_max / (.2 * dbh_max));
        }

        // getting constant b2
        static double Constant_b2()
        {
            return (2 * (H_max - 137) / dbh_max);
        }

        //getting constant b3
        static double Constant_b3()
        {
            return ((H_max - 137) / Math.Pow(dbh_max, 2));
        }

        //calculating cmpetition
        static IList<Tree> Competition(IList <Tree>TreeFA)
        {
            var list = new List<Tree>();
            foreach (var trees in TreeFA)
            {
                double result = (1 - (1.3 * (trees.FA)));

                Tree tree = new Tree()
                {
                    CFa = result,
                };
                list.Add(tree);
            }
            return list;
        }

        //calculating Growth per year
        static double Growth_Rate(Tree tree)
        {
            double G = Growth_Constant_G();
            double b2 = Constant_b2();
            double b3 = Constant_b3();
            double dbh = tree.dbh;
            double H = tree.H;
            double SU = 1;
            double CFa = tree.CFa;

            double result = ((G * dbh * (1 - ((dbh * H) / (dbh_max * H_max))) / (274 + (3 * b2 * dbh) - (4 * b3 * Math.Pow(dbh, 2))))) * SU * CFa;
            //Console.WriteLine(result);

            return result;
        }
       
        //calculating Salt Stress Factor
        static double Salt_Stress_factor()
        {
            return (1 / (1 + Math.Exp(d * (Ui - U))));
        }

        //calculating Growth of this year
        static void Growth()
        {
            var args = new[] { "../../../DATA/input.csv" };
            var trees = ReadTrees(args[0]);
            var zoi_trees = ZOI_Trees(trees);
            var FA_trees = CalculateFA(trees);
            var CFa_trees = Competition(FA_trees);

            for(int i=0; i < trees.Count(); i++)
            {
                
                double result = trees[i].dbh;
                
                Tree tree = new Tree()
                {
                    H = trees[i].H,
                    CFa = CFa_trees[i].CFa,
                    dbh = trees[i].dbh,
                };
                result += Growth_Rate(tree);
                Console.WriteLine(result);
            }

            
        }
    }
}
