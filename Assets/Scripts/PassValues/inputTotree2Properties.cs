using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inputTotree2Properties : MonoBehaviour {

    public InputField maxHeight;
    public InputField maxDbh;
    public InputField constA;
    public InputField constB;
    public InputField constC;
    public InputField phi;
    public InputField minFON;
    public InputField saltEffect;
    public InputField salinity;
    public InputField constSaltEffect;
    public InputField maxAge;
    public InputField shadeToleranceRanking;

    public static float MaxHeight;
    public static float MaxDbh;
    public static float ConstA;
    public static float ConstB;
    public static float ConstC;
    public static float Phi;
    public static float MinFON;
    public static float SaltEffect;
    public static float Salinity;
    public static float ConstSaltEffect;
    public static float MaxAge;
    public static float ShadeToleranceRanking;


    void Update()
    {
       // if ((maxHeight != null) && (maxDbh != null) && (constA != null) && (constB != null) && (constC != null) && (saltEffect != null) && (salinity != null) && (constSaltEffect != null) && (maxAge != null) && (shadeToleranceRanking != null))
       // {
            try
            {
                /*MaxHeight = float.Parse(maxHeight.text);
                MaxDbh = float.Parse(maxDbh.text);
                ConstA = float.Parse(constA.text);
                ConstB = float.Parse(constB.text);
                ConstC = float.Parse(constC.text);
                Phi = float.Parse(phi.text);
                MinFON = float.Parse(minFON.text);
                SaltEffect = float.Parse(saltEffect.text);
                Salinity = float.Parse(salinity.text);
                ConstSaltEffect = float.Parse(constSaltEffect.text);
                MaxAge = float.Parse(maxAge.text);
                ShadeToleranceRanking = float.Parse(shadeToleranceRanking.text);*/

                MaxHeight = 3500f;//delete this when debugging done
                MaxDbh = 250f;//delete this when debugging done
                MaxAge = 500f;//delete this when debugging done
                ConstA = 7.99f;//delete this when debugging done
                ConstB = .5326f;//delete this when debugging done
                ConstC = .289f;//delete this when debugging done
                Phi = 1.3f;//delete this when debugging done
                MinFON = .1f;//delete this when debugging done
                SaltEffect = 1f;//delete this when debugging done
                Salinity = 1f;//delete this when debugging done
                ConstSaltEffect = 1f;//delete this when debugging done

                ShadeToleranceRanking = 9f;//delete this when debugging done
            }
            catch (Exception e) { }
        //}
        //Debug.Log(MaxHeight);
    }
}
