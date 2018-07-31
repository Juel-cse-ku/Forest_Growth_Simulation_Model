using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleSwitch2 : MonoBehaviour {

    public InputField InputField2;
    public Button properties2;
    public static int forToggle3;
    public Toggle toggle3;
    public void OnchangeValue()
    {
        bool statusOn = gameObject.GetComponent<Toggle>().isOn;
        if (statusOn==true)
        {
            forToggle3 = 1;
            InputField2.interactable = true;
            properties2.interactable = true;
            Debug.Log("Toggle is on");
        }
        if(statusOn==false)
        {
            forToggle3 = 0;
            InputField2.interactable = false;
            properties2.interactable = false;
            Debug.Log("Toggle is off");
            toggle3.isOn = false;
            //InputField2.SetActive(false);
            //properties2.SetActive(false);
        }
    }
}

