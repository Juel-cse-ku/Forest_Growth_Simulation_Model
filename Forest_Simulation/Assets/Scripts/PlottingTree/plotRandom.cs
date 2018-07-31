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
    class Properties
    {
        public int Index { set; get; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double DBH { get; set; }
       // public double IsDeleted { get; set; }
        public int Species { get; set; }
        public int Age { get; set; }
    }

    static IList<Properties> Read_Properties(bool hasheaders = true)
    {
        var list = new List<Properties>();
        var path = "OUTPUT/Simulation_year_" + p + ".csv";
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
            // Debug.Log("file counter: in list: "+p);
            // Debug.Log(properties.X+" "+properties.Y +" "+properties.Height);
            //fileCounter++;
        }
        //fileCounter++;

        return list;
    }

    public float lifeTime = 10f;

    public GameObject ground;
    //public Terrain terrain;
    static List<GameObject> References;
    List<GameObject> References2;
    List<GameObject> References3;
    public static int numberOfObjects;
    public static int numberOfObjects2;
    public static int numberOfObjects3;
    public static int total;
    public static int n = 1;
    public static int time;


    private int currentObjects;
    private int currentObjects2;
    private int currentObjects3;
    public GameObject objectToPlace;
    public GameObject objectToPlace2;
    public GameObject objectToPlace3;

    public static float[,] prop;


    public static float groundWidth;
    public static float groundLength;
    public static int groundPosX;
    public static int groundPosZ;



    void Start()
    {
        Vector3 groundSize = GetComponent<Collider>().bounds.size;
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
        numberOfObjects = fromInputToSimulation.Tree1;
        numberOfObjects2 = fromInputToSimulation.Tree2;
        numberOfObjects3 = fromInputToSimulation.Tree3;
        time = int.Parse(inputSceneStatus.year);

        //        Debug.Log("1:" + numberOfObjects + " 2:" + numberOfObjects2 + " time:" + time);
        total = numberOfObjects + numberOfObjects2 + numberOfObjects3;

        prop = new float[total + 1, 16];

        /* time = fromInputToSimulation.Time;
         Debug.Log(time);*/



        // while (currentObjects < numberOfObjects || currentObjects2 < numberOfObjects2 || currentObjects3 < numberOfObjects3)
        // {
        Debug.Log(getDropDown.dropDownIndex);
        if(getDropDown.dropDownIndex==0)
        { 
            while (currentObjects < numberOfObjects)
            {
                float posx = UnityEngine.Random.Range((float)(groundPosX - (groundWidth / 2)), (float)(groundPosX + (groundWidth / 2)));
                float posz = UnityEngine.Random.Range((float)(groundPosZ - (groundLength / 2)), (float)(groundPosZ + (groundLength / 2)));
                prop[n, 0] = n;
                prop[n, 1] = (posx) * 100;
                prop[n, 2] = (posz) * 100;
                prop[n, 3] = inputTotree1Properties.MaxHeight;
                prop[n, 4] = inputTotree1Properties.MaxDbh;
                prop[n, 5] = inputTotree1Properties.ConstA;
                prop[n, 6] = inputTotree1Properties.ConstB;
                prop[n, 7] = inputTotree1Properties.ConstC;
                prop[n, 8] = inputTotree1Properties.Phi;
                prop[n, 9] = inputTotree1Properties.MinFON;
                prop[n, 10] = inputTotree1Properties.SaltEffect;
                prop[n, 11] = inputTotree1Properties.Salinity;
                prop[n, 12] = inputTotree1Properties.ConstSaltEffect;
                prop[n, 13] = inputTotree1Properties.MaxAge;
                prop[n, 14] = inputTotree1Properties.ShadeToleranceRanking;
                prop[n, 15] = 1f;


                //  Debug.Log(prop[n, 1] + " " + prop[n, 2] + " " + prop[n, 3] + " " + prop[n, 4] + " " + prop[n, 5] + " " + prop[n, 6] + " " + prop[n, 7] + " " + prop[n, 8] + " " + prop[n, 9] + " " + prop[n, 10] + " " + prop[n, 11] + " " + prop[n, 12] + " " + prop[n, 13] + " " + prop[n, 14]);
                currentObjects = currentObjects + 1;
                n++;

            }
            while (currentObjects2 < numberOfObjects2 && inputSceneStatus.tree2isOn)
            {
                float posx = UnityEngine.Random.Range((float)(groundPosX - (groundWidth / 2)), (float)(groundPosX + (groundWidth / 2)));
                float posz = UnityEngine.Random.Range((float)(groundPosZ - (groundLength / 2)), (float)(groundPosZ + (groundLength / 2)));

                prop[n, 0] = n;
                prop[n, 1] = (posx) * 100;
                prop[n, 2] = (posz) * 100;
                prop[n, 3] = inputTotree2Properties.MaxHeight;
                prop[n, 4] = inputTotree2Properties.MaxDbh;
                prop[n, 5] = inputTotree2Properties.ConstA;
                prop[n, 6] = inputTotree2Properties.ConstB;
                prop[n, 7] = inputTotree2Properties.ConstC;
                prop[n, 8] = inputTotree2Properties.Phi;
                prop[n, 9] = inputTotree2Properties.MinFON;
                prop[n, 10] = inputTotree2Properties.SaltEffect;
                prop[n, 11] = inputTotree2Properties.Salinity;
                prop[n, 12] = inputTotree2Properties.ConstSaltEffect;
                prop[n, 13] = inputTotree2Properties.MaxAge;
                prop[n, 14] = inputTotree2Properties.ShadeToleranceRanking;
                prop[n, 15] = 2f;
                // Debug.Log(prop[n, 1] + " " + prop[n, 2] + " " + prop[n, 3] + " " + prop[n, 4] + " " + prop[n, 5] + " " + prop[n, 6] + " " + prop[n, 7] + " " + prop[n, 8] + " " + prop[n, 9] + " " + prop[n, 10] + " " + prop[n, 11] + " " + prop[n, 12] + " " + prop[n, 13] + " " + prop[n, 14]);
                currentObjects2 = currentObjects2 + 1;
                n++;
            }
            while (currentObjects3 < numberOfObjects3 && inputSceneStatus.tree3isOn)
            {
                float posx = UnityEngine.Random.Range((float)(groundPosX - (groundWidth / 2)), (float)(groundPosX + (groundWidth / 2)));
                float posz = UnityEngine.Random.Range((float)(groundPosZ - (groundLength / 2)), (float)(groundPosZ + (groundLength / 2)));
                prop[n, 0] = n;
                prop[n, 1] = (posx) * 100;
                prop[n, 2] = (posz) * 100;
                prop[n, 3] = inputTotree3Properties.MaxHeight;
                prop[n, 4] = inputTotree3Properties.MaxDbh;
                prop[n, 5] = inputTotree3Properties.ConstA;
                prop[n, 6] = inputTotree3Properties.ConstB;
                prop[n, 7] = inputTotree3Properties.ConstC;
                prop[n, 8] = inputTotree3Properties.Phi;
                prop[n, 9] = inputTotree3Properties.MinFON;
                prop[n, 10] = inputTotree3Properties.SaltEffect;
                prop[n, 11] = inputTotree3Properties.Salinity;
                prop[n, 12] = inputTotree3Properties.ConstSaltEffect;
                prop[n, 13] = inputTotree3Properties.MaxAge;
                prop[n, 14] = inputTotree3Properties.ShadeToleranceRanking;
                prop[n, 15] = 3f;

                //References.Add(Instantiate(objectToPlace3, new Vector3(posx, 0, posz), Quaternion.identity));
                // Debug.Log(prop[n, 1] + " " + prop[n, 2] + " " + prop[n, 3] + " " + prop[n, 4] + " " + prop[n, 5] + " " + prop[n, 6] + " " + prop[n, 7] + " " + prop[n, 8] + " " + prop[n, 9] + " " + prop[n, 10] + " " + prop[n, 11] + " " + prop[n, 12] + " " + prop[n, 13] + " " + prop[n, 14]);
                currentObjects3 = currentObjects3 + 1;
                // Debug.Log(currentObjects3);
                n++;
            }
    }
        if (getDropDown.dropDownIndex == 1)
        {
            n = 0;
            float spacing = (float)Math.Sqrt((groundLength * groundWidth) / total);
           // Debug.Log(spacing);
            float posx = (-groundLength / 2) + spacing / 2;
            float posz = (-groundWidth / 2) + spacing / 2;
            currentObjects = 0;
            while (currentObjects < numberOfObjects || currentObjects2 < numberOfObjects2 || currentObjects3 < numberOfObjects3)
            {
                
                if(currentObjects < numberOfObjects)
                {
                    if (posx < groundLength / 2)
                    {
                        posx = posx + spacing;
                        posz = posz;
                    }
                    if (posx > groundLength / 2)
                    {
                        posx = -groundLength / 2;
                        posz = posz + spacing;
                    }

                    //Debug.Log("posx=" + posx + "posz=" + posz);

                    prop[n, 0] = n;
                    prop[n, 1] = (posx) * 100;
                    prop[n, 2] = (posz) * 100;
                    prop[n, 3] = inputTotree1Properties.MaxHeight;
                    prop[n, 4] = inputTotree1Properties.MaxDbh;
                    prop[n, 5] = inputTotree1Properties.ConstA;
                    prop[n, 6] = inputTotree1Properties.ConstB;
                    prop[n, 7] = inputTotree1Properties.ConstC;
                    prop[n, 8] = inputTotree1Properties.Phi;
                    prop[n, 9] = inputTotree1Properties.MinFON;
                    prop[n, 10] = inputTotree1Properties.SaltEffect;
                    prop[n, 11] = inputTotree1Properties.Salinity;
                    prop[n, 12] = inputTotree1Properties.ConstSaltEffect;
                    prop[n, 13] = inputTotree1Properties.MaxAge;
                    prop[n, 14] = inputTotree1Properties.ShadeToleranceRanking;
                    prop[n, 15] = 1f;

                    currentObjects = currentObjects + 1;
                    //currentObjects2++;
                    //currentObjects3++;
                    n++;
                }
                

                if (currentObjects2 < numberOfObjects2)
                {
                    if (posx < groundLength / 2)
                    {
                        posx = posx + spacing;
                        posz = posz;
                    }
                    if (posx >= groundLength / 2)
                    {
                        posx = -groundLength / 2;
                        posz = posz + spacing;
                    }

                  //  Debug.Log("posx=" + posx + "posz=" + posz);

                    prop[n, 0] = n;
                    prop[n, 1] = (posx) * 100;
                    prop[n, 2] = (posz) * 100;
                    prop[n, 3] = inputTotree2Properties.MaxHeight;
                    prop[n, 4] = inputTotree2Properties.MaxDbh;
                    prop[n, 5] = inputTotree2Properties.ConstA;
                    prop[n, 6] = inputTotree2Properties.ConstB;
                    prop[n, 7] = inputTotree2Properties.ConstC;
                    prop[n, 8] = inputTotree2Properties.Phi;
                    prop[n, 9] = inputTotree2Properties.MinFON;
                    prop[n, 10] = inputTotree2Properties.SaltEffect;
                    prop[n, 11] = inputTotree2Properties.Salinity;
                    prop[n, 12] = inputTotree2Properties.ConstSaltEffect;
                    prop[n, 13] = inputTotree2Properties.MaxAge;
                    prop[n, 14] = inputTotree2Properties.ShadeToleranceRanking;
                    prop[n, 15] = 2f;

                    //currentObjects = currentObjects + 1;
                    currentObjects2++;
                    //currentObjects3++;
                    n++;
                }

                if (currentObjects3 < numberOfObjects3)
                {
                    if (posx < groundLength / 2)
                    {
                        posx = posx + spacing;
                        posz = posz;
                    }
                    if (posx >= groundLength / 2)
                    {
                        posx = -groundLength / 2;
                        posz = posz + spacing;
                    }

                 //   Debug.Log("posx=" + posx + "posz=" + posz);

                    prop[n, 0] = n;
                    prop[n, 1] = (posx) * 100;
                    prop[n, 2] = (posz) * 100;
                    prop[n, 3] = inputTotree3Properties.MaxHeight;
                    prop[n, 4] = inputTotree3Properties.MaxDbh;
                    prop[n, 5] = inputTotree3Properties.ConstA;
                    prop[n, 6] = inputTotree3Properties.ConstB;
                    prop[n, 7] = inputTotree3Properties.ConstC;
                    prop[n, 8] = inputTotree3Properties.Phi;
                    prop[n, 9] = inputTotree3Properties.MinFON;
                    prop[n, 10] = inputTotree3Properties.SaltEffect;
                    prop[n, 11] = inputTotree3Properties.Salinity;
                    prop[n, 12] = inputTotree3Properties.ConstSaltEffect;
                    prop[n, 13] = inputTotree3Properties.MaxAge;
                    prop[n, 14] = inputTotree3Properties.ShadeToleranceRanking;
                    prop[n, 15] = 3f;

                   // currentObjects = currentObjects + 1;
                    //currentObjects2++;
                    currentObjects3++;
                    n++;
                }



                //prop[n, 0] = n;
                //prop[n, 1] = (posx) * 100;
                //prop[n, 2] = (posz) * 100;
                //prop[n, 3] = inputTotree2Properties.MaxHeight;
                //prop[n, 4] = inputTotree2Properties.MaxDbh;
                //prop[n, 5] = inputTotree2Properties.ConstA;
                //prop[n, 6] = inputTotree2Properties.ConstB;
                //prop[n, 7] = inputTotree2Properties.ConstC;
                //prop[n, 8] = inputTotree2Properties.Phi;
                //prop[n, 9] = inputTotree2Properties.MinFON;
                //prop[n, 10] = inputTotree2Properties.SaltEffect;
                //prop[n, 11] = inputTotree2Properties.Salinity;
                //prop[n, 12] = inputTotree2Properties.ConstSaltEffect;
                //prop[n, 13] = inputTotree2Properties.MaxAge;
                //prop[n, 14] = inputTotree2Properties.ShadeToleranceRanking;
                //prop[n, 15] = 2f;
                //// Debug.Log(prop[n, 1] + " " + prop[n, 2] + " " + prop[n, 3] + " " + prop[n, 4] + " " + prop[n, 5] + " " + prop[n, 6] + " " + prop[n, 7] + " " + prop[n, 8] + " " + prop[n, 9] + " " + prop[n, 10] + " " + prop[n, 11] + " " + prop[n, 12] + " " + prop[n, 13] + " " + prop[n, 14]);
                //currentObjects2 = currentObjects2 + 1;
                //n++;

            }

            //while (currentObjects3 < numberOfObjects3 && inputSceneStatus.tree3isOn)
            //{
            //  //  float posx = UnityEngine.Random.Range((float)(groundPosX - (groundWidth / 2)), (float)(groundPosX + (groundWidth / 2)));
            //    //float posz = UnityEngine.Random.Range((float)(groundPosZ - (groundLength / 2)), (float)(groundPosZ + (groundLength / 2)));
            //    prop[n, 0] = n;
            //    prop[n, 1] = (posx) * 100;
            //    prop[n, 2] = (posz) * 100;
            //    prop[n, 3] = inputTotree3Properties.MaxHeight;
            //    prop[n, 4] = inputTotree3Properties.MaxDbh;
            //    prop[n, 5] = inputTotree3Properties.ConstA;
            //    prop[n, 6] = inputTotree3Properties.ConstB;
            //    prop[n, 7] = inputTotree3Properties.ConstC;
            //    prop[n, 8] = inputTotree3Properties.Phi;
            //    prop[n, 9] = inputTotree3Properties.MinFON;
            //    prop[n, 10] = inputTotree3Properties.SaltEffect;
            //    prop[n, 11] = inputTotree3Properties.Salinity;
            //    prop[n, 12] = inputTotree3Properties.ConstSaltEffect;
            //    prop[n, 13] = inputTotree3Properties.MaxAge;
            //    prop[n, 14] = inputTotree3Properties.ShadeToleranceRanking;
            //    prop[n, 15] = 3f;

            //    //References.Add(Instantiate(objectToPlace3, new Vector3(posx, 0, posz), Quaternion.identity));
            //    // Debug.Log(prop[n, 1] + " " + prop[n, 2] + " " + prop[n, 3] + " " + prop[n, 4] + " " + prop[n, 5] + " " + prop[n, 6] + " " + prop[n, 7] + " " + prop[n, 8] + " " + prop[n, 9] + " " + prop[n, 10] + " " + prop[n, 11] + " " + prop[n, 12] + " " + prop[n, 13] + " " + prop[n, 14]);
            //    currentObjects3 = currentObjects3 + 1;
            //    // Debug.Log(currentObjects3);
            //    n++;
            //}
        }
        // }
        if (currentObjects == numberOfObjects && currentObjects2 == numberOfObjects2 && currentObjects3 == numberOfObjects3)
        {
            Debug.Log("Generate objects complete!");
         //   Debug.Log(prop.GetLength(0));
        }


    }
    static Vector3 temp;
    static Vector3 temp2;
    static Vector3 temp3;
    static Vector3 init = new Vector3(.025f, .025f, .025f);
    static Vector3 init2 = new Vector3(.1f, .1f, .1f);
    static Vector3 init3 = new Vector3(.1f, .1f, .1f);


    static int p = 2;

    void Update()
    {
        if (Growth.start == 1)
        {
            try
        {
            

            if (p <= int.Parse(inputSceneStatus.year) )
            {
                var properties = Read_Properties();
               // Debug.Log("prop count: " + properties.Count);
                //Debug.Log("File Counter:" + p);
                fileCounter++;
                for (int i = 0; i < properties.Count; i++)
                {

                    if (properties[i].Age == 2)
                        Instantiate(((float)properties[i].X ), ((float)properties[i].Y ), properties[i].Species);
                    References[i].transform.localScale = init;
                    if (temp.x < (((float)properties[i].DBH) * (groundLength / 100) / 200) && temp.y < (((float)properties[i].Height) * (groundLength / 100) / 3000) && temp.z < (((float)properties[i].DBH) * (groundLength / 100) / 200 ))
                    {
                        temp = References[i].transform.localScale;
                        //temp.x += (((float)properties[i].DBH) / 100) * Time.deltaTime;
                        //temp.y += (((float)properties[i].Height) / 3000) * Time.deltaTime;
                        //temp.z += (((float)properties[i].DBH) / 100) * Time.deltaTime;

                        temp.x = (((float)properties[i].DBH) / 100) ;
                        temp.y = (((float)properties[i].Height) / 3000) ;
                        temp.z = (((float)properties[i].DBH) / 100) ;

                        References[i].transform.localScale = temp;
                        init = temp;
                    }
                        //if (properties[i] == 1)
                        //{
                        //    Destroy(References[i]);
                        //}


                        //Debug.Log("Initial: "+ References[1].transform.localScale.y);
                        // if(i==0)


                    }
                p++;
                fileCounter = 0;
                //i++;
                /* References[i].transform.localScale = sc;*/
            }
        }
        catch (Exception e) {
                Debug.Log("Exception found"+e.Message);
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
        if (species == 1) 
            References.Add(Instantiate(objectToPlace, new Vector3(posx, 0, posz), Quaternion.identity));
        if (species == 2) 
            References.Add(Instantiate(objectToPlace2, new Vector3(posx, 0, posz), Quaternion.identity));
        if (species == 3)
            References.Add(Instantiate(objectToPlace3, new Vector3(posx, 0, posz), Quaternion.identity));
        

    }

}