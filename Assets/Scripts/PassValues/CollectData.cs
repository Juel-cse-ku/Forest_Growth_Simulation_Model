using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectData : MonoBehaviour {

    public InputField MaxHeight;
    public InputField MaxDBH;
    public InputField ConstA;
    public InputField ConstB;
    public InputField CritDx;
    public InputField Phi;
    public InputField MinFon;
    public InputField SaltEffectOnGrowth;
    public InputField Salinity;
    public InputField ConstForSaltEffect;
    public InputField MaxAge;
    public InputField STRating;
    public InputField NumOfSapling;
    public InputField PercenageOfRegeneration;

    public static double[,] data = new double[int.Parse(inputSceneStatus.numTree1), 15];

	public void GatherData()
    {
        try
        {
            data[Counter.Count - 1, 0] = double.Parse(MaxHeight.text);
        }
        catch(Exception e)
        {
            //data[Counter.Count - 1, 0] = 0;
            data[Counter.Count - 1, 0] = 3500;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 1] = double.Parse(MaxDBH.text);
        }
        catch (Exception e)
        {
            //data[Counter.Count - 1, 1] = 0;
            data[Counter.Count - 1, 1] = 450;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 2] = double.Parse(ConstA.text);
        }
        catch (Exception e)
        {
            //data[Counter.Count - 1, 2] = 0;
            data[Counter.Count - 1, 2] = 8.26;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 3] = double.Parse(ConstB.text);
        }
        catch (Exception e)
        {
            //data[Counter.Count - 1, 3] = 0;
            data[Counter.Count - 1, 3] = .5352;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 4] = double.Parse(CritDx.text);
        }
        catch (Exception e)
        {
            //data[Counter.Count - 1, 4] = 0;
            data[Counter.Count - 1, 4] = .589;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 5] = double.Parse(Phi.text);
        }
        catch (Exception e)
        {
           // data[Counter.Count - 1, 5] = 0;
            data[Counter.Count - 1, 5] = 1.3;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 6] = double.Parse(MinFon.text);
        }
        catch (Exception e)
        {
           // data[Counter.Count - 1, 6] = 0;
            data[Counter.Count - 1, 6] = 0.1;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 7] = double.Parse(SaltEffectOnGrowth.text);
        }
        catch (Exception e)
        {
            //data[Counter.Count - 1, 7] = 0;
            data[Counter.Count - 1, 7] = 1;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 8] = double.Parse(Salinity.text);
        }
        catch (Exception e)
        {
            data[Counter.Count - 1, 8] = 1;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 9] = double.Parse(ConstForSaltEffect.text);
        }
        catch (Exception e)
        {
            data[Counter.Count - 1, 9] = 1;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 10] = double.Parse(MaxAge.text);
        }
        catch (Exception e)
        {
            data[Counter.Count - 1, 10] = 500;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 11] = double.Parse(STRating.text);
        }
        catch (Exception e)
        {
            data[Counter.Count - 1, 11] = 9;
            Debug.Log(e);
        }

        try
        {
            data[Counter.Count - 1, 12] = double.Parse(NumOfSapling.text);
        }
        catch (Exception e)
        {
            data[Counter.Count - 1, 12] = 0;
            Debug.Log(e);
        }
        try
        {
            data[Counter.Count - 1, 14] = double.Parse(PercenageOfRegeneration.text);
            //Debug.Log(double.Parse(PercenageOfRegeneration.text));
        }
        catch (Exception e)
        {
            data[Counter.Count - 1, 14] = 0;
            Debug.Log(e);
        }

        data[Counter.Count - 1, 13] = Counter.Count;

        //data[Counter.Count - 1, 1] = double.Parse(MaxDBH.text);
        //data[Counter.Count - 1, 2] = double.Parse(ConstA.text);
        //data[Counter.Count - 1, 3] = double.Parse(ConstB.text);
        //data[Counter.Count - 1, 4] = double.Parse(CritDx.text);
        //data[Counter.Count - 1, 5] = double.Parse(Phi.text);
        //data[Counter.Count - 1, 6] = double.Parse(MinFon.text);
        //data[Counter.Count - 1, 7] = double.Parse(SaltEffectOnGrowth.text);
        //data[Counter.Count - 1, 8] = double.Parse(Salinity.text);
        //data[Counter.Count - 1, 9] = double.Parse(ConstForSaltEffect.text);
        //data[Counter.Count - 1, 10] = double.Parse(MaxAge.text);
        //data[Counter.Count - 1, 11] = double.Parse(STRating.text);
        
    }
}
