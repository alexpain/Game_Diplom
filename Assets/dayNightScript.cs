using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Quaternion spin = Quaternion.AngleAxis(1, new Vector3(0, 1, 0));
        transform.rotation *= spin;
    }
}
