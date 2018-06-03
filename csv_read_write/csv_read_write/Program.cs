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
            public double AGE { get; set; }
            public double Species { get; set; }
            public double ZOI { get; set; }

            public static Tree FromLine(string line)
            {
                var data = line.Split(',');

                return new Tree()
                {
                    X = double.Parse(data[0]),
                    Y = double.Parse(data[1]),
                    Species = double.Parse(data[2]),
                    dbh = double.Parse(data[3]),
                    AGE = double.Parse(data[4]),
                    a = double.Parse(data[5]),
                    b = double.Parse(data[6]),
                    H = double.Parse(data[7]),
                    R = Get_R(double.Parse(data[5]), double.Parse(data[6]), double.Parse(data[3])),
                    //ZOI = Get_ZOI(R),
                };
            }

            static double Get_R(double a,double b,double dbh)
            {
                return (a * (Math.Pow(dbh / 2, b)));
            }
            static double Get_ZOI(object obj)
            {
                var R = Convert.ToDouble(obj);
                return (3.1416 * Math.Pow(R, 2));
            } 
        }
        static void Main(string[] args)
        {
            args = new[] { "../../../DATA/input.csv" };
            var trees = ReadTrees(args[0]);

            var text = new StringBuilder();
            var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "X", "Y","species","dbh","AGE", "a", "b", "H","R");
            text.AppendLine(header);

            foreach (var tree in trees)
            {
                //Console.WriteLine(tree.X);
               // Console.WriteLine(tree.Y);
                var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", tree.X, tree.Y,tree.Species,tree.dbh,tree.AGE,tree.a,tree.b,tree.H,tree.R);
                text.AppendLine(newLine);
            }
            File.WriteAllText("../../../DATA/output.csv", text.ToString());
            Console.WriteLine(text);

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
        
       
    }
}
