using UnityEngine;
using System.Collections;

/// <summary>
/// This code makes the control points invisible
/// </summary>

public class ByeByePDs : MonoBehaviour 
	{
	
	// Use this for initialization
	void Start () 
		{
		GameObject[] PDS = GameObject.FindGameObjectsWithTag("PD");
		
		for(int i=0; i<PDS.Length; i++)
			{
			PDS[i].GetComponent<ParticleEmitter>().GetComponent<Renderer>().enabled = false;
			}
		}
	
	// Update is called once per frame
	void Update () 
		{
		
		}
	
	}
