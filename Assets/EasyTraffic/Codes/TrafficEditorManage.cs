using UnityEngine;
using System.Collections;
using System;
//using UnityEditor;


[ExecuteInEditMode]
public class TrafficEditorManage :  MonoBehaviour 
{


// ---------- Very Important Variables to see if the Plugin is already up to date
	public bool IsUpToDate = false;
	public bool IsVersion10926 = false;


	public bool FirstCCP;

	public Ray TEMRay;

	public Event CurrentEvent;

	private GameObject CarTarget;
	private bool IsCarTarget;

	
	private Vector3 WireBaseSize;
	private Vector3 WireBaseCenter;
	private Vector3 WireSize;
	private Vector3 WireCenter;
	private int WireStyle = 2;
	private float WirePosition = 2.0F;

	private float CarScale;

	private float MainScale = 1;
	private float MainRoadSize = 3;
	private int MainLanes = 2;


	public	bool		AddCPP;
	
	public	bool		AddSemaphore;
	
			bool		AddMesh;
	
	/// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
	
	public	int			Num_CCP;
	
	public	int			Count_CPPManager;
	
	public	string[]	Car_Prefabs;
	public	GameObject[] Car_Prefabs_Obj;
	
			int			Count_CPPs;
	
	public	int			Begin_Count;
	
	/// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// 
	
	public	int			Num_Mesh;
	
	public	int			Begin_Mesh;
	
	/// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///

// ---------- Important Functions to set the Plugin up to date
	public void SetUpToDate()
	{
		while(!IsUpToDate)
		{
			if(!IsVersion10926)
			{
				SetVersion10926();
			}
			else
			{
				IsUpToDate = true;
			}
		}
	}

	private void SetVersion10926()
	{
		GenerateChild("ControlPoints");
		GenerateChild("TrafficLights");
		GenerateChild("SpawnedCars");
		GenerateChild("Effects");
	
		Transform[] ChildCCP = this.GetComponentsInChildren<Transform>();

		for(int i=0; i<ChildCCP.Length; i++)
		{
			if(ChildCCP[i].gameObject.tag == "PD")
			{
				ChildCCP[i].GetComponent<CCP>().MainTEM = this;
				ChildCCP[i].parent = transform.Find("ControlPoints");
				ChildCCP[i].GetComponent<CCP>().Lanes = Mathf.Max(1,ChildCCP[i].GetComponent<CCP>().Lanes);
				if (ChildCCP[i].GetComponent<CCP>().Spawn_Car)
				{
					if (ChildCCP[i].GetComponent<CCP>().Opposite_direct)
					{
						ChildCCP[i].GetComponent<CCP>().Spawn_Direction1 = true;
					}
					else
					{
						ChildCCP[i].GetComponent<CCP>().Spawn_Direction2 = true;
					}
				}
				if (ChildCCP[i].GetComponent<CCP>().Destroy_Car)
				{
					ChildCCP[i].GetComponent<CCP>().Destroy_Direction1 = true;
					if (ChildCCP[i].GetComponent<CCP>().PriorCCP != false)
					{
						ChildCCP[i].GetComponent<CCP>().PriorCCP.Destroy_Direction2 = true;
					}
				}
			}
		}

		Array.Resize(ref Car_Prefabs_Obj, 0);

		if(Car_Prefabs_Obj.Length != Car_Prefabs.Length)
		{
			Array.Resize(ref Car_Prefabs_Obj, Car_Prefabs.Length);
			for(int t=0; t<Car_Prefabs.Length; t++)
			{
				Car_Prefabs_Obj[t] = Resources.Load(Car_Prefabs[t]) as GameObject;
			}
		}

		IsVersion10926 = true;
	}


	
// ---------- Get and Set functions
	
	public void SetAddCPP()
	{
		AddCPP = true;
		AddSemaphore = false;
	}
	
	public bool GetAddCPP()
	{
		return AddCPP;
	}
	
	public void SetAddSemaphore()
	{
		AddCPP = false;
		AddSemaphore = true;
	}
	
	public bool GetAddSemaphore()
	{
		return AddSemaphore;
	}

	public void SetFirstCCP()
	{
		FirstCCP = true;
	}
	
	public void SetNotFirstCCP()
	{
		FirstCCP = false;
	}

	public bool IsFirstCCP()
	{
		return FirstCCP;
	}

// ---------- Get and Set Functions to Add Car
	
	public void SetCarTargeted()
	{
		IsCarTarget = true;
	}

	public void SetCarNotTargeted()
	{
		IsCarTarget = false;
	}

