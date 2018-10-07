using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeSceneOnCondition : MonoBehaviour {

    public void changeScene()
    {
        if (Counter.Count < int.Parse(inputSceneStatus.numTree1))
        {
            Application.LoadLevel("propertiesofTree1");

        }

        else
        {
            Application.LoadLevel("inputScene");
        }
            
    }
}
