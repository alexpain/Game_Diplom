using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightScript : MonoBehaviour
{
    Quaternion originalRotation;
    float angle;
    // Use this for initialization
    void Start()
    {
        originalRotation = transform.rotation;
        angle = (float)0.0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angle += (float)0.006;
        Quaternion spin = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
        transform.rotation = originalRotation * spin;
    }
}
