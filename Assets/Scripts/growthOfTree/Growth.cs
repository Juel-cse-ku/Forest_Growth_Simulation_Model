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
    public static float[,] pos_trees = new float[plotRandom.total, 2];
    
    
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
        public double PercentageOfRegeneration { set; get; }

        static double Get_R(double a, double b, double dbh)
        {
            return (a * (Math.Sqrt(dbh / 2)));
        }

    }

    //Initialization the initial values of each tree from the given input
    static IList<Tree> ReadTrees()
    {
        float[,] prop = new float[plotRandom.n, 17];

        for (int i = 0; i < plotRandom.n; i++) 
        {

            for (int j = 0; j < 17; j++) 
            {
                prop[i, j] = plotRandom.prop[i,j];
            }
        }
        
        var list = new List<Tree>();

        for (int i = 0; i < prop.GetLength(0); i++)
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
                Species = prop[i, 15],
                PercentageOfRegeneration = prop[i, 16],
                FA = 0,
                b2 = b2,
                b3 = b3,

            };
            list.Add(tree);
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

            Tree tree = new Tree()
            {
                Phi=trees[k].Phi,
                FA = trees[k].FA,
                Shaded_Area = trees[k].Shaded_Area,
                ShadeToleranceRanking = trees[k].ShadeToleranceRanking,
            };
            list.Add(tree);
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
            double shade_tolerant = 1 - (1 / n.ShadeToleranceRanking);

            //if (n.ShadeToleranceRanking == 2)
            //    shade_tolerant = 1 - (1 / .5);
            //else if (n.ShadeToleranceRanking == 3)
            //    shade_tolerant = 1 - (1 / .667);
            //else if (n.ShadeToleranceRanking == 4)
            //    shade_tolerant = 1 - (1 / .750);
            //else if (n.ShadeToleranceRanking == 5)
            //    shade_tolerant = 1 - (1 / .8);
            //else if (n.ShadeToleranceRanking == 6)
            //    shade_tolerant = 1 - (1 / .833);
            //else if (n.ShadeToleranceRanking == 7)
            //    shade_tolerant = 1 - (1 / .857);
            //else if (n.ShadeToleranceRanking == 8)
            //    shade_tolerant = 1 - (1 / .875);
            //else if (n.ShadeToleranceRanking == 9)
            //    shade_tolerant = 1 - (1 / .889);
            //else if (n.ShadeToleranceRanking == 10)
            //    shade_tolerant = 1 - (1 / .9);

            if (shade_tolerant < 0)
            {
                shade_tolerant = 0;
            }

            Tree tree = new Tree()
            {
                ShadeToleranceValue = shade_tolerant,
            };
            list.Add(tree);
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
        
        double result = ((G * dbh * (1 - ((dbh * H) / (tree.MaxDbh * tree.MaxHeight))) / (274 + (3 * b2 * dbh) - (4 * b3 * Math.Pow(dbh, 2))))) * SU * CFa * ST;
        
        
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
        var trees = ReadTrees();
        int Index = plotRandom.n;
        
        int[] new_tree = new int[Counter.Species];
        for (int i = 0; i < Counter.Species; i++)
        {
            new_tree[i] = (int)(CollectData.data[i, 12] * CollectData.data[i, 14]) / 100;
        }

        string CreateFolder = "OUTPUT";
        bool exists = System.IO.Directory.Exists(CreateFolder);
        if (!exists)
            System.IO.Directory.CreateDirectory(CreateFolder);
        else
        {
            DirectoryInfo dir = new DirectoryInfo("OUTPUT");
            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }
        }

        for (int simNum = 0; simNum < int.Parse(inputSceneStatus.SimulationNumber); simNum++)
        {
            for (int age = 1; age < int.Parse(inputSceneStatus.year); age++)
            {
                var zoi_trees = ZOI_Trees(trees);
                var FA_trees = CalculateFA(trees);
                var CFa_trees = Competition(FA_trees);
                var shadeToleranceVal = ShadeTolerance(FA_trees);

                int[] count_new_tree = new int[Counter.Species];

                List<Tree> treeToWrite = new List<Tree>();
                List<Tree> treeToWriteNewGen = new List<Tree>();

                for (int i = 0; i < trees.Count(); i++)
                {
                    double result = trees[i].dbh;
                    Tree tree = new Tree()
                    {
                        H = trees[i].H,
                        CFa = CFa_trees[i].CFa,
                        dbh = trees[i].dbh,
                        MaxDbh = trees[i].MaxDbh,
                        MaxHeight = trees[i].MaxHeight,
                        ShadeToleranceValue = shadeToleranceVal[i].ShadeToleranceValue,
                    };

                    double GR = Growth_Rate(tree);
                    result += GR;

                    if (GR > trees[i].CritDx)
                    {
                        Tree tree_to_write = new Tree()
                        {
                            index = trees[i].index,
                            X = trees[i].X,
                            Y = trees[i].Y,
                            Species = trees[i].Species,
                            dbh = result,
                            AGE = trees[i].AGE + 1,
                            a = trees[i].a,
                            b = trees[i].b,
                            H = (137 + (trees[i].b2 * result) - (trees[i].b3 * Math.Pow(result, 2))),
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
                            b2 = trees[i].b2,
                            b3 = trees[i].b3,
                            PercentageOfRegeneration = trees[i].PercentageOfRegeneration,
                        };
                        treeToWrite.Add(tree_to_write);
                    }

                    if (age > 5 && new_tree[(int)trees[i].Species - 1] > 0 && count_new_tree[(int)trees[i].Species - 1] < new_tree[(int)trees[i].Species - 1]) 
                    {
                        float randomDBH = (float)2.5 + UnityEngine.Random.Range(0.0f, 0.09f);
                        double rad = 15.233 * (trees[i].dbh / 2) + 1.1331;

                        float x, y;
                        while (true)
                        {
                            x = UnityEngine.Random.Range((float)(plotRandom.groundPosX - (plotRandom.groundWidth / 2)), (float)(plotRandom.groundPosX + (plotRandom.groundWidth / 2))) * 100;
                            y = UnityEngine.Random.Range((float)(plotRandom.groundPosZ - (plotRandom.groundLength / 2)), (float)(plotRandom.groundPosZ + (plotRandom.groundLength / 2))) * 100;

                            if (D(x, y, (float)trees[i].X, (float)trees[i].Y) > trees[i].rbh)
                            {
                                break;
                            }

                        }

                        Tree tree_to_write_new_gen = new Tree()
                        {
                            index = Index,
                            X = x,
                            Y = y,
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
                            b2 = trees[i].b2,
                            b3 = trees[i].b3,
                            PercentageOfRegeneration = trees[i].PercentageOfRegeneration,
                        };
                        treeToWriteNewGen.Add(tree_to_write_new_gen);

                        Index++;
                        count_new_tree[(int)trees[i].Species - 1]++;
                    }

                }
                // Debug.Log(treeToWrite.Count());
                treeToWrite.AddRange(treeToWriteNewGen);

                trees = treeToWrite;

                string fileName = "OUTPUT/Simulation_year_" + ((age + 1)).ToString() + "_Sim Num_" + simNum.ToString() + ".csv";
                var data = new StringBuilder();
                var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", "Index", "X", "Y", "Species", "DBH", "Age", "a", "b", "Height");
                data.AppendLine(header);
                foreach (var tree in trees)
                {
                    var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", tree.index, tree.X / 100, tree.Y / 100, tree.Species, tree.dbh, tree.AGE + 1, tree.a, tree.b, tree.H);
                    data.AppendLine(newLine);
                }
                File.WriteAllText(fileName, data.ToString());

            }
            trees = ReadTrees();

            foreach(var n in trees)
            {
                if (getDropDown.dropDownIndex == 1)
                {
                    break;
                }
                if (getDropDown.dropDownIndex==0)
                {
                    n.X = UnityEngine.Random.Range((float)(plotRandom.groundPosX - (plotRandom.groundWidth / 2)), (float)(plotRandom.groundPosX + (plotRandom.groundWidth / 2)))*100;
                    n.Y = UnityEngine.Random.Range((float)(plotRandom.groundPosZ - (plotRandom.groundLength / 2)), (float)(plotRandom.groundPosZ + (plotRandom.groundLength / 2)))*100;
                }               

                if(getDropDown.dropDownIndex==2)
                {
                    Cluster_Distribution();
                    int n1 = 0;
                    for (int i = 0; i < Counter.Species; i++)
                    {
                        for (int j = 0; j < (int)(CollectData.data[i, 12]); j++)
                        {
                            float posx = pos_trees[n1, 0];
                            float posz = pos_trees[n1, 1];
                            n.X = posx * 100;
                            n.Y = posz * 100;
                            n1++;
                        }

                    }
                }

                if(getDropDown.dropDownIndex==3)
                {
                    {
                        Repulsion_Distribution();

                        int n1 = 0;
                        for (int i = 0; i < Counter.Species; i++)
                        {
                            for (int j = 0; j < (int)(CollectData.data[i, 12]); j++)
                            {
                                float posx = pos_trees[n1, 0];
                                float posz = pos_trees[n1, 1];
                                n.X = posx * 100;
                                n.Y = posz * 100;

                                n1++;
                            }

                        }

                    }
                }
            }
            if (simNum == int.Parse(inputSceneStatus.SimulationNumber) - 1)
                start = 1;
        }
    }

    void Cluster_Distribution()
    {
        float cluster_percentage = float.Parse(inputSceneStatus.ClusterPercentage);
        int n_cluster = int.Parse(inputSceneStatus.ClusterNumber);
        float cluster_radius = float.Parse(inputSceneStatus.ClusterRadius);
        float x, z;
        float temp = (float)plotRandom.total / 100f * cluster_percentage;
        int clustered_trees = (int)temp;

        pos_trees = new float[plotRandom.total, 2];

        pos_trees[0, 0] = UnityEngine.Random.Range(plotRandom.groundPosX - plotRandom.groundWidth / 2 - 1, plotRandom.groundPosX + plotRandom.groundWidth / 2 - 1); // x coordinate
        pos_trees[0, 1] = UnityEngine.Random.Range(plotRandom.groundPosZ - plotRandom.groundLength / 2 - 1, plotRandom.groundPosZ + plotRandom.groundLength / 2 - 1); // z coordinate

        int n = 1;
        int stop = 0;
        while (n < n_cluster)
        {
            x = UnityEngine.Random.Range(plotRandom.groundPosX - plotRandom.groundWidth / 2 - 1, plotRandom.groundPosX + plotRandom.groundWidth / 2 - 1); // x coordinate
            z = UnityEngine.Random.Range(plotRandom.groundPosZ - plotRandom.groundLength / 2 - 1, plotRandom.groundPosZ + plotRandom.groundLength / 2 - 1); // z coordinate

            int flag = 0;
            for (int i = 0; i < n; i++)
            {
                if (D(x, z, pos_trees[i, 0], pos_trees[i, 1]) < cluster_radius * 2)
                {
                    stop++;
                    flag = 1;
                    break;
                }
            }
            if (stop == 10) //break point of infinity loop due to lack of appropriate place
                break;

            if (flag == 0)
            {
                pos_trees[n, 0] = x; // x coordinate
                pos_trees[n, 1] = z; // z coordinate
                n++;
                stop = 0;
            }
        }


        while (n < clustered_trees)
        {
            int index = UnityEngine.Random.Range(0, n_cluster);

            x = UnityEngine.Random.Range(pos_trees[index, 0] - cluster_radius, pos_trees[index, 0] + cluster_radius);
            z = UnityEngine.Random.Range(pos_trees[index, 1] - cluster_radius, pos_trees[index, 1] + cluster_radius);

            if (D(x, z, pos_trees[index, 0], pos_trees[index, 1]) <= cluster_radius && x > plotRandom.groundPosX - plotRandom.groundWidth / 2 && x < plotRandom.groundPosX + plotRandom.groundWidth / 2 && z > plotRandom.groundPosZ - plotRandom.groundLength / 2 && z < plotRandom.groundPosZ + plotRandom.groundLength / 2)
            {
                n++;
                pos_trees[n, 0] = x;
                pos_trees[n, 1] = z;

                Debug.Log(pos_trees[n, 0] + " " + pos_trees[n, 1]);

            }
        }

        while (n < plotRandom.total)
        {
            pos_trees[n, 0] = UnityEngine.Random.Range(plotRandom.groundPosX - plotRandom.groundWidth / 2 + 1, plotRandom.groundPosX + plotRandom.groundWidth / 2 - 1); // x coordinate
            pos_trees[n, 1] = UnityEngine.Random.Range(plotRandom.groundPosZ - plotRandom.groundLength / 2 + 1, plotRandom.groundPosZ + plotRandom.groundLength / 2 - 1); // z coordinate
            n++;
        }

    }

    static double D(float x1, float y1, float x2, float y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }

    void Repulsion_Distribution()
    {
        pos_trees = new float[plotRandom.total, 2];
        pos_trees[0, 0] = UnityEngine.Random.Range(plotRandom.groundPosX - plotRandom.groundWidth / 2, plotRandom.groundPosX + plotRandom.groundWidth / 2); // x coordinate
        pos_trees[0, 1] = UnityEngine.Random.Range(plotRandom.groundPosZ - plotRandom.groundLength / 2, plotRandom.groundPosZ + plotRandom.groundLength / 2); // z coordinate

        int count = 1;
        int stop = 0;
        float repulsion_distance = float.Parse(inputSceneStatus.RepulsionDistance);
        while (count < plotRandom.total)
        {
            float x, z;
            x = UnityEngine.Random.Range(plotRandom.groundPosX - plotRandom.groundWidth / 2, plotRandom.groundPosX + plotRandom.groundWidth / 2); // x coordinate
            z = UnityEngine.Random.Range(plotRandom.groundPosZ - plotRandom.groundLength / 2, plotRandom.groundPosZ + plotRandom.groundLength / 2); // z coordinate

            int flag = 0;
            for (int i = 0; i < count; i++)
            {
                if (D(x, z, pos_trees[i, 0], pos_trees[i, 1]) < repulsion_distance)
                {
                    stop++;
                    flag = 1;
                    break;
                }
            }
            if (stop == 5) //break point of infinity loop due to lack of appropriate place
                break;

            if (flag == 0)
            {
                pos_trees[count, 0] = x; // x coordinate
                pos_trees[count, 1] = z; // z coordinate
                count++;
                stop = 0;
            }
        }
        for (int i = 0; i < Counter.Species; i++)
        {
            Debug.Log(ColorPicker.SpeciesColor[i]);
        }
    }
}
