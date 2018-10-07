using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleSwitch3 : MonoBehaviour {

    public InputField InputField3;
    public Button properties3;
    public Toggle toggle3;

    void Update()
    {
        if (toggleSwitch2.forToggle3 == 1)
        {
            toggle3.interactable = true;
        }
        else
        {
            toggle3.interactable = false;
            InputField3.interactable = false;
            properties3.interactable = false;
        }
    }

    public void OnchangeValue()
    {
        /*if(toggleSwitch2.forToggle3==1)
        {
            toggle3.interactable = true;
        }
        else
            toggle3.interactable = false;*/
        bool statusOn = gameObject.GetComponent<Toggle>().isOn;
        if (statusOn == true && toggle3.interactable == true )
        {
            //InputField2.SetActive(true);
            InputField3.interactable = true;
            properties3.interactable = true;
            
            //Debug.Log("Toggle is on");
            //properties2.SetActive(true);
        }
        if (statusOn == false || toggle3.interactable == false)
        //else
        {
            InputField3.interactable = false;
            properties3.interactable = false;
            
            Debug.Log("Toggle is interactable false");
            //InputField2.SetActive(false);
            //properties2.SetActive(false);
        }
    }

}
