using UnityEngine;
using System.Collections;

/// <summary>
/// Semi unit. - Semaphores manager (Unity)
/// </summary>

public class SemiUnit : MonoBehaviour 
	{
	public string SemaControl;	// Stipulates semaphore manager
	
	public int State;			// State of semaphore
	
		/* Semaphore switching light state */
	public void Take_Control(int std)
		{
		State = std;
		CompareLights();
		}
	
		/* Semaphore switchi light comparison */
	void CompareLights()
		{
		Lights[] light = gameObject.GetComponentsInChildren<Lights>();
		for(int i = 0; i<light.Length; i++)
			{
			if(light[i].TypeLight == State) { light[i].GetComponent<ParticleEmitter>().GetComponent<Renderer>().enabled = true; }
			else   						    { light[i].GetComponent<ParticleEmitter>().GetComponent<Renderer>().enabled = false; }
			}
		}



	// Use this for initialization
	void Start () 
		{
		State = 2;
		CompareLights();
		}
	
	// Update is called once per frame
	void Update () 
		{
		
		}
	
	}