	public void SetCarTarget(GameObject Target)
	{
		CarTarget = Target;
	}

	public void NonSenseFunction()
	{
		if(CarTarget != null)
		{
			IsCarTarget = !IsCarTarget;
			IsCarTarget = !IsCarTarget;
		}
	}
	

// ----------

	public void SetScale(float Scale)
	{
		MainScale = Scale;

		CCP[] ChildCCP = transform.GetComponentsInChildren<CCP>();
		for(int i=0; i<ChildCCP.Length; i++)
		{
			ChildCCP[i].SetScale(MainScale);
		}

		Vehicle_Control[] ChildCar = transform.GetComponentsInChildren<Vehicle_Control>();
		for(int i=0; i<ChildCar.Length; i++)
		{
			ChildCar[i].SetScale(MainScale);
		}

	}

	public float GetScale()
	{
		return MainScale;
	}

	public void SetRoadSize(float RoadSize)
	{
		MainRoadSize = RoadSize;
		CCP[] ChildCCP = transform.GetComponentsInChildren<CCP>();
		for(int i=0; i<ChildCCP.Length; i++)
		{
			ChildCCP[i].SetSize(MainRoadSize);
		}
	}

	public float GetRoadSize()
	{
		return MainRoadSize;
	}
	
	public void SetLanes(int Lanes)
	{
		MainLanes = Lanes;
		CCP[] ChildCCP = transform.GetComponentsInChildren<CCP>();
		for(int i=0; i<ChildCCP.Length; i++)
		{
			ChildCCP[i].SetLanes(MainLanes);
		}
	}

	public int GetLanes()
	{
		return MainLanes;
	}


// ---------- 

	public void GenerateChild(string Child)
	{
		if(!this.transform.Find(Child))
		{
			GameObject NewChild = new GameObject();
			NewChild.transform.position = this.gameObject.transform.position;
			NewChild.transform.parent = this.gameObject.transform;
			NewChild.name = Child;
		}
	}

	public	void Zero_CCP()
	{
		Count_CPPs			= 0;
	}
	
	public void Zero_All()
	{
		Count_CPPs			= 0;
		
		Count_CPPManager	= 0;
		
		Begin_Count			= 0;
		
		Num_CCP				= 0;
	}
	
	public void Add_Semaphore(Vector3 pos)
	{
		Instantiate(Resources.Load("Semaphores"),pos,Quaternion.identity);
	}
	
	public int Add_CPP()
	{
		Count_CPPs++;
		Count_CPPManager++;
		
		return Count_CPPManager;
	}
	
	public int Return_CountCPP()
	{
		return	Count_CPPs;
	}
	
	public int Return_CPPProjectors()
	{
		return	Num_CCP;
	}

/*	public	void Add_CarPrefab(GameObject Ref)
	{
		if(Car_Prefabs == null)
		{
			Car_Prefabs	= new string[0];
		}

		if(Car_Prefabs_Obj == null)
		{
			Car_Prefabs_Obj	= new GameObject[0];
		}
		
		Array.Resize(ref Car_Prefabs, Car_Prefabs.Length+1);
		
		string name = "ET_CAR"+Ref.name;
		
		Car_Prefabs[Car_Prefabs.Length-1] = name;
		
		PrefabUtility.CreatePrefab ("Assets/EasyTraffic/Models/Prefabs/resources/"+name+".prefab", Ref); 
		
		DestroyImmediate(Ref.gameObject);

		if(Car_Prefabs_Obj.Length != Car_Prefabs.Length)
		{
			Array.Resize(ref Car_Prefabs_Obj, Car_Prefabs.Length);
			for(int t=0; t<Car_Prefabs.Length; t++)
			{
				Car_Prefabs_Obj[t] = Resources.Load(Car_Prefabs[t]) as GameObject;
			}
		}
	}
	*/
	
	
	public	void Add_CCP()
	{
		Num_CCP++;
	}
	
	
	
	void	Seach_CCPPrior()
	{
		GameObject[] ccps = GameObject.FindGameObjectsWithTag("PD");
		
		string	num = "";
		
		for(int i=0; i<ccps.Length; i++)
		{
			for(int t=3; t<ccps[i].name.Length; t++)
			{
				num += ccps[i].name[t];
			}
			
			Begin_Count	= Convert.ToInt32(num);
			
			num = "";
		}
	}
	
// Use this for initialization
	void Start () 
	{
		AddCPP	= true;
		
		Seach_CCPPrior();
		
		Num_CCP = 0;
		
		if(Application.isPlaying)
		{
			GameObject[] bb = GameObject.FindGameObjectsWithTag("PD");
			
			for(int i=0; i<bb.Length; i++)
			{
				bb[i].GetComponent<ParticleEmitter>().enabled = false;
			}
		}
	}
	

