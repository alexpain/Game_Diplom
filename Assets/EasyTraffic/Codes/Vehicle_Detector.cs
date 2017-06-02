using UnityEngine;
using System.Collections;

public class Vehicle_Detector : MonoBehaviour 
	{
	
	public bool		VehicleAlert;			// Vehicle presence alert
	
	public Vector3	OtheVehiclePos;			// Vehicle data necessary to deflect
	public bool		OtheVehicleST;			// Vehicle data necessary to deflect (Same track)
	public bool		OtheVehicleIT;			// Vehicle data necessary to deflect (opposite track)
	
	public bool    	Frontal;				// Indicates if the detector is frontal;
	
	public int		ID_Vehicle;				// Vehicle ID detector
	
	
	// Use this for initialization
	void Start () 
		{
		OtheVehiclePos = new Vector3(0,0,0);
		
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
				VehicleAlert 	= true;
				OtheVehiclePos  = other.gameObject.transform.position;
				
				OtheVehicleST  	= other.gameObject.GetComponent<Vehicle_Control>().Car_Incorrect_Track;
				OtheVehicleIT	= other.gameObject.GetComponent<Vehicle_Control>().Opposite_Direction;
				}
			}
		}
	
		/* Vehicles collision area (exit) */
	void OnTriggerStay(Collider other) 
		{
		
		if(other.gameObject.tag == "ET_AI")
			{
			if(other.gameObject.GetComponent<Vehicle_Control>().Vehicle_ID != ID_Vehicle)
				{
				VehicleAlert 	= true;
				OtheVehiclePos  = other.gameObject.transform.position;
				
				OtheVehicleST	= other.gameObject.GetComponent<Vehicle_Control>().Car_Incorrect_Track;
				OtheVehicleIT	= other.gameObject.GetComponent<Vehicle_Control>().Opposite_Direction;
				}
			}
		}
	
	}