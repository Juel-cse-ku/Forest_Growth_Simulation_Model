using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
public class plotRandom : MonoBehaviour
{
    static int fileCounter = 0;
    static public int SimYear;
    static string color;
    static Color c;
    class Properties
    {
        public int Index { set; get; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double DBH { get; set; }
        public int Species { get; set; }
        public int Age { get; set; }
    }

    static IList<Properties> Read_Properties(bool hasheaders = true)
    {
        var list = new List<Properties>();
        var path = "OUTPUT/Simulation_year_" + p + "_Sim Num_" + SimYear.ToString() + ".csv";
        //Debug.Log(path);
        foreach (var line in File.ReadAllLines(path).Skip(hasheaders ? 1 : 0))
        {
            var data = line.Split(',');
            Properties properties = new Properties()
            {
                Index = int.Parse(data[0]),
                X = float.Parse(data[1]),
                Y = float.Parse(data[2]),
                Species = int.Parse(data[3]),
                DBH = float.Parse(data[4]),
                Age = int.Parse(data[5]),
                Height = float.Parse(data[8]),
            };
            list.Add(properties);
        }
        //fileCounter++;

        return list;
    }



    public GameObject ground;
    //public Terrain terrain;
    static List<GameObject> References;
    List<GameObject> References2;
    List<GameObject> References3;
    public static int numberOfObjects;
    public static int numberOfObjects2;
    public static int numberOfObjects3;
    public static int total = 0;
    public static int n;
    public static int time;


    private int currentObjects;
    private int currentObjects2;
    private int currentObjects3;
    public GameObject objectToPlace;
    private GameObject[] objectToPlace2;
    public GameObject objectToPlace3;

    public static float[,] prop;
    public static float[,] forUniform;
    public static float[,] pos_trees;


    public static float groundWidth;
    public static float groundLength;
    public static int groundPosX;
    public static int groundPosZ;

    public Material[] SpeciesMaterial = new Material[Counter.Species];