	void Update()
	{
		if(WireSize.x != 0)
		{
			if(WirePosition != WireStyle)
			{
				if(Mathf.Abs(WirePosition - WireStyle) < 0.3)
				{
					WirePosition = WireStyle;
				}
				else
				{
					WirePosition -= 0.3F * (WirePosition - WireStyle)/Mathf.Abs(WirePosition - WireStyle);
				}
			}
		}
	}
	
	public void DrawBaseWire(Vector3 Center, Vector3 Size)
	{
		WireBaseCenter = Center;
		WireBaseSize = Size;
	}
	
	public void DrawWire(Vector3 Center, Vector3 Size, int Style)
	{
		WireCenter = Center;
		WireSize = Size;
		WireStyle = Style;
	}
	
	public void SetWirePosition(int Style)
	{
		WireStyle = Style;
	}

	public void ResetWirePosition()
	{
		WirePosition = WireStyle;
	}

	void OnDrawGizmos()
	{
		if(WireBaseSize.x !=0)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(WireBaseCenter, 1.01F * WireBaseSize);
		}

		if(WireSize.x !=0)
		{
			float BikeAlpha = 0.25F;
			float CarAlpha = 0.25F;
			float BusAlpha = 0.25F;

			float WireBikeX = WireCenter.x - (1.0F-WirePosition) * 2.5F * WireSize.x;
			float WireCarX = WireCenter.x - (2.0F-WirePosition) * 2.5F * WireSize.x;
			float WireBusX = WireCenter.x - (3.0F-WirePosition) * 2.5F * WireSize.x;
 
			if(WirePosition == 1) {BikeAlpha = 1;}
			if(WirePosition == 2) {CarAlpha = 1;}
			if(WirePosition == 3) {BusAlpha = 1;}

			// Draw the Bike
			Gizmos.color = Color.green - new Color(0,0,0,1-BikeAlpha);
			Gizmos.DrawWireCube
				(new Vector3
				 (WireBikeX, WireCenter.y - WireSize.y/2 + WireSize.x*0.35F, WireCenter.z),
				 new Vector3
				 (WireSize.x*0.2F, WireSize.x*0.25F, WireSize.x*1.3F)
				 );
			Gizmos.DrawWireCube
				(new Vector3
				 (WireBikeX, WireCenter.y - WireSize.y/2 + WireSize.x*0.625F, WireCenter.z + WireSize.x*0.3F),
				 new Vector3
				 (WireSize.x*0.2F, WireSize.x*0.3F, WireSize.x*0.35F)
				 );
			Gizmos.color = Color.blue - new Color(0,0,0,1-BikeAlpha);
			for(int i=0; i<2; i++)
			{
				Gizmos.DrawWireCube
					(new Vector3
					 (WireBikeX, WireCenter.y - WireSize.y/2 + WireSize.x*0.09F*2.0F, WireCenter.z +(0.4F * Mathf.Pow(-1,i)) * WireSize.x),
					 new Vector3
					 (WireSize.x*0.125F, WireSize.x*0.09F*4.0F, WireSize.x*0.09F*4.0F)
					 );
			}

			//Draw the Car
			Gizmos.color = Color.green - new Color(0,0,0,1-CarAlpha);
			Gizmos.DrawWireCube
				(new Vector3
				 (WireCarX, WireCenter.y - WireSize.y/2 + WireSize.x*0.09F*4.0F, WireCenter.z),
				 new Vector3
				 (WireSize.x*0.9F, WireSize.x*0.09F*4.5F, WireSize.x*2.4F)
				 );
			Gizmos.DrawWireCube
				(new Vector3
				 (WireCarX, WireCenter.y - WireSize.y/2 + WireSize.x*0.09F*8.25F, WireCenter.z - WireSize.x*0.09F*1.5F),
				 new Vector3
				 (WireSize.x*0.9F, WireSize.x*0.09F*4.0F, WireSize.x*1.5F)
				 );
			Gizmos.color = Color.blue - new Color(0,0,0,1-CarAlpha);
			for(int i=0; i<4; i++)
			{
				Gizmos.DrawWireCube
					(new Vector3
					 (WireCarX + (0.375F * Mathf.Pow(-1,i)) * WireSize.x, WireCenter.y - WireSize.y/2 + WireSize.x*0.09F*2.0F, WireCenter.z - WireSize.x*0.09F*0.5F +(0.75F * Mathf.Pow(-1,Mathf.Floor(i*0.5F))) * WireSize.x),
					 new Vector3
					 (WireSize.x*0.125F, WireSize.x*0.09F*4.0F, WireSize.x*0.09F*4.0F)
					 );
			}

			//Draw the Bus
			Gizmos.color = Color.green - new Color(0,0,0,1-BusAlpha);
			Gizmos.DrawWireCube
				(new Vector3
				 (WireBusX, WireCenter.y - WireSize.y/2 + WireSize.x*0.9F, WireCenter.z),
				 new Vector3
				 (WireSize.x*1.3F, WireSize.x*1.4F, WireSize.x*5.0F)
				 );
			Gizmos.color = Color.blue - new Color(0,0,0,1-BusAlpha);
			for(int i=0; i<4; i++)
			{
				Gizmos.DrawWireCube
					(new Vector3
					 (WireBusX + (0.525F * Mathf.Pow(-1,i)) * WireSize.x, WireCenter.y - WireSize.y/2 + WireSize.x*0.09F*3.5F, WireCenter.z - WireSize.x*0.09F*0.5F +(1.4F * Mathf.Pow(-1,Mathf.Floor(i*0.5F))) * WireSize.x),
					 new Vector3
					 (WireSize.x*0.2F, WireSize.x*0.09F*7.0F, WireSize.x*0.09F*7.0F)
					 );
			}
		}


