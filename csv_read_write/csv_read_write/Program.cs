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
                                      
                };
            }
           
            static double Get_R(double a,double b,double dbh)
            {
                
                return (a * (Math.Pow(dbh / 2, b)));
            }
            
        }


        static void Main(string[] args)
        {
            args = new[] { "../../../DATA/input.csv" };
            var trees = ReadTrees(args[0]);
            var zoi_trees = ZOI_Trees(trees);

            CalculateFA(trees);


            var text = new StringBuilder();
            var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "X", "Y","species","dbh","rbh","AGE", "a", "b", "H","R","ZOI");
            text.AppendLine(header);

            for (int i=0;i<trees.Count();i++)
            {
                //Console.WriteLine(tree.X);
               // Console.WriteLine(tree.Y);
                var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", trees[i].X, trees[i].Y, trees[i].Species, trees[i].dbh,trees[i].rbh, trees[i].AGE, trees[i].a, trees[i].b, trees[i].H, trees[i].R, zoi_trees[i].ZOI);
                text.AppendLine(newLine);
            }
            foreach(var d_tree in zoi_trees)
            {
                //Console.WriteLine(d_tree.ZOI);
            }
            File.WriteAllText("../../../DATA/output.csv", text.ToString());
            //Console.WriteLine(text);

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
                    ZOI = (3.1416 * Math.Pow(n.R, 2)),
                };
                list.Add(tree);
            }
            return list;
        }

        static IList<Tree> CalculateFA(IList<Tree> trees)
        {
            var list = new List<Tree>();

            for (int k = 0; k < trees.Count(); ++k)
            {
                //Console.WriteLine(trees[k].FA);
                for (int n = k + 1; n < trees.Count(); ++n)
                {
                    trees[k].FA = trees[k].FA + FA_kn(trees[k], trees[n]);
                    trees[n].FA = trees[n].FA + FA_kn(trees[n], trees[k]);
                }
                
                trees[k].FA = trees[k].FA / (3.1416 * Math.Pow(trees[k].R, 2));
                Console.WriteLine(trees[k].FA);
                Tree tree = new Tree()
                {
                    FA = trees[k].FA,
                };
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
                result = Tree_FON_PCR(3.1416, 0, r0) *.5;
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

            for (int i = 1; i <= nStrides; ++i)
            {
                double x = (Math.Pow(A, 2) + Math.Pow(r, 2) - Math.Pow(TreeK.R, 2)) / (2 * A);
                double y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x, 2));
                double phi = Math.Atan(y / x);

                result = result + Tree_FON_PCR(phi, (r * dr) / 2, r + (dr / 2)) *.5;

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

        // shade tolerance calculation

    }
}
