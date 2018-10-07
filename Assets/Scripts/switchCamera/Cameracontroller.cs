using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour {

   // public GameObject ground;
   // private Vector3 offset;

	// Use this for initialization
	void Start () {
      //  offset = transform.position - ground.transform.position;		
	}
	
	// Update is called once per frame
	void Update () {
     //   transform.position = ground.transform.position + 2*offset;

        if(Input.GetAxis("Mouse ScrollWheel")>0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x+5f,transform.position.y-5f,transform.position.z+5f);
           // transform.Rotate(-2,0,0);
           
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x-5f, transform.position.y + 5f, transform.position.z-5f);
            //transform.Rotate(2, 0, 0);
        }

    }
}
