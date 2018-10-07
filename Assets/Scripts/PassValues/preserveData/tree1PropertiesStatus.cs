using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class tree1PropertiesStatus : MonoBehaviour {

    public InputField MaxHeight;
    public InputField MaxDbh;
    public InputField ConstA;
    public InputField ConstB;
    public InputField ConstC;
    public InputField Phi;
    public InputField MinFON;
    public InputField SaltEffect;
    public InputField Salinity;
    public InputField ConstSaltEffect;
    public InputField MaxAge;
    public InputField ShadeToleranceRanking;


    public static string maxHeight;
    public static string maxDbh;
    public static string constA;
    public static string constB;
    public static string constC;
    public static string phi;
    public static string minFON;
    public static string saltEffect;
    public static string salinity;
    public static string constSaltEffect;
    public static string maxAge;
    public static string shadeToleranceRanking;

    static int state = 0;

    // Use this for initialization
    void Start ()
    {
        if (state == 1)
        {
            MaxHeight.text = maxHeight;
            MaxDbh.text = maxDbh;
            ConstA.text = constA;
            ConstB.text = constB;
            ConstC.text = constC;
            Phi.text = phi; ;
            MinFON.text = minFON;
            SaltEffect.text = saltEffect;
            Salinity.text = salinity;
            ConstSaltEffect.text=constSaltEffect;
            MaxAge.text = maxAge;
            ShadeToleranceRanking.text = shadeToleranceRanking;
        }
    }

    public void OnClick()
    {
        try
        {
            maxHeight = MaxHeight.text;
            maxDbh = MaxDbh.text;
            constA = ConstA.text;
            constB = ConstB.text;
            constC = ConstC.text;
            phi = Phi.text;
            minFON = MinFON.text;
            saltEffect = SaltEffect.text;
            salinity = Salinity.text;
            constSaltEffect = ConstSaltEffect.text;
            maxAge = MaxAge.text;
            shadeToleranceRanking = ShadeToleranceRanking.text;
            state = 1;
        }
        catch(Exception e)
        { }
    }
	
}