    void Start()
    {
        for (int i = 0; i < Counter.Species; i++)
        {
            SpeciesMaterial[i] = new Material(Shader.Find("Standard"));
            SpeciesMaterial[i].color = ColorPicker.SpeciesColor[i];
        }

        SimYear = 0;
        //Vector3 groundSize = GetComponent<Collider>().bounds.size;
        References = new List<GameObject>();
        References2 = new List<GameObject>();
        References3 = new List<GameObject>();

        //groundWidth = (int)groundSize.x;
        groundWidth = fromInputToSimulation.Width;
        //groundLength = (int)groundSize.z;
        groundLength = fromInputToSimulation.Length;

        groundPosX = (int)ground.transform.position.x;
        groundPosZ = (int)ground.transform.position.z;
        currentObjects = 0;
        currentObjects2 = 0;
        currentObjects3 = 0;
        //numberOfObjects = fromInputToSimulation.Tree1;
        //numberOfObjects2 = fromInputToSimulation.Tree2;
        //numberOfObjects3 = fromInputToSimulation.Tree3;
        time = int.Parse(inputSceneStatus.year);

        for(int i=0;i< int.Parse(inputSceneStatus.numTree1); i++)
        {
            total += (int) CollectData.data[i, 12];
          //  Debug.Log(total);
        }
        prop = new float[total + 1, 17];
        pos_trees = new float[total, 2];

       // Debug.Log(getDropDown.dropDownIndex);
        if (getDropDown.dropDownIndex==0)
        {            
            n = 0;
            for(int i=0;i<Counter.Species;i++)
            {
                for(int j=0; j<(int)(CollectData.data[i,12]);j++)
                {
                    float posx = UnityEngine.Random.Range((float)(groundPosX - (groundWidth / 2)), (float)(groundPosX + (groundWidth / 2)));
                    float posz = UnityEngine.Random.Range((float)(groundPosZ - (groundLength / 2)), (float)(groundPosZ + (groundLength / 2)));
                    prop[n, 0] = n+1;
                    prop[n, 1] = (posx) * 100;
                    prop[n, 2] = (posz) * 100;
                    prop[n, 3] = (float)CollectData.data[i,0];
                    prop[n, 4] = (float)CollectData.data[i, 1];
                    prop[n, 5] = (float)CollectData.data[i, 2];
                    prop[n, 6] = (float)CollectData.data[i, 3];
                    prop[n, 7] = (float)CollectData.data[i, 4];
                    prop[n, 8] = (float)CollectData.data[i, 5];
                    prop[n, 9] = (float)CollectData.data[i, 6];
                    prop[n, 10] = (float)CollectData.data[i, 7];
                    prop[n, 11] = (float)CollectData.data[i, 8];
                    prop[n, 12] = (float)CollectData.data[i, 9];
                    prop[n, 13] = (float)CollectData.data[i, 10];
                    prop[n, 14] = (float)CollectData.data[i, 11];
                    prop[n, 15] = (float)CollectData.data[i, 13];//species index
                    prop[n, 16] = (float)CollectData.data[i, 14];
                    n++;
                }
                
            }
    }
        
        if (getDropDown.dropDownIndex == 1)
        {
            float spacing = 0;
            if (inputSceneStatus.ddSpacingCheck==1)
            {
                spacing = int.Parse(inputSceneStatus.spacing);
            }
            else if(inputSceneStatus.ddSpacingCheck == 0)
            {
                spacing = (float)Math.Sqrt((groundLength * groundWidth) / total);
            }
            
            float posx = (-groundLength / 2) ;
            float posz = (-groundWidth / 2) ;

            int[] trace = new int[Counter.Species];
            Array.Clear(trace, 0, trace.Length);
            n = 0;
            for(int i=0;i<total;)
            {
                for (int j = 0; j < Counter.Species; j++) 
                {
                   
                    if (trace[j] >= CollectData.data[j, 12]) 
                    {
                        continue;
                    }

                    else
                    {
                        prop[n, 0] = n + 1;
                        prop[n, 1] = (posx) * 100;
                        prop[n, 2] = (posz) * 100;
                        if (posx < groundLength / 2 ) 
                        {
                            posx = posx + spacing;
                           // posz = posz;
                        }
                        //if (posx > groundLength / 2)
                        else
                        {
                            posx = -groundLength / 2;
                            if (posz < groundWidth / 2 - spacing) 
                            {
                                posz = posz + spacing;
                            }
                        }
                        //prop[n, 0] = n;
                        
                        prop[n, 3] = (float)CollectData.data[j, 0];
                        prop[n, 4] = (float)CollectData.data[j, 1];
                        prop[n, 5] = (float)CollectData.data[j, 2];
                        prop[n, 6] = (float)CollectData.data[j, 3];
                        prop[n, 7] = (float)CollectData.data[j, 4];
                        prop[n, 8] = (float)CollectData.data[j, 5];
                        prop[n, 9] = (float)CollectData.data[j, 6];
                        prop[n, 10] = (float)CollectData.data[j, 7];
                        prop[n, 11] = (float)CollectData.data[j, 8];
                        prop[n, 12] = (float)CollectData.data[j, 9];
                        prop[n, 13] = (float)CollectData.data[j, 10];
                        prop[n, 14] = (float)CollectData.data[j, 11];
                        prop[n, 15] = (float)CollectData.data[j, 13];
                        prop[n, 16] = (float)CollectData.data[i, 14];
                        trace[j]++;                      

                        i++;
                        n++;
                    }
                    
                }
            }

            for(int i=0;i<n;i++)
            {
                Debug.Log(prop[i,15]);
            }
            
        }

        if (getDropDown.dropDownIndex == 2)
        {
            Cluster_Distribution();

            n = 0;
            for (int i = 0; i < Counter.Species; i++)
            {
                for (int j = 0; j < (int)(CollectData.data[i, 12]); j++)
                {
                    float posx = pos_trees[n, 0];
                    float posz = pos_trees[n, 1];

                    
                    prop[n, 0] = n + 1;
                    prop[n, 1] = (posx) * 100;
                    prop[n, 2] = (posz) * 100;
                    prop[n, 3] = (float)CollectData.data[i, 0];
                    prop[n, 4] = (float)CollectData.data[i, 1];
                    prop[n, 5] = (float)CollectData.data[i, 2];
                    prop[n, 6] = (float)CollectData.data[i, 3];
                    prop[n, 7] = (float)CollectData.data[i, 4];
                    prop[n, 8] = (float)CollectData.data[i, 5];
                    prop[n, 9] = (float)CollectData.data[i, 6];
                    prop[n, 10] = (float)CollectData.data[i, 7];
                    prop[n, 11] = (float)CollectData.data[i, 8];
                    prop[n, 12] = (float)CollectData.data[i, 9];
                    prop[n, 13] = (float)CollectData.data[i, 10];
                    prop[n, 14] = (float)CollectData.data[i, 11];
                    prop[n, 15] = (float)CollectData.data[i, 13];//species index
                    prop[n, 16] = (float)CollectData.data[i, 14];
                    n++;
                }

            }

        }

        if (getDropDown.dropDownIndex == 3)
        {
            Repulsion_Distribution();

            n = 0;
            for (int i = 0; i < Counter.Species; i++)
            {
                for (int j = 0; j < (int)(CollectData.data[i, 12]); j++)
                {
                    //float posx = UnityEngine.Random.Range((float)(groundPosX - (groundWidth / 2)), (float)(groundPosX + (groundWidth / 2)));
                    //float posz = UnityEngine.Random.Range((float)(groundPosZ - (groundLength / 2)), (float)(groundPosZ + (groundLength / 2)));
                    float posx = pos_trees[n, 0];
                    float posz = pos_trees[n, 1];
                    prop[n, 0] = n + 1;
                    prop[n, 1] = (posx) * 100;
                    prop[n, 2] = (posz) * 100;
                    prop[n, 3] = (float)CollectData.data[i, 0];
                    prop[n, 4] = (float)CollectData.data[i, 1];
                    prop[n, 5] = (float)CollectData.data[i, 2];
                    prop[n, 6] = (float)CollectData.data[i, 3];
                    prop[n, 7] = (float)CollectData.data[i, 4];
                    prop[n, 8] = (float)CollectData.data[i, 5];
                    prop[n, 9] = (float)CollectData.data[i, 6];
                    prop[n, 10] = (float)CollectData.data[i, 7];
                    prop[n, 11] = (float)CollectData.data[i, 8];
                    prop[n, 12] = (float)CollectData.data[i, 9];
                    prop[n, 13] = (float)CollectData.data[i, 10];
                    prop[n, 14] = (float)CollectData.data[i, 11];
                    prop[n, 15] = (float)CollectData.data[i, 13];//species index
                    prop[n, 16] = (float)CollectData.data[i, 14];
                    n++;
                }

            }

        }


        // }
        


    }
    static Vector3 temp;
    static Vector3 temp2=new Vector3(0f,0f,0f);
    static Vector3 temp3;
    static Vector3 init = new Vector3(2f, 2f, 2f);
    static Vector3 init2 = new Vector3(.1f, .1f, .1f);
    static Vector3 init3 = new Vector3(.1f, .1f, .1f);


