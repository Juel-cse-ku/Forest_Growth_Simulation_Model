using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getDropDown : MonoBehaviour {

    List<string> distributionType = new List<string>() { "Random", "Uniform","Clustered","Repulsion" };
    public Dropdown dropdown;    
    public GameObject spacing;
    public GameObject cluster_percentage;
    public GameObject cluster_radius;
    public GameObject cluster_number;
    public GameObject repulsion_distance;

    public static int dropDownIndex;
    

    public void DropDown_IndexChanged(int index)
    {
        //spa = GameObject.Find("spacing");
        dropDownIndex = index;
       // Debug.Log(dropDownIndex);

        if(index==1)
        {
            spacing.SetActive(true);
        }
        else
        {
            spacing.SetActive(false);
        }
        if (index == 2)
        {
            cluster_percentage.SetActive(true);
            cluster_radius.SetActive(true);
            cluster_number.SetActive(true);
        }
        else
        {
            cluster_percentage.SetActive(false);
            cluster_radius.SetActive(false);
            cluster_number.SetActive(false);
        }
        if (index == 3)
        {
            repulsion_distance.SetActive(true);
        }
        else
        {
            repulsion_distance.SetActive(false);
        }
    }
    void Start()
    {
        spacing = GameObject.Find("spacing");
        cluster_percentage = GameObject.Find("cluster_percentage");
        cluster_radius = GameObject.Find("cluster_radius");
        cluster_number = GameObject.Find("cluster_number");
        repulsion_distance = GameObject.Find("repulsion_distance");
        spacing.SetActive(false);
        cluster_percentage.SetActive(false);
        cluster_radius.SetActive(false);
        cluster_number.SetActive(false);
        repulsion_distance.SetActive(false);
    }


}
