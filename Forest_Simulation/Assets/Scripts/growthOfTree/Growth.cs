using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;

public class Growth : MonoBehaviour {


    public static double ddbh_max = 3.92857;
    public static double d = 1;     //constant for salt effect on growth
    public static double Ui = 1;    //constant salt effect on growth
    public static double U = 1;     //constant salinity at stem position
    public static int start = 0;
    
    
    //Properties of Tree
    class Tree
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double MaxHeight { get; set; }
        public double MaxDbh { get; set; }
        public double CritDx { get; set; }
        public double Phi { get; set; }
        public double MinFon { get; set; }
        public double SaltEffect { get; set; }
        public double Salinity { get; set; }
        public double ConstSaltEffect { get; set; }
        public double MaxAge { get; set; }
        public double ShadeToleranceRanking { get; set; }
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
        public double Shaded_Area { get; set; }
        public double CFa { get; set; }
        public double b2 { set; get; }
        public double b3 { set; get; }
        public int index { set; get; }
        public double ShadeToleranceValue { set; get; }

        //public static Tree FromLine(string line)
        //{
        //    var data = line.Split(',');

        //    return new Tree()
        //    {
        //        X = double.Parse(data[0]),
        //        Y = double.Parse(data[1]),
        //        Species = double.Parse(data[2]),
        //        dbh = double.Parse(data[3]),
        //        rbh = double.Parse(data[3]) / 2,
        //        AGE = double.Parse(data[4]),
        //        a = double.Parse(data[5]),
        //        b = double.Parse(data[6]),
        //        H = double.Parse(data[7]),
        //        R = Get_R(double.Parse(data[5]), double.Parse(data[6]), double.Parse(data[3])),
        //        //1.1331 +(15.233* double.Parse(data[3])),
        //        Crown_Radius = (1.1331 + (15.233 * double.Parse(data[3]))) / 2,

