using UnityEngine;
using System.Collections;

/// <summary>
/// Destroy. - This code represents the destruction of the vehicle
/// </summary>

public class Destroy : MonoBehaviour 
	{
	float TimeDie;	// Death time of the vehicle

	// Use this for initialization
	void Start () 
		{
		TimeDie = 1.5f;
		}
	
	// Update is called once per frame
	void Update () 
		{
		TimeDie -= Time.deltaTime;
		
		if(TimeDie <= 0.0f)
			{
			Destroy(this.gameObject);
			}
		}
	}
