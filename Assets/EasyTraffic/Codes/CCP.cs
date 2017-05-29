using UnityEngine;
using System.Collections;

public class CCP : MonoBehaviour 
{
	
	public	TrafficEditorManage	MainTEM;
	
	bool			Verificado;			// Tag of verification of CCP
	
	public 	float	  		Size_Road;			// Regular size of the lane
	/// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
	
	public 	bool 	  		Spawn_Car;			// Creates a car here

	public	int				CCPID;

	public	bool			Spawn_Direction1;
	public	bool			Spawn_Direction2;
	public	bool			Destroy_Direction1;
	public	bool			Destroy_Direction2;

	public 	bool		  	Opposite_direct;	// The car goes to the opposite direct

	public 	int				MaxTimeSpawn;		// Max. time of vehicle replacement
	
	public	bool			Destroy_Car;		// Vehicle destruction CCP
	
	float  			TimeSpwan;			// Real time vehicle Replacement
	
	/// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// Opposite direct
	
	//Useful			UTL = new Useful();
	
	GameObject 		NVehicle; 			// Vehicle instance
	
	float 	  		Out_Way;			// Outside lane perimeter

	public	float			Scale;
	public	float			Size;
	public 	int 			Lanes;			// Number of lanes in the right direction
	
	public 	int 			NumberCCP;
	
	public 	Transform		Next;
	public 	CCP				NextCCP;
	public	float			NextAngle;
	public 	CCP				PriorCCP;
	public	float			PriorAngle;
	public	float			AngleBetween;


	
	// Get and Set Functions

	public void SetCCPID(int ID)
	{
		CCPID = ID;
	}

	public int GetCCPID()
	{
		return CCPID;
	}
	
	public void SetScale(float s)
	{
		Scale = s;
		Size_Road = Size * Scale;
	}
	
	public void SetLanes(int l)
	{
		Lanes = l;
	}

	public void SetSize(float s)
	{
		Size = s;
		Size_Road = Size * Scale;
	}


	// Vehicle placement function
	void Spawn_The_Car()
	{
		{
			int RandomCar = Random.Range(0,MainTEM.Car_Prefabs_Obj.Length);
			
			Vector3 NPos = gameObject.transform.position;
			
			NPos.y += 3.5F * Scale;
			
			NVehicle = Instantiate(MainTEM.Car_Prefabs_Obj[RandomCar], NPos, Quaternion.identity) as GameObject;

			if (Spawn_Direction2)
			{
				NVehicle.GetComponent<Vehicle_Control>().Opposite_Direction = true;
			}
			else
			{
				NVehicle.GetComponent<Vehicle_Control>().Opposite_Direction = false;
			}

			NVehicle.GetComponent<Vehicle_Control>().Current_CCP 	= NumberCCP;
			NVehicle.GetComponent<Vehicle_Control>().CPP_C			= this.GetComponent<CCP>();
			NVehicle.GetComponent<Vehicle_Control>().ChangingGears 	= true;
			NVehicle.GetComponent<Vehicle_Control>().Aceleration 	= 0.5f;
			
			int max	= (int) Mathf.Round(NVehicle.GetComponent<Vehicle_Control>().Max_Aceleration) + 8;
			int min = (int) Mathf.Round(NVehicle.GetComponent<Vehicle_Control>().Max_Aceleration) - 8;
			
			if(min <= 0) { min = 10; }
			
			max 	= Random.Range(min,max);
			
			NVehicle.GetComponent<Vehicle_Control>().Max_Aceleration = max;
			
			int Newlane = Random.Range(1,Lanes + 1);
			
			NVehicle.GetComponent<Vehicle_Control>().Lane = Newlane;

			MainTEM.GenerateChild("SpawnedCars");
			NVehicle.transform.parent = MainTEM.transform.Find("SpawnedCars");

			Vector3 ScaleV3 = new Vector3(Scale,Scale,Scale);
			NVehicle.transform.localScale = ScaleV3;
			NVehicle.GetComponent<Vehicle_Control>().Scale = Scale;
		}
	}
	
	
	
	// Use this for initialization
	void Start () 
	{
		//MainTEM = this.transform.parent.GetComponent<TrafficEditorManage>();

		TimeSpwan = 1;
		
		if(MaxTimeSpawn == 0) { MaxTimeSpawn = 2; }
		
		string mod = "";
		
		for(int i=0; i<gameObject.name.Length; i++)
		{
			if(i > 2)
			{
				mod = mod+gameObject.name[i];
			}
		}
		
		int md 		= int.Parse(mod);
		NumberCCP 	= md;
		
		
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		if(Spawn_Direction1 || Spawn_Direction2)
		{
			if(TimeSpwan <= 0.0f)
			{
				if(MainTEM.Car_Prefabs_Obj.Length>0)
				{
					Spawn_The_Car();
				}
				else
				{
					Debug.Log("There are no Cars on Traffic Editor Manager Prefabs list. Please add new Cars.");
				}

				TimeSpwan = Random.Range(MaxTimeSpawn,MaxTimeSpawn * 4);
			}
			else
			{
				TimeSpwan -= Time.deltaTime;
			}
		}
		
		
		if((Application.isPlaying) && (GetComponent<LineRenderer>()) )
		{
			Destroy(GetComponent("LineRenderer"));
		}
	}
	
	
	void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		{
			if(GetComponent<LineRenderer>())
			{
				if(Next != null)
				{
					GetComponent<LineRenderer>().SetPosition(0,gameObject.transform.position);
					GetComponent<LineRenderer>().SetPosition(1,Next.transform.position);
				}
			}
			else
			{
				if(!Destroy_Car)
				{
					string mod = "";
					
					for(int i=0; i<gameObject.name.Length; i++)
					{
						if(i > 2)
						{
							mod = mod+gameObject.name[i];
						}
					}
					
					int md 		= int.Parse(mod);
					NumberCCP 	= md;
					
					gameObject.AddComponent<LineRenderer>();
					
					int nxx 	= NumberCCP+1;
					
					if(GameObject.Find("PD0"+nxx))
					{
						Next 	= GameObject.Find("PD0"+nxx).transform;
						NextCCP	= Next.GetComponent<CCP>();
					}
				}
			}
			
			if(PriorCCP == null)
			{
				string mod = "";
				
				for(int i=0; i<gameObject.name.Length; i++)
				{
					if(i > 2)
					{
						mod = mod+gameObject.name[i];
					}
				}
				
				int md 		= int.Parse(mod);
				NumberCCP 	= md;
				
				int nxp 	= NumberCCP-1;
				
				if(GameObject.Find("PD0"+nxp))
				{
					PriorCCP = GameObject.Find("PD0"+nxp).GetComponent<CCP>();
				}
			}
			
			if(NextCCP == null)
			{
				string mod = "";
				
				for(int i=0; i<gameObject.name.Length; i++)
				{
					if(i > 2)
					{
						mod = mod+gameObject.name[i];
					}
				}
				
				int md 		= int.Parse(mod);
				NumberCCP 	= md;
				
				int nxx 	= NumberCCP+1;
				
				if(GameObject.Find("PD0"+nxx))
				{
					NextCCP	= GameObject.Find("PD0"+nxx).GetComponent<CCP>();
				}
			}
			
		}
		
	}
	
} 