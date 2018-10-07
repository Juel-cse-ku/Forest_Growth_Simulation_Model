using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGround : MonoBehaviour {

    //public float speed = 1f;
    static Vector3 temp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        int groundLen = fromInputToSimulation.Length;
        int groundWid = fromInputToSimulation.Width;
        temp = transform.localScale;

        if (temp.x<groundLen)
        {
            //temp.x += Time.deltaTime;
            temp.x += 1f;
           // GetComponent<Camera>().fieldOfView += .1f;
        }

        if(temp.z<groundWid)
        {
            //temp.z += Time.deltaTime;
            temp.z += 1f;
        }


        transform.localScale = temp;
        

    }
}
