using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pausePlay : MonoBehaviour {

    public Text pPText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

     public void PausePlay()
    {
        if(Time.timeScale==0)
        {
            Time.timeScale = 1;
            pPText.text = "PAUSE";
        }
        else if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            pPText.text = "RESUME";
        }
    }
}
