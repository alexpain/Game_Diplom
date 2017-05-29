using UnityEngine;
using System.Collections;

public class Sides_Detector : MonoBehaviour 
	{

	public bool Invert;		// Changing lane vehicle indicator
	public int 	ID_Vehicle;	// Vehicle ID detector
	public bool Side;     	// Vehicle lane indicator
	
	// Use this for initialization
	void Start () 
		{
		Invert = false;
		}
	
	// Update is called once per frame
	void Update () 
		{
		
		}
	
		/* Vehicles collision area (enter) */
	void OnTriggerEnter(Collider other) 
		{
		
		
		if(other.gameObject.tag == "ET_AI") 
			{
			
			if(other.gameObject.GetComponent<Vehicle_Control>().Vehicle_ID != ID_Vehicle)
				{
				Invert = true;
				
				}
			}
		}
	
		/* Vehicles collision area (stay) */
	void OnTriggerStay(Collider other) 
		{
		if(other.gameObject.tag == "ET_AI") 
			{
			
			if(other.gameObject.GetComponent<Vehicle_Control>().Vehicle_ID != ID_Vehicle)
				{
				Invert = true;
				
				}
			}
		}
	
		/* Vehicles collision area (exit) */
	void OnTriggerExit(Collider other) 
		{
		Invert = false;
		}
	
	}