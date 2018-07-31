using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getDropDown : MonoBehaviour {

    List<string> distributionType = new List<string>() { "Random", "Uniform","Clustered" };
    public Dropdown dropdown;

    public static int dropDownIndex;

    public void DropDown_IndexChanged(int index)
    {
        dropDownIndex = index;
        Debug.Log(dropDownIndex);
    }

    
}