    static int p = 2;

    void Update()
    {
        if (Growth.start == 1)
        {
            try
            {
                if (p <= int.Parse(inputSceneStatus.year) && SimYear < int.Parse(inputSceneStatus.SimulationNumber)) 
                {
                    var properties = Read_Properties();
                    fileCounter++;
                    for (int i = 0; i < properties.Count; i++)
                    {
                       // Debug.Log(i);
                        if (properties[i].Age == 2 || properties[i].Age == 1)
                            Instantiate(((float)properties[i].X ), ((float)properties[i].Y ), properties[i].Species);

                        References[i].transform.localScale = init;
                        temp2 = References[i].transform.position;
                       // if (temp.x < (((float)properties[i].DBH) * (groundLength / 100) / 200) && temp.y < (((float)properties[i].Height) * (groundLength / 100) / 3000) && temp.z < (((float)properties[i].DBH) * (groundLength / 100) / 200))
                       // {
                            temp = References[i].transform.localScale;

                            temp.x = (((float)properties[i].DBH) / 50);
                            temp.y = (((float)properties[i].Height) / 1500);
                            temp.z = (((float)properties[i].DBH) / 50);
                            temp2.y = temp.y;
                            
                            References[i].transform.localScale = temp;
                            init = temp;
                       // }
                        References[i].transform.position = temp2;
                        //References.Remove(References[i]);


                    }
                    p++;
                    fileCounter = 0;
                }
                else
                {
                    References.Clear();
                    if (SimYear < int.Parse(inputSceneStatus.SimulationNumber)-1)
                    {
                        p = 2;
                        objectToPlace2 = GameObject.FindGameObjectsWithTag("clone");
                        for (int h = 0; h < objectToPlace2.Length; h++)
                        {
                            DestroyObject(objectToPlace2[h]);
                        }
                        SimYear++;
                    }

                }
            }
        catch (Exception e) {
                Debug.Log("Exception found"+e.Source);
            }

        

    }
}
    void Destruct()
    {
        Destroy(this.gameObject);
    }
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }

