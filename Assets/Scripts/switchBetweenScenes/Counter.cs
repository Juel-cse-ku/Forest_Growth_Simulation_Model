using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour {

    public static int Count=1;
    public static int Species= int.Parse(inputSceneStatus.numTree1);

    public void increase()
    {
        Count++;
        if(Count> Species)
        {
            Count = 1;
        }
    }

    
   

}
