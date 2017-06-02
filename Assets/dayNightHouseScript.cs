using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class dayNightHouseScript : MonoBehaviour {
    private double Rotate = 91;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Quaternion spinZ = Quaternion.AngleAxis(90*(float)Math.Sin(Rotate), new Vector3(1, 0, 0));
        Quaternion spinY = Quaternion.AngleAxis(270 + 60*(float)Math.Cos(Rotate), new Vector3(0, 1, 0));
        transform.rotation = spinZ * spinY;
        Rotate-=0.0001;
    }
}