        //    };
        //}
        static double Get_R(double a, double b, double dbh)
        {

            return (a * (Math.Sqrt(dbh / 2)));
        }

    }
    //Initialization the initial values of each tree from the given input
    static IList<Tree> ReadTrees()
    {
        //float[,] prop = new float[plotRandom.total+1,15];

        float[,] prop = new float[plotRandom.n, 16];
        //prop = plotRandom.prop;
        //Debug.Log("Here it is: "+ plotRandom.n);
        for (int i=0;i<plotRandom.n;i++)
        {
            
            for(int j=0;j<16;j++)
            {
                prop[i, j] = plotRandom.prop[i,j];
                //Debug.Log("Here it is: " + prop[i,j]);
            }
        }
        
        var list = new List<Tree>();


        for (int i = 1; i < prop.GetLength(0); i++)
        {
            double ranDbh = 2.5 + UnityEngine.Random.Range(0.0f,0.09f);
            double b2 = (2 * (prop[i, 3] - 137) / prop[i, 4]);
            double b3 = (prop[i, 3] - 137) / Math.Pow(prop[i, 4], 2);
            Tree tree = new Tree()
            {
                index = (int)prop[i, 0],
                X = prop[i, 1],
                Y = prop[i, 2],
                MaxHeight = prop[i, 3],
                MaxDbh = prop[i, 4],
                a = prop[i, 5],
                b = prop[i, 6],
                CritDx = prop[i, 7],
                Phi = prop[i, 8],
                MinFon = prop[i, 9],
                SaltEffect = prop[i, 10],
                Salinity = prop[i, 11],
                ConstSaltEffect = prop[i, 12],
                MaxAge = prop[i, 13],
                ShadeToleranceRanking = prop[i, 14],
                AGE = 0,
                dbh = ranDbh,
                H = (137 + (b2 * ranDbh) - (b3 * Math.Pow(ranDbh, 2))),
                R = prop[i, 5] * Math.Pow((ranDbh / 2), prop[i, 6]),
               // isDead = 0,
                Species = prop[i, 15],
                FA = 0,
                b2 = b2,
                b3 = b3,

            };
            list.Add(tree);
            //Debug.Log("Here it is:DBH: " + tree.dbh+ "Height:=" +tree.H);


        }

        return list;
    }
    //Calculation of the zone of influence of each tree
    static IList<Tree> ZOI_Trees(IList<Tree> trees)
    {
        var list = new List<Tree>();
        foreach (var n in trees)
        {
            
            
            
                Tree tree = new Tree()
                {
                    ZOI = n.ZOI,
                };
                list.Add(tree);
            
            
            
        }
        return list;
    }
    //Calculation of the affected area of a tree by the other trees 
    static IList<Tree> CalculateFA(IList<Tree> trees)
    {
        var list = new List<Tree>();

        for (int k = 0; k < trees.Count(); k++)
        {
            
            foreach(var n in trees)
            {
                n.FA = 0;
            }
            for (int n = k + 1; n < trees.Count(); n++)
            {
                //if (trees[n].isDead == 1)
                //{
                //    continue;
                //}
                trees[k].FA = trees[k].FA + (FA_kn(trees[k], trees[n]));  //Math.Abs              
                trees[n].FA = trees[n].FA + (FA_kn(trees[n], trees[k]));

                // for shade tolerance
                if (trees[k].H < trees[n].H)
                {
                    trees[k].Shaded_Area += FA_kn(trees[k], trees[n]);
                }
                else if (trees[n].H < trees[k].H)
                {
                    trees[n].Shaded_Area += FA_kn(trees[n], trees[k]);
                }
            }
          //  Debug.Log(trees[k].FA+ " " + (3.1416 * Math.Pow(trees[k].R, 2)));
            trees[k].FA = trees[k].FA / (3.1416 * Math.Pow(trees[k].R, 2));
            trees[k].Shaded_Area = trees[k].Shaded_Area / (3.1416 * Math.Pow(trees[k].R, 2));

            /*if (double.IsNaN(trees[k].FA) == true)
            {
                trees[k].FA = 0.05;
            }*/

            Tree tree = new Tree()
            {
                Phi=trees[k].Phi,
                FA = trees[k].FA,
                Shaded_Area = trees[k].Shaded_Area,
                ShadeToleranceRanking = trees[k].ShadeToleranceRanking,
            };
            list.Add(tree);

           // Debug.Log("FA: " + tree.FA);

        }
        return list;    //FA of all trees
    }
    //Calculation of affected area for a tree by another individual tree
    static double FA_kn(Tree TreeK, Tree TreeN)
    {
        double result = 0;

        double A = Math.Sqrt(Math.Pow(TreeK.X - TreeN.X, 2) + Math.Pow(TreeK.Y - TreeN.Y, 2));

       // Debug.Log("A"+A);

        if (A > TreeK.R + TreeN.R)
        {
          //  Debug.Log("result 0");
            return result;
        }
        double r0 = A - TreeK.R;
        if (r0 < 0)
        {
            r0 = -r0;
            result =( Tree_FON_PCR(Math.PI, 0, r0, TreeN));
          //  Debug.Log("when r0"+r0+"<0,res:"+result);
            if (r0 >= TreeN.R)
            {
              //  Debug.Log("when r0>"+r0+"treeN.R"+TreeN.R+",res:" + result);
                return result;
            }
        }

        double r1 = Math.Min(TreeN.R, A + TreeK.R);
        double nStrides = 2;
        double dr = (r1 - r0) / nStrides;
        double r = r0 + (dr / 2);

        for (int i = 0; i < nStrides; i++)
        {
            double x = (Math.Pow(A, 2) + Math.Pow(r, 2) - Math.Pow(TreeK.R, 2)) / (2 * A);
            double y = Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x, 2));
            double phi = Math.Atan(y / x);
            result = result + ( Tree_FON_PCR(phi, (r * dr) / 2, r + (dr / 2) , TreeN));
            r = r + dr;
        }
      //  Debug.Log("Rest of the cases result: "+result);
        return result;

    }
    //Calculation of the affected area by using pitch cycle ring
    static double Tree_FON_PCR(double phi, double r0, double r1,Tree TreeN)
    {
        double result = 0;  //do integration
        double c = Math.Abs(Math.Log(0.1)) / (TreeN.R - TreeN.rbh);
        //result = 2 *c* phi * (r1 - r0);
        result = 2*(phi/c)*(Math.Exp(TreeN.rbh-r0)-Math.Exp(TreeN.rbh-r1));

        return (result);//Math.Abs
    }
    //Calculation of the compitition factor for a tree
    static IList<Tree> Competition(IList<Tree> TreeFA)
    {
        var list = new List<Tree>();
        foreach (var trees in TreeFA)
        {
            double result = (1 - (trees.Phi * (trees.FA)));

            if (result < 0) 
            {
                result = 0;
            }

            Tree tree = new Tree()
            {
                CFa = result,
            };
            list.Add(tree);
            
        }
        return list;
    }
    //Shade tolerance calculation
    static IList<Tree> ShadeTolerance(IList<Tree> TreeST)
    {
        var list = new List<Tree>();
        foreach(var n in TreeST)
        {
            double result = Math.Log10(n.ShadeToleranceRanking) * (1 - n.Shaded_Area);

            if (result < 0)
            {
                result = 0;
            }
            //result = 1;//edit
            Tree tree = new Tree()
            {
                ShadeToleranceValue = result,
            };
            list.Add(tree);
           // Debug.Log(result);

        }

        return list;

    }
    //Calculation of the growth rate per year
    static double Growth_Rate(Tree tree)
    {
        double G = (ddbh_max * tree.MaxHeight / (.2 * tree.MaxDbh));
        double b2 = (2 * (tree.MaxHeight - 137) / tree.MaxDbh);
        double b3 = (tree.MaxHeight - 137) / Math.Pow(tree.MaxDbh, 2);
        double dbh = tree.dbh;
        double H = tree.H;
        double SU = 1;
        double CFa = tree.CFa;
        double ST = tree.ShadeToleranceValue;


        // Debug.Log(" CFa: "+CFa);//"Result in GrowthRAte: G " + G+"b2: "+b2+" b3"+b3+" dbh:"+dbh+" H:"+H+
        double result = ((G * dbh * (1 - ((dbh * H) / (tree.MaxDbh * tree.MaxHeight))) / (274 + (3 * b2 * dbh) - (4 * b3 * Math.Pow(dbh, 2))))) * SU * CFa * ST;
        //Console.WriteLine(result);
        
        return result;
        
    }
    //Salt stress factor calculation
    static double Salt_Stress_factor()
    {
        return (1 / (1 + Math.Exp(d * (Ui - U))));
    }
    //the whole properties are updated here by calling the executions of the above functions and writing the updates into some files
    public void TGrowth()
    {
        //totalInFile = new int[int.Parse(inputSceneStatus.year)];
        // var args = new[] { "../../../DATA/input.csv" };
        var trees = ReadTrees();


        int Index = plotRandom.n;
        //int species=0;
        //int deletion = 0;

        int[] deleteCheck = new int[trees.Count()];

        for (int x = 0; x < deleteCheck.Length; x++)
            deleteCheck[x] = 0;
        double[,] dbhAvg = new double[trees.Count(), 5];
        

        //Debug.Log(int.Parse(inputSceneStatus.year));
        for (int age=1;age < int.Parse(inputSceneStatus.year); age++)
        {
            
            var zoi_trees = ZOI_Trees(trees);
            var FA_trees = CalculateFA(trees);
            var CFa_trees = Competition(FA_trees);
            var shadeToleranceVal = ShadeTolerance(FA_trees);
            int New = 20, count=0;

          //  Debug.Log(CFa_trees.Count);

            List<Tree> treeToWrite = new List<Tree>();
            List<Tree> treeToWriteNewGen = new List<Tree>();



            for (int i = 0; i < trees.Count(); i++)
            {
                //if ( trees[i].isDead == 1)
                //{
                //    Tree tree_to_write2 = new Tree()
                //    {
                //        X = trees[i].X,
                //        Y = trees[i].Y,
                //        Species = species,
                //        dbh = trees[i].dbh,
                //        AGE = trees[i].AGE,
                //        a = trees[i].a,
                //        b = trees[i].b,
                //        H = trees[i].H,
                //        MaxHeight = trees[i].MaxHeight,
                //        MaxDbh = trees[i].MaxDbh,
                //        CritDx = trees[i].CritDx,
                //        Phi = trees[i].Phi,
                //        MinFon = trees[i].MinFon,
                //        SaltEffect = trees[i].SaltEffect,
                //        Salinity = trees[i].Salinity,
                //        ConstSaltEffect = trees[i].ConstSaltEffect,
                //        MaxAge = trees[i].MaxAge,
                //        ShadeToleranceRanking = trees[i].ShadeToleranceRanking,
                //        R = trees[i].R,
                //       // isDead = 1,
                //        b2 = trees[i].b2,
                //        b3 = trees[i].b3,
                //    };
                //    //treeToWrite.Add(tree_to_write2);

                //    continue;
                //}

                double result = trees[i].dbh;
                // Debug.Log("CFA: " + trees[i].CFa);
                Tree tree = new Tree()
                {
                    H = trees[i].H,
                    CFa = CFa_trees[i].CFa,
                    //CFa=0.5,
                    dbh = trees[i].dbh,
                    MaxDbh = trees[i].MaxDbh,
                    MaxHeight = trees[i].MaxHeight,
                    ShadeToleranceValue = shadeToleranceVal[i].ShadeToleranceValue,
                };
                
                double GR = Growth_Rate(tree);
                //Debug.Log("GR: " + GR);
                result += GR;
                           

                
                if (GR > trees[i].CritDx) 
                {
                    Debug.Log(trees[i].CritDx);
                    Tree tree_to_write = new Tree()
                    {
                        index=trees[i].index,
                        X = trees[i].X,
                        Y = trees[i].Y,
                        Species = trees[i].Species,
                        dbh = result,
                        AGE = trees[i].AGE + 1,
                        a = trees[i].a,
                        b = trees[i].b,
                        H= (137 + (trees[i].b2 * result) - (trees[i].b3 * Math.Pow(result, 2))),                    
                        MaxHeight = trees[i].MaxHeight,
                        MaxDbh = trees[i].MaxDbh,                    
                        CritDx = trees[i].CritDx,
                        Phi = trees[i].Phi,
                        MinFon = trees[i].MinFon,
                        SaltEffect = trees[i].SaltEffect,
                        Salinity = trees[i].Salinity,
                        ConstSaltEffect = trees[i].ConstSaltEffect,
                        MaxAge = trees[i].MaxAge,
                        ShadeToleranceRanking = trees[i].ShadeToleranceRanking, 
                        R = trees[i].a * Math.Pow((result / 2), trees[i].b),
                       // isDead=deletion,
                        b2 = trees[i].b2,
                        b3 = trees[i].b3,
                    };
                treeToWrite.Add(tree_to_write);
                }

                int ranNum = UnityEngine.Random.Range(1,20);
                if (age > 1 && ranNum < 2 && count < New) 
                {
                   // Debug.Log("Index:"+Index+" plotRandom.n="+plotRandom.n);
                    float randomDBH =(float) 2.5 + UnityEngine.Random.Range(0.0f, 0.09f);
                    double rad = 15.233 * (trees[i].dbh / 2) + 1.1331;
                    Tree tree_to_write_new_gen = new Tree()
                    {
                        index = Index,
                        //X = (UnityEngine.Random.Range((float)(trees[i].X - rad), (float)(trees[i].X + rad))),
                        //Y = (UnityEngine.Random.Range((float)(trees[i].Y - rad), (float)(trees[i].Y + rad))),
                        X = UnityEngine.Random.Range((float)(plotRandom.groundPosX - (plotRandom.groundWidth / 2)), (float)(plotRandom.groundPosX + (plotRandom.groundWidth / 2)))*100,
                        Y = UnityEngine.Random.Range((float)(plotRandom.groundPosZ - (plotRandom.groundLength / 2)), (float)(plotRandom.groundPosZ + (plotRandom.groundLength / 2)))*100,
                        Species = trees[i].Species,
                        dbh = randomDBH,
                        AGE = 0,
                        a = trees[i].a,
                        b = trees[i].b,
                        H = (137 + (trees[i].b2 * randomDBH) - (trees[i].b3 * Math.Pow(randomDBH, 2))),
                        MaxHeight = trees[i].MaxHeight,
                        MaxDbh = trees[i].MaxDbh,
                        CritDx = trees[i].CritDx,
                        Phi = trees[i].Phi,
                        MinFon = trees[i].MinFon,
                        SaltEffect = trees[i].SaltEffect,
                        Salinity = trees[i].Salinity,
                        ConstSaltEffect = trees[i].ConstSaltEffect,
                        MaxAge = trees[i].MaxAge,
                        ShadeToleranceRanking = trees[i].ShadeToleranceRanking,
                        R = trees[i].a * Math.Pow((randomDBH / 2), trees[i].b),
                        // isDead = 0,
                        b2 = trees[i].b2,
                        b3 = trees[i].b3,
                    };
                    treeToWriteNewGen.Add(tree_to_write_new_gen);

                    // Debug.Log("Its ok");
                    Index++;
                    count++;
                }

              //  Debug.Log( "  dbh : " + tree_to_write.dbh + " Height: " + tree_to_write.H);
            }
           // Debug.Log(treeToWrite.Count());
            treeToWrite.AddRange(treeToWriteNewGen);
            //totalInFile[age - 1] = treeToWrite.Count;

           
            trees = treeToWrite;
            
            string CreateFolder = "OUTPUT";
                        
            bool exists = System.IO.Directory.Exists(CreateFolder);

            if (!exists)
                System.IO.Directory.CreateDirectory(CreateFolder);

            string fileName = "OUTPUT/Simulation_year_" + (age + 1).ToString() + ".csv";

            var data = new StringBuilder();
            var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}","Index", "X", "Y", "Species", "DBH", "Age", "a", "b", "Height");
            data.AppendLine(header);
            foreach(var tree in trees)
            {
                var newLine= string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",tree.index,tree.X/100,tree.Y/100,tree.Species,tree.dbh,tree.AGE+1,tree.a,tree.b,tree.H);
                data.AppendLine(newLine);
            }
            File.WriteAllText(fileName,data.ToString());

            if (age == int.Parse(inputSceneStatus.year)-1)
                start = 1;
           
        }
        


    }
}