    void Instantiate(float x, float y,int Species)
    {
        float posx = x;
        float posz = y;
        int species = Species;

        var obj=(Instantiate(objectToPlace, new Vector3(posx, 0, posz), Quaternion.identity));
        obj.gameObject.tag = "clone";
        
        References.Add(obj);
        var cyl = obj.transform.Find("Sphere");
        Renderer r = cyl.GetComponent<Renderer>();
        r.enabled = true;
        r.material = SpeciesMaterial[species - 1];
        //Debug.Log(r.material.color);
    }

    static double D(float x1, float y1, float x2, float y2)
    {
        return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }

    void Repulsion_Distribution()
    {
        //grPosX = ground.transform.position.x;
        //grPosZ = ground.transform.position.z;

        pos_trees = new float[total, 2];
        pos_trees[0, 0] = UnityEngine.Random.Range(groundPosX - groundWidth / 2, groundPosX + groundWidth / 2); // x coordinate
        pos_trees[0, 1] = UnityEngine.Random.Range(groundPosZ - groundLength / 2, groundPosZ + groundLength / 2); // z coordinate

        int count = 1;
        int stop = 0;
        float repulsion_distance = float.Parse(inputSceneStatus.RepulsionDistance);
        while (count < total)
        {
            float x, z;
            x = UnityEngine.Random.Range(groundPosX - groundWidth / 2, groundPosX + groundWidth / 2); // x coordinate
            z = UnityEngine.Random.Range(groundPosZ - groundLength / 2, groundPosZ + groundLength / 2); // z coordinate

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
        for(int i=0; i<Counter.Species;i++)
        {
           Debug.Log(ColorPicker.SpeciesColor[i]);
        }
        //for (int n = 0; n < count; n++)
        //{
        //    Instantiate(tree, new Vector3(pos_trees[n, 0], 0, pos_trees[n, 1]), Quaternion.identity);
        //}
        Debug.Log(count);
        // Write_Trees(pos_trees);
    }

    void Cluster_Distribution()
    {
        float cluster_percentage = float.Parse(inputSceneStatus.ClusterPercentage);
        int n_cluster = int.Parse(inputSceneStatus.ClusterNumber);
        float cluster_radius = float.Parse(inputSceneStatus.ClusterRadius);
        float x, z;
        float temp = (float)total / 100f * cluster_percentage;
        int clustered_trees = (int)temp;

        pos_trees = new float[total, 2];

        pos_trees[0, 0] = UnityEngine.Random.Range(groundPosX - groundWidth / 2 - 1, groundPosX + groundWidth / 2 - 1); // x coordinate
        pos_trees[0, 1] = UnityEngine.Random.Range(groundPosZ - groundLength / 2 - 1, groundPosZ + groundLength / 2 - 1); // z coordinate

        int n = 1;
        int stop = 0;
        while (n < n_cluster)
        {
            x = UnityEngine.Random.Range(groundPosX - groundWidth / 2 - 1, groundPosX + groundWidth / 2 - 1); // x coordinate
            z = UnityEngine.Random.Range(groundPosZ - groundLength / 2 - 1, groundPosZ + groundLength / 2 - 1); // z coordinate

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

            if (D(x, z, pos_trees[index, 0], pos_trees[index, 1]) <= cluster_radius && x > groundPosX - groundWidth / 2 && x < groundPosX + groundWidth / 2 && z > groundPosZ - groundLength / 2 && z < groundPosZ + groundLength / 2)
            {
                n++;
                pos_trees[n, 0] = x;
                pos_trees[n, 1] = z;

                Debug.Log(pos_trees[n, 0] + " " + pos_trees[n, 1]);

            }
        }

        while (n < total)
        {
            pos_trees[n, 0] = UnityEngine.Random.Range(groundPosX - groundWidth / 2 + 1, groundPosX + groundWidth / 2 - 1); // x coordinate
            pos_trees[n, 1] = UnityEngine.Random.Range(groundPosZ - groundLength / 2 + 1, groundPosZ + groundLength / 2 - 1); // z coordinate
            n++;
        }
        
    }

    public void ChangeColor(Color col)
    {
        objectToPlace.GetComponent<Renderer>().material.color = col;
    }

}