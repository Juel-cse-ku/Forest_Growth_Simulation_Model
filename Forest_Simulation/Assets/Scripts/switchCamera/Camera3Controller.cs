using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
            
            // transform.Rotate(-2,0,0);

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Transform>().position = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            //transform.Rotate(2, 0, 0);
        }
    }
}
