using UnityEngine;
using System.Collections;

public class Vehicle_Control : MonoBehaviour 
	{
	//public bool		Mostrar;
	
	public float 	Gear_Pressure;		// Gear pressure
	public float 	Max_Gear;			// Maximum Gear pressure
	
	public float	Aceleration;		// Vehicle aceleration
	public float 	Max_Aceleration;	// Max. Vehicle aceleration
	public float	Scale;				// Aceleration Scale
	
		   Vector3	StrWheel;			//steering wheel
	
		   Useful	UTL = new Useful(); // Utility object
	
		   
	public Vector3  Correct_Track;		// Correct track car 
	public Vector3  Incorrect_Track;	// Incorrect track car 
	public int		Lane;				// Wich lane is the car?
	public int		TotalLanes;			// Total lanes in current CCP
	public float	Position_Lane;		// Car position in the lane
	public float    Progress_Lane;		// Progress in the lane
	
	
		/* Vectores of the vehicle direction */
	public int		Current_CCP;		// Current Control Point (CCP)
	public CCP 		CPP_C;	
	public float	Vector_Size;		// Vector size between CCP's (1 -> 2)
		   float    Vector_Final_Size;	// Vector's final size 
		   Vector2	Direction;			// Direction correction vector
	public Vector2	Normal;				// Vector's adjust point (Normal CCP)
		   float    Degrees;			// Right angle for the vehicle
	 	   float    Slope;				// Variation of velocity in slopes
	
	
	public bool		Opposite_Direction;	// Direction of the vehicle
	public bool		Car_Incorrect_Track;// Incorrect track car?
	public int		Vehicle_ID;			// Vehicle ID
	
		   bool		Pos_Create;			// Flag to avoid Bug in the after creation


		/* NPC Data */
		   float	Dizzi_Time;			// Dizzy time of vehicle (After Crash)
		   bool		OtherVhcDirect;		// What's the lane of the other vehicle?
		   Vector3  OtherVhcPosition;	// Position of the other vehicle
		   float	OtherVhcAceleration;// Aceleration of the other vehicle
		   int		ThrowSide;			// Witch side the other vehicle is throw
		   float	ColisionForce;		// Collision force between two vehicles ( Real number )
		   Vector3  ColisionVector;			// Collision force between two vehicles ( Vector )
		   float    SpeedRotating;		// Velocity of the Crash spin
	
	public bool		ChangingGears;		// Increase the velocity of the vehicle on the first instant

	public Vector2  Fake_CCP1;			// Current fake CCP 
	public Vector2	Fake_CCP2;			// Next current fake CCP
	
			float   TimeContact;		// Contact time with another vehicle

			float	SemapTime;	




	public void SetScale(float s)
	{
		Scale = s;
		Vector3 ScaleV3 = new Vector3(Scale,Scale,Scale);
		transform.localScale = ScaleV3;
	}

		/* Adjust the new vector of CCP */
	void New_VPD()
		{
		if( CPP_C.NextCCP != null)
			{
			Vector2 A = UTL.Perpendicular_Vector(CPP_C.gameObject.transform.localPosition,1);
			Vector2 B = UTL.Perpendicular_Vector(CPP_C.NextCCP.gameObject.transform.localPosition ,1);
			
			Direction = UTL.Vector(A,B);
			
			Vector_Size 		= UTL.Vector_Size(Direction);
			Vector_Final_Size	= Vector_Size;
			
			Direction 	= UTL.Normalize_Vector(Direction);
			
			Degrees 	= UTL.AngleBetweenVectors(A,B);
			
			float Size_Way = (CPP_C.NextCCP.Size_Road * Progress_Lane);
		
			Vector2 Perpendicular = new Vector2(0,0);
			
			if(!Opposite_Direction)
				{
				Perpendicular 	= UTL.Perpendicular_Reta2d(Direction,true);
				}
			else
				{
				Perpendicular 	= UTL.Perpendicular_Reta2d(Direction,false);
				}
			
			Perpendicular 		= UTL.Adjust_Size(Perpendicular, Size_Way );
		
			Fake_CCP1 			= UTL.Perpendicular_Vector(CPP_C.gameObject.transform.position,1);
		
			Fake_CCP2 			= UTL.Perpendicular_Vector(CPP_C.NextCCP.gameObject.transform.position,1);
			
			Fake_CCP1 			= UTL.Vector_Addition(Fake_CCP1, Perpendicular);
			Fake_CCP2 			= UTL.Vector_Addition(Fake_CCP2, Perpendicular);
			}//*/
		}
	
	
	
		/**/
	void Reset_Values()
		{
		Gear_Pressure 			= 0.0f;
		if(!ChangingGears)		{ Aceleration	= 0.0f; }
		Normal 					= new Vector2(0,0);
		
		}
	
	

	public void SpawnSmoke()
	{
		GameObject Smoke = GameObject.Instantiate(Resources.Load("Smoke"), transform.position,Quaternion.identity) as GameObject;
		Smoke.name = "Smoke";
		Smoke.transform.parent = transform.parent.parent.Find("Effects");
	}


	
	// Use this for initialization
	void Start () 
		{
		SemapTime			= 0.0f;
		/*faamorim
		if(Lane == 0)		{ Lane 			= 1; }
		if( Lane == 1 ) 	{ Position_Lane = 0.5f; } //faamorim { Position_Lane = 0.85f; }
		else if(Lane == 2)	{ Position_Lane = 1.5f; } //faamorim { Position_Lane = 2.125f; }
		faamorim*/

		TotalLanes = CPP_C.Lanes;
		Position_Lane = (Lane-0.5f)/Mathf.Max(1,TotalLanes);
		Progress_Lane 		= Position_Lane;
		Fake_CCP1 				= new Vector2(0,0);
		Fake_CCP2 				= new Vector2(0,0);
		Vehicle_ID			= gameObject.GetInstanceID();
		
		Pos_Create 			= true;
		if(Max_Gear == 0) 			{ Max_Gear 	= 50.0f; }
		if(Max_Aceleration == 0) 	{ Max_Aceleration = 37.5f; }
		
		Slope	 			= 1;
		
		Reset_Values();
		Car_Incorrect_Track	= false;
		
		New_VPD();
		
		TimeContact			= 0.0f;
		
		StrWheel			= gameObject.transform.forward;
		}
	
	
	
		/* Adjust and corrections of the forces applied on the vehicle*/
	void Balance_of_forces()
		{
		
		if(Gear_Pressure > Max_Gear) 		{ Gear_Pressure = Max_Gear; }
		else if(Gear_Pressure < -Max_Gear) 	{ Gear_Pressure = -Max_Gear; }
		
		if(Gear_Pressure != 0.0f)
			{
			if(Gear_Pressure > 0.0f) 	{ Gear_Pressure -= Time.deltaTime * 54; }
			else 						{ Gear_Pressure += Time.deltaTime * 54; }
			
			StrWheel =	UTL.Euler_Rotation(StrWheel, 1, Gear_Pressure);
			}
		
		float Max_Vel = Max_Aceleration;
		
		if(Aceleration > Max_Vel) 
			{ 
			Aceleration -= Time.deltaTime * 9.5f;
			}
		else if(Aceleration < -Max_Vel/3.25f) 
			{ 
			Aceleration += Time.smoothDeltaTime * 1.5f;
			}
		
		
		if(Aceleration != 0)
			{
			if(Aceleration > 0.0f) 	{ Aceleration -= Time.deltaTime * 1.25f; }
			else 					{ Aceleration += Time.deltaTime * 1.25f; }
			}
		
		}
	
	
	
	
	
		/* Has the objective of aligning the vehicle with the right direction of the lane */
	void Angle_Sensor_Vehicle()	
		{
		float result = gameObject.transform.eulerAngles.y;
		
		float root = 180;
		if(Opposite_Direction) { root = 0; }//360
		
		result = Degrees + root;
		
		if(gameObject.transform.eulerAngles.y  != result)
			{
			
			result = UTL.Turning_Aside(gameObject.transform.eulerAngles.y, result);

			result = result/5 ; //faamorim result = result / 15.5f;
			
			if(Aceleration >= 0.025f)
				{
				gameObject.transform.Rotate(0,result,0);
				}
			
			if(Aceleration >= 0.0f) { Aceleration -= UTL.Natural_Number(result/100); }
			
			}
		}
	
	
	
	
	
		/* Adjust routine of the vehicle with the CCP's */
	void CCP_Conduct()
		{
		float 	T1 		= 0;
		float 	T2 		= 0;
		int 	next 	= Current_CCP+1;
		
		
			/* Investigate if the vehicle has passed the limits of the current CCP's */
		Vector2 nnorm = new Vector2(0,0);
		
		nnorm = UTL.Perpendicular_Vector(gameObject.transform.position,1);
		
		T1  = UTL.Distance(Fake_CCP1,nnorm);
		
		T2  = UTL.Distance(Fake_CCP2,nnorm);
		
		bool Position_Control = false;
		
		if(!Pos_Create)
		{
			if((T1 > Vector_Final_Size) && (!Opposite_Direction) )
			{
				Current_CCP++;
				if( (CPP_C.NextCCP.NextCCP != null) && (CPP_C.NextCCP != null) )
				{
					CPP_C = CPP_C.NextCCP;
					
					New_VPD();
				}
				else 								
				{
					Current_CCP --;
					
					SpawnSmoke();
					
					Destroy(gameObject);
				}
				Position_Control = true;
			}
			else if((T2 > Vector_Final_Size) && (Opposite_Direction) )
			{
				Current_CCP--;
				if( (CPP_C != null) && (CPP_C.PriorCCP != null) )
				{ 
					CPP_C = CPP_C.PriorCCP;
					
					New_VPD();
				}
				else 								
				{ 
					Current_CCP++;
					
					SpawnSmoke();
					
					Destroy(gameObject);
				}
				Position_Control = true;
			}
			
			if(CPP_C.Destroy_Direction1 || CPP_C.NextCCP.Destroy_Direction2 )
			{
				if(CPP_C.Destroy_Direction1 && !Opposite_Direction)
				{
					SpawnSmoke();
					Destroy(gameObject);
				}
				else if(CPP_C.NextCCP != null && CPP_C.NextCCP.Destroy_Direction2 && Opposite_Direction)
				{
					SpawnSmoke();
					Destroy(gameObject);
				}
			}
		}
		else { Pos_Create = false; }
		
			/* Calculate the perpendicular with the straight lane to define if the vehicle abanded the road */
		Vector2 Perpendicular = UTL.Perpendicular_Reta2d(Direction,false);
		
		CCP Prox	= CPP_C.NextCCP;
		
		if(Prox == null) { next--; Prox = CPP_C.PriorCCP; }
		
		Vector2 A1 = UTL.Perpendicular_Vector(CPP_C.transform.position,1);
		Vector2 A2 = UTL.Perpendicular_Vector(Prox.transform.position,1 );
		
		Vector2 B1 = UTL.Perpendicular_Vector(gameObject.transform.position,1);
		Vector2 B2 = UTL.Vector_Addition(B1, Perpendicular);
		
		Normal = UTL.Intersection_Point( A1, A2, B1, B2);
		
		if( float.IsNaN (Normal.x))
			{
			Normal.x = gameObject.transform.position.x;
			Normal.y = gameObject.transform.position.z;
			}
		
		if(!Position_Control)
			{
					/* Height average (Y) */
			float AltMedia 	= Prox.transform.position.y - CPP_C.transform.position.y;
			float distance 	= UTL.Distance(UTL.Perpendicular_Vector(CPP_C.transform.position,1), Normal);
			distance 		= ((distance * AltMedia)/Vector_Final_Size) + CPP_C.transform.position.y;
			
			Perpendicular 	= UTL.Adjust_Size(Perpendicular, CPP_C.Size_Road);
			
			Vector3 npg	 	= new Vector3(Perpendicular.x,	0,		Perpendicular.y);
			Vector3 trans	= new Vector3(Normal.x,			distance,	Normal.y);
		
			npg 			= UTL.Vector_Addition(npg,trans);
			Incorrect_Track = npg;
			
			Perpendicular 	= UTL.Adjust_Size(Perpendicular,-1);
		
			npg	 			= new Vector3(Perpendicular.x,	0,		Perpendicular.y);
			trans			= new Vector3(Normal.x,			distance,	Normal.y);
		
			npg 			= UTL.Vector_Addition(npg,trans);
			Correct_Track 	= npg;
			
			
			
				/* Verify if the vehicle is in the wrong direction of the lane */
			Vector3 CTrack = new Vector3(0,0,0);
		
			if(!Opposite_Direction)	{ CTrack = Correct_Track; }
			else   					{ CTrack = Incorrect_Track; }
			
			float dist_road 	= UTL.Distance(CTrack, trans); 
			float dis_vehicle	= UTL.Distance(CTrack, gameObject.transform.position);
				
				/* Tests if vehicle is in the wrong direction of the lane */
			if( dis_vehicle <= dist_road)	{ Car_Incorrect_Track = false; }
			else   							{ Car_Incorrect_Track = true;  }
			}
		}
	
		
	
		/* Changing lane function */
	void Changing_Lane()
		{
		if(Aceleration > 0.35f)
		{
			/*faamorim
			if( Lane == 1 ) 	{ Position_Lane = 0.85f;  }
			else if(Lane == 2)	{ Position_Lane = 2.125f; }
			faamorim*/
			TotalLanes = CPP_C.Lanes;
			Position_Lane = (Lane-0.5f)/Mathf.Max(1,TotalLanes);
			float difference = (Position_Lane - Progress_Lane) * (Time.deltaTime/2);
			Progress_Lane += difference;
			
			int proximo = Current_CCP + 1;
			CCP	Prox	= CPP_C.NextCCP;
			
			if(Prox == null) { proximo--; Prox = CPP_C.PriorCCP; }
		
			float Size_way = (Prox.Size_Road * Progress_Lane);
		
			Vector2 Perpendicular = new Vector2(0,0);
			
			if(!Opposite_Direction)
				{
				Perpendicular = UTL.Perpendicular_Reta2d(Direction,true);
				}
			else
				{
				Perpendicular = UTL.Perpendicular_Reta2d(Direction,false);
				}
			
			Perpendicular = UTL.Adjust_Size(Perpendicular, Size_way );
		
			Fake_CCP1 = UTL.Perpendicular_Vector(CPP_C.transform.position,1);
		
			Fake_CCP2 = UTL.Perpendicular_Vector(Prox.transform.position,1);
			
			Fake_CCP1 = UTL.Vector_Addition(Fake_CCP1, Perpendicular);
			Fake_CCP2 = UTL.Vector_Addition(Fake_CCP2, Perpendicular);
			}
		}

	
	void RollCar()
		{
		Vector3 rll = gameObject.transform.eulerAngles;
		
		if( ((rll.x > 60)&&(rll.x < 310)) || ((rll.z > 60)&&(rll.z < 310)))
			{
			Gear_Pressure 	= 0.0f;
			Aceleration		= 0.0f;
			TimeContact += Time.deltaTime;
			}
		
		if((rll.x > 10)&&(rll.x < 350))
			{
			if((rll.x > 360) && (rll.x <= 180))
				{
				Slope	= 1-((360 - rll.x) / 90 );
				
				if(Slope < 0.0f) { Slope = 0.0f; }
				}
			else
				{
				Slope	= 1;
				}
			}
		else
			{
			Slope = 1;
			}
		}
	
	// Update is called once per frame
	void Update () 
		{
		CCP_Conduct();
		
			{
			Balance_of_forces();
			Angle_Sensor_Vehicle();
			
			Vector3 result = StrWheel * (Aceleration * Scale * Slope)*Time.smoothDeltaTime;
			
			gameObject.transform.Translate(result);
			} 
				
		Changing_Lane();
		
		RollCar();
		
		if(SemapTime > 0.0f)
			{
			SemapTime -= Time.deltaTime;
			}
		
		if(TimeContact >= 1.5f)
			{
			SpawnSmoke();
			
			Destroy(this.gameObject);
			}
					
		}
	
		
		/* collision register (enter) */
	void OnCollisionEnter(Collision other)  
		{
		if(other.gameObject.tag == "AI")
			{
			TimeContact += Time.deltaTime;
			}
		}
	
		/* collision register (stay) */
	void OnCollisionStay(Collision other) 
		{
		if(other.gameObject.tag == "ET_AI")
			{
			TimeContact += Time.deltaTime;
			}
		}
	
	void OnCollisionExit(Collision other)
		{
		if( (other.gameObject.tag != "ET_AI") && (other.gameObject.tag != "AI") )
			{
			Gear_Pressure = 0.0f;
			}
		}
	
		/* Distance trigger of the Semaphore (enter) */
	void OnTriggerEnter(Collider other)
		{
		if(other.gameObject.tag == "ET_Semaphoro")
			{
			if( (other.gameObject.GetComponent<SemiUnit>().State == 2 ) && (SemapTime <= 0.0f) )
				{
				SemapTime 	= 0.0f;
				
				Aceleration /= 15;
				}
			else
				{
				SemapTime = 6.5f;
				}
			}
		}
	
		/* Distance trigger of the Semaphore (stay) */
	void OnTriggerStay(Collider other)
		{
		if(other.gameObject.tag == "ET_Semaphoro")
			{
			if( (other.gameObject.GetComponent<SemiUnit>().State == 2 ) && (SemapTime <= 0.0f) )
				{
				SemapTime 	= 0.0f;
				
				Aceleration /= 15;
				}
			else
				{
				SemapTime = 6.5f;
				}
			}
		}
	}