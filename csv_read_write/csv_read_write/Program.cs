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

            public static Tree FromLine(string line)
            {
                var data = line.Split(',');

                return new Tree()
                {
                    X = double.Parse(data[0]),
                    Y = double.Parse(data[1]),
                };
            }
        }
        static void Main(string[] args)
        {
            args = new[] { "../../../DATA/input.csv" };
            var trees = ReadTrees(args[0]);

            var text = new StringBuilder();
            var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", "X", "Y","species","dbh","a","b","AGE","H");
            text.AppendLine(header);

            foreach (var tree in trees)
            {
                //Console.WriteLine(tree.X);
               // Console.WriteLine(tree.Y);
                var newLine = string.Format("{0},{1}", tree.X, tree.Y);
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