		CurrentEvent = Event.current;
		Vector2 MousePos = CurrentEvent.mousePosition;
		MousePos.y = Camera.current.pixelHeight - MousePos.y;
		TEMRay = Camera.current.ScreenPointToRay(MousePos);
		
		/*
		Event Mouse = Event.current;
		
		if (CreatingCCP)
		{
			Selection.activeGameObject	= this.gameObject;
			
			if (Mouse.button == 0 && Mouse.isMouse && Mouse.type == EventType.MouseDown)
			{
				GameObject[] existPD 	= GameObject.FindGameObjectsWithTag("PD");
				
				GameObject[] existCCP	= GameObject.FindGameObjectsWithTag("ET_CCP");
				
				if( (existPD.Length <= 0) && (existCCP.Length <= 0) )
				{
					Zero_All();
				}
				
				Vector2 dir = Mouse.mousePosition;
				
				dir.y		= Camera.current.pixelHeight - dir.y;
				
				RaycastHit hit;
				Ray ray = Camera.current.ScreenPointToRay(dir);
				
				if (Physics.Raycast(ray, out hit,2700))
				{				
					if(AddCPP)
					{
						GameObject Int 			= 	Instantiate(Resources.Load("CCP_Projector2")) as GameObject;
						
						Undo.RegisterCreatedObjectUndo (Int,"Add CCP");
						
						Int.GetComponent<CCP_Projector2>().Starter( this.gameObject.GetComponent<TrafficEditorManage>(), hit.point);
						
						Int.gameObject.tag		= "ET_CCP";
						
						Int.transform.parent	=	gameObject.transform;
						
						
						Selection.activeGameObject = Int;
					}
					
					if(AddSemaphore)
					{
						GameObject Int 			= 	Instantiate(Resources.Load("Semaphores")) as GameObject;
						
						Undo.RegisterCreatedObjectUndo (Int,"Add Semaphore");
						
						Int.transform.position	= 	hit.point;
						
						Int.transform.parent	=	gameObject.transform;
						
						Selection.activeGameObject = Int;
					}
					
				}
				
			}
			Selection.activeGameObject	= this.gameObject;
		}*/
		
		if(Num_CCP > 0)
		{
			GameObject[] CCPs = GameObject.FindGameObjectsWithTag("ET_CCP");
			
			if(CCPs.Length != Num_CCP) 
			{
				Num_CCP = CCPs.Length; 
				
				if(CCPs.Length > 0)
				{
					GameObject ccp = GameObject.Find("CCP_Projector"+(CCPs.Length-1));
					
					ccp.GetComponent<CCP_Projector2>().ChangeFirst(false);
					
					ccp.GetComponent<CCP_Projector2>().Dead_end = true;
					
					DestroyImmediate(ccp.GetComponentInChildren<LineCCP2>().gameObject);
				}
			}
			
		}
		
		if(Num_Mesh > 0)
		{
			GameObject[] Meshs = GameObject.FindGameObjectsWithTag("ET_Mesh");
			
			if(Meshs.Length != Num_Mesh) 
			{
				Num_Mesh = Meshs.Length; 
			}
		}
		
	}
}