using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class properties : MonoBehaviour {

    public Text propertiesText;
    void Start()
    {
         propertiesText.text = "Parameters of Species : " + (Counter.Count);
        
    }

    public void Show()
    {
        if (Counter.Count > 1)
        {
            for (int i = 0; i < 12; i++)
            {
               // Debug.Log(Counter.Count);
               // Debug.Log(CollectData.data[Counter.Count - 1, i]);
            }
        }
    }
}
