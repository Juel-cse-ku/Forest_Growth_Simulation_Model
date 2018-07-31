using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class fromInputToSimulation : MonoBehaviour {
    

    public InputField length;
    public InputField width;
    public InputField tree1;
    public InputField tree2;
    public InputField tree3;
    public InputField ttime;

    public static int Length;
    public static int Width;
    public static int Tree1;
    public static int Tree2;
    public static int Tree3;
    public static int Time;



    void Update()
    {
        if ((length != null) && (width != null) && ((tree1 != null) || (tree2 != null) || (tree3 != null) || (ttime != null)))
        //if ((length.text !=null )&& (width.text!=null)&& (tree1.text!=null)&& (tree2.text != null)&& (tree3.text != null)&& (time.text != null))
        { 
            try
            {
                /*Length = int.Parse(length.text);
                Width = int.Parse(width.text);
                Tree1 = int.Parse(tree1.text);
                Tree2 = int.Parse(tree2.text);
                Tree3 = int.Parse(tree3.text);
                Time = int.Parse(ttime.text);*/ //uncomment when done

                Length = 10;//delete when debugged
                Width = 10;//delete when debugged
                Tree1 = 400;//delete when debugged
                Tree2 = 400;//delete when debugged
                Tree3 = 400;//delete when debugged
                Time = 20;//delete when debugged

               // Debug.Log("Time : "+Time);
            }
            catch(Exception e){}
            
           
            //Length = Int32.TryParse(length.text,out 0);
            /*Length = Convert.ToInt32(length.text.ToString());
            Width = Convert.ToInt32(width.text.ToString());
            Tree1 = Convert.ToInt32(tree1.text.ToString());
            Tree2 = Convert.ToInt32(tree2.text.ToString());
            Tree3 = Convert.ToInt32(tree3.text.ToString());
            Time = Convert.ToInt32(time.text.ToString());*/

        }
}

}
