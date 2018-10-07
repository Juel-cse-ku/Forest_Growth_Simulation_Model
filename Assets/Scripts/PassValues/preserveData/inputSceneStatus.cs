
//This script is to preserve the data from inputScene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class inputSceneStatus : MonoBehaviour {

    public InputField NumTree1;
    public InputField Spacing;
    public InputField cluster_percentage;
    public InputField cluster_radius;
    public InputField cluster_number;
    public InputField repulsion_distance;
    public InputField simulation_number;

    public InputField GrLen;
    public InputField GrWid;
    public InputField Year;

    public Button propTree1;

    public Dropdown dropdown;

    
    public static bool tree1InputInteractability;

    public static bool prop1Interactability;

    public static string numTree1;
    public static string spacing;
    public static string ClusterPercentage;
    public static string ClusterRadius;
    public static string ClusterNumber;
    public static string RepulsionDistance;
    public static string SimulationNumber;
    public static int ddSpacingCheck;

    public static string grLen;
    public static string grWid;
    public static string year;

    static int state = 0;

    void Start()
    {
        
        if (state==1)
        {
            NumTree1.text = numTree1;
            Spacing.text = spacing;
            cluster_percentage.text = ClusterPercentage;
            cluster_radius.text = ClusterRadius;
            cluster_number.text = ClusterNumber;
            repulsion_distance.text = RepulsionDistance;
            simulation_number.text = SimulationNumber;

            GrLen.text = grLen;
            GrWid.text = grWid;
            Year.text = year;
        }
    }
    public void OnClick()
    {
       try
        {
            numTree1 = NumTree1.text;
            spacing = Spacing.text;

            try
            {
                if (int.Parse(spacing)!=0 || Spacing!=null )
                {
                    ddSpacingCheck = 1;
                }
                
            }
            catch (Exception e) { ddSpacingCheck = 0; }
            //numTree3 = NumTree3.text;
            ClusterPercentage = cluster_percentage.text;
            ClusterRadius = cluster_radius.text;
            ClusterNumber = cluster_number.text;
            RepulsionDistance = repulsion_distance.text;
            SimulationNumber = simulation_number.text;
            grLen = GrLen.text;
            grWid = GrWid.text;
            year = Year.text; //comment this when debugging

            state = 1;



        }
        catch(Exception e)
        {
           // Debug.Log(NumTree1.text);
        }
    }


}
