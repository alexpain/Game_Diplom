using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

public class EasyTraffic : EditorWindow
{
//	private ETTutorial Tutorial;

	private string ETversion = "V. 1.11.30";
	
	private Event CurrentEvent;
	
	public	TrafficEditorManage MainTEM;
	public	bool		TEMSelected;

	private	Vector2		ScrollPosition = Vector2.zero;

	private	bool		Begin = true;
	private	bool		Started;
	private	bool		Error;
	private	int			CheckPrefabsCooldown = 0;
	private	int			DemoTutorialCooldown = 0;

	private	int			AddCars;
	private	int			ManageCCP;
	private bool		CreatingCCP;
	
	private int			Lanes;
	private float		SizeRoad;
	private float		Scale;
	private int			LanesNew;
	private float		SizeRoadNew;
	private float		ScaleNew;
	private float		ScaleFake;
	private string		ScaleString;

	private Semaphore[]	TrafficLights;

	private GameObject	LastTarget;
	private Quaternion	LastRotation;
	private Vector3		LastScale;
	private Vector3		LastPosition;
	private bool		ResetRotation = true;
	private bool		OldResetRotation = true;
	private bool		OldManual = false;
	private	bool		Auto = true;
	private	bool		Manual = false;
	private bool		DrawWire = false;
	private	int			ScaleAuto = 2;
	private int			OldScaleAuto = 0;
	private Bounds		BaseBounds;
	private float		CarScale;

	private	int			TrafficSettings;
	private	bool		Extras;
	private	bool		ToggleAdvanced = false;
	private	bool		WarningAdvanced;
	
	private	static		Texture2D tex = new Texture2D (1, 1, TextureFormat.RGBA32, false);
	static	string		Title;
	static	string		TitleBold;
	static	GUIStyle	ButtonStyle;
	static	GUIStyle	TextStyle;


// ---------- This Adds the Easy Traffic item on the Window menu 
	[MenuItem ("Window/Easy Traffic")]



// ---------- Here are the Initialization Functions for the Plugin Window and Traffic Editor Manager
	static	void Easy_Traffic()
	{
		EasyTraffic window 	= (EasyTraffic)EditorWindow.GetWindow (typeof (EasyTraffic));
		window.title 		= "Easy Traffic";
		window.minSize		= new Vector2(200,500);
		window.maxSize		= new Vector2(200,500);
		window.Show();
	}

	void	Initiate()
	{
		tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		tex.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.85f));

		tex.Apply();

		Begin = false;
		Started = false;
		AddCars = 0;
		ManageCCP = 0;
		ToggleAdvanced = false;
		WarningAdvanced = false;
	}

	void	Start()
	{
		StartET(0,0);
	}

	void	StartET(int StartCars, int StartCCP)
	{
		Begin = false;
		Started = false;
		Manual = false;
		AddCars = StartCars;
		ManageCCP = StartCCP;
		ToggleAdvanced = false;
		WarningAdvanced = false;

		GameObject ET_CCP = Instantiate(Resources.Load("CCP_Projector2")) as GameObject;
		
		GameObject ET_LINE	= Instantiate(Resources.Load("LineCCP0")) as GameObject;
		
		GameObject ET_TEMStart	= Instantiate(Resources.Load("TrafficEditorManager")) as GameObject;
		
		GameObject PD0	= Instantiate(Resources.Load("PD0")) as GameObject;
		
		GameObject ET_TL	= Instantiate(Resources.Load("Semaphores")) as GameObject;
		
		DestroyImmediate(ET_CCP.gameObject);
		
		DestroyImmediate(ET_LINE.gameObject);
		
		DestroyImmediate(ET_TEMStart.gameObject);
		
		DestroyImmediate(PD0.gameObject);
		
		DestroyImmediate(ET_TL.gameObject);
		
		if(!GameObject.FindGameObjectWithTag("ET_TEM"))
		{
			GameObject ET_TEM	= Instantiate(Resources.Load("TrafficEditorManager")) as GameObject;
			ET_TEM.name = "TrafficEditorManager";
			ET_TEM.transform.position = new Vector3(0,0,0);
			EditorUtility.DisplayDialog("Easy Traffic Started!", "You now just created a new object called TrafficEditorManager, do not delete it or it will lose all your current settings.", "Close");
		}
		
		MainTEM = GameObject.FindGameObjectWithTag("ET_TEM").GetComponent<TrafficEditorManage>();

		MainTEM.GenerateChild("ControlPoints");
		MainTEM.GenerateChild("TrafficLights");
		MainTEM.GenerateChild("SpawnedCars");
		MainTEM.GenerateChild("Effects");

		MainTEM.SetUpToDate();


		Scale = GetTEMScale();
		ScaleNew = Scale;
		SizeRoad = GetTEMRoadSize();
		SizeRoadNew = SizeRoad;
		Lanes = GetTEMLanes();
		LanesNew = Lanes;
		
		if (Scale >= 1)
		{
			ScaleFake = (Scale - 1) * 10;
		}
		
		if (Scale < 1)
		{
			ScaleFake = (Scale - 1) * 100;
		}


		Started = true;
	}


// ---------- Here Starts the Functions used by this Plugin on the Unity Editor

	private Bounds GetTotalBounds(GameObject Target, bool BaseRot)
	{
		Quaternion OldRot = Target.transform.rotation;
		if(BaseRot) Target.transform.rotation = Quaternion.identity;

		bool hasBounds = false;
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

		if (Target.GetComponent<Renderer>())
		{
			bounds = Target.GetComponent<Renderer>().bounds;
			hasBounds = true;
		}

		foreach (Renderer childRenderer in Target.GetComponentsInChildren<Renderer>())
		{
			if (childRenderer != null)
			{
				if (hasBounds)
				{
					bounds.Encapsulate(childRenderer.bounds);
				}
				else
				{
					bounds = childRenderer.bounds;
					hasBounds = true;
				}
			}
		}

		Target.transform.rotation = OldRot;
		return bounds;
	}

	private void SetScaleStyle()
	{
		if(OldManual != Manual)
		{
			OldManual = Manual;
			if (Manual)
			{
				SetDrawWire(Selection.activeGameObject);
			}
		}

		if (Manual)
		{
			if (ScaleAuto != OldScaleAuto)
			{
				OldScaleAuto = ScaleAuto;
				MainTEM.SetWirePosition(ScaleAuto);
			}
		}
		else
		{
			OldScaleAuto = 0;
			ScaleAuto = 2;
		}
	}

	private void SetDrawWire(GameObject Target)
	{
		BaseBounds = GetTotalBounds (Target, ResetRotation);
		MainTEM.DrawWire(BaseBounds.center, BaseBounds.size, ScaleAuto);
		MainTEM.DrawBaseWire(BaseBounds.center, BaseBounds.size);
		DrawWire = true;
	}

	private void ResetDrawWire()
	{
		if(MainTEM && DrawWire)
		{
			MainTEM.DrawWire(Vector3.zero, Vector3.zero, ScaleAuto);
			MainTEM.DrawBaseWire(Vector3.zero, Vector3.zero);
			MainTEM.ResetWirePosition();
			DrawWire = false;
		}
	}

	private void TargetUpdate()
	{
		TargetUpdate (null);
	}

	private void TargetUpdate(GameObject Target)
	{
		if(LastTarget)
		{
			LastTarget.transform.rotation = LastRotation;
			LastTarget.transform.position = LastPosition;
			LastTarget.transform.localScale = LastScale;
			LastTarget = null;
		}
		if(Target)
		{
			LastTarget = Target;
			LastRotation = LastTarget.transform.rotation;
			LastPosition = LastTarget.transform.position;
			LastScale = LastTarget.transform.localScale;
		}
	}

	private void SetCarScale(GameObject Car, BoxCollider Box)
	{
		Bounds CarBounds = GetTotalBounds(Car, ResetRotation);
		
		if (Auto)
		{
			float ScaleX = Box.size.x / CarBounds.size.x;
			Car.transform.localScale *= ScaleX;
			CarBounds.size *= ScaleX;
			if(CarBounds.size.z > Box.size.z)
			{
				float ScaleZ = ((CarBounds.size.z / Box.size.z)-1.0F)/1.3F + 1.0F;
				Car.transform.localScale *= ScaleZ;
				CarBounds.size *= ScaleZ;
			}
			if(CarBounds.size.y > 2.5 * Box.size.y)
			{
				float ScaleY = ((CarBounds.size.y / Box.size.y)-2.5F)/1.3F + 1.0F;
				Car.transform.localScale *= ScaleY;
				CarBounds.size *= ScaleY;
			}
		}
		else if (Manual)
		{
			float BaseScale = Box.size.x / BaseBounds.size.x;
			Car.transform.localScale *= BaseScale;
		}

		CarBounds = GetTotalBounds(Car, ResetRotation);

		if(CarBounds.size.y > 1.25 * CarBounds.size.x)
		{
			CarBounds.size = new Vector3 (CarBounds.size.x, CarBounds.size.x, CarBounds.size.z);
		}

		Box.size = CarBounds.size;

		Car.transform.position += Car.transform.position - CarBounds.center;
		Car.transform.position += new Vector3(0, (GetTotalBounds(Car, ResetRotation).size.y - CarBounds.size.y)/2, 0);
	}


	private void Check_Car_Prefabs(int Action)
	{
		if(MainTEM.Car_Prefabs.Length != MainTEM.Car_Prefabs_Obj.Length)
		{
			if (Action == 1)
			{
			}
			if (Action > 1)
			{
				int MaxLength = Mathf.Max (MainTEM.Car_Prefabs.Length,MainTEM.Car_Prefabs_Obj.Length);
				Array.Resize(ref MainTEM.Car_Prefabs, MaxLength);
				Array.Resize(ref MainTEM.Car_Prefabs_Obj, MaxLength);
			}
		}

		if (Action == 5)
		{
			for(int t=0; t<MainTEM.Car_Prefabs.Length; t++)
			{
				string name = MainTEM.Car_Prefabs[t];
				if(MainTEM.Car_Prefabs_Obj[t])
				{
					name = MainTEM.Car_Prefabs_Obj[t].name;
					name = name.Remove (0,6);

					MainTEM.Car_Prefabs[t] = name;
				}
				else if (!Recover_Car_Prefab(name, t))
				{
					for(int i=t; i<MainTEM.Car_Prefabs_Obj.Length - 1; i++)
					{
						MainTEM.Car_Prefabs_Obj[i] = MainTEM.Car_Prefabs_Obj[i+1];
						MainTEM.Car_Prefabs[i] = MainTEM.Car_Prefabs[i+1];
					}
					Array.Resize(ref MainTEM.Car_Prefabs_Obj, MainTEM.Car_Prefabs_Obj.Length - 1);
					Array.Resize(ref MainTEM.Car_Prefabs, MainTEM.Car_Prefabs.Length - 1);
					t--;
				}
			}
		}
	}

	private bool Recover_Car_Prefab(string PrefabName, int PrefabIndex)
	{
		bool WillReturn = false;
		bool Dialog = true;
		//int tries = 1;
		while (Dialog)
		{
			/*faamorim
			if(ToggleAdvanced)
			{
				if(EditorUtility.DisplayDialog(
					"Missing Prefab!",
					"The prefab named \"" + PrefabName + "\" could not be found in the prefabs folder.\n" +
					"It may be at your recycle bin or trash folder under the name \"ET_CAR" + PrefabName + ".prefab\".\n" +
					"You can try to move it to the project folder at \"Assets\\EasyTraffic\\Models\\Prefabs\\Resources\" and after that press \"Retry\". Or hit \"Cancel\" and import and transform into prefab again.",
					"Retry (" + tries + ")",
					"Cancel"))
				{
					tries++;
					if(MainTEM.Car_Prefabs_Obj[PrefabIndex])
					{
						if(Resources.Load(MainTEM.Car_Prefabs_Obj[PrefabIndex].name))
						{
							Dialog = false;
							WillReturn = true;
							tries--;
						}
					}
				}
				else
				{
					Dialog = false;
					WillReturn = false;
				}
			}
			else
			faamorim*/
			{
				EditorUtility.DisplayDialog(
					"Missing Prefab!",
					"The prefab named \"" + PrefabName + "\" could not be found in the prefabs folder.\n" +
					"You may have to add this prefab again.",
					"OK");
				Dialog = false;
				WillReturn = false;
			}
		}
		return WillReturn;
	}


	
	private void Add_Car_Prefab(GameObject Target)
	{
		Check_Car_Prefabs(5);

		if(Auto)
		{
			Target.transform.rotation = Quaternion.identity;
		}
		Target.transform.position = Vector3.zero;

		//GameObject NewCarPrefab = Instantiate(Resources.Load("CarPrefab"),GetTotalBounds (Target, ResetRotation).center, Quaternion.identity) as GameObject;
		GameObject NewCarPrefab = Instantiate(Resources.Load("CarPrefab"), Vector3.zero, Quaternion.identity) as GameObject;


		SetCarScale (Target, NewCarPrefab.GetComponent<BoxCollider>());

		Target.transform.parent = NewCarPrefab.transform;
		
		NewCarPrefab.name = Target.name;


		if(MainTEM.Car_Prefabs == null)
		{
			MainTEM.Car_Prefabs	= new string[0];
		}
		
		if(MainTEM.Car_Prefabs_Obj == null)
		{
			MainTEM.Car_Prefabs_Obj	= new GameObject[0];
		}
		
		Array.Resize(ref MainTEM.Car_Prefabs, MainTEM.Car_Prefabs.Length+1);
		Array.Resize(ref MainTEM.Car_Prefabs_Obj, MainTEM.Car_Prefabs_Obj.Length+1);

		string name = NewCarPrefab.name;
		string prename = name;
		int namenewindex = 1;
		for(int t=0; t<MainTEM.Car_Prefabs.Length; t++)
		{
			if(MainTEM.Car_Prefabs[t] == name)
			{
				namenewindex++;
				name = prename + namenewindex;
			}
		}

		string prefabname = "ET_Car"+name;

		MainTEM.Car_Prefabs[MainTEM.Car_Prefabs.Length-1] = name;
		
		PrefabUtility.CreatePrefab ("Assets/EasyTraffic/Models/Prefabs/resources/"+prefabname+".prefab", NewCarPrefab); 
		
		DestroyImmediate(NewCarPrefab.gameObject);

		MainTEM.Car_Prefabs_Obj[MainTEM.Car_Prefabs_Obj.Length-1] = Resources.Load(prefabname) as GameObject;

		Check_Car_Prefabs(2);
	}


	private void Create_CCP()
	{
		GameObject[] existPD 	= GameObject.FindGameObjectsWithTag("PD");
		
		GameObject[] existCCP	= GameObject.FindGameObjectsWithTag("ET_CCP");
		
		if( (existPD.Length <= 0) && (existCCP.Length <= 0) )
		{
			MainTEM.Zero_All();
		}

		RaycastHit hit;

		Ray ray = MainTEM.TEMRay;
		
		if (Physics.Raycast(ray, out hit,2700))
		{
			MainTEM.GenerateChild("NewControlPoints");

			if(MainTEM.GetAddCPP())
			{
				GameObject Int 			= 	Instantiate(Resources.Load("CCP_Projector2")) as GameObject;
				
				Undo.RegisterCreatedObjectUndo (Int,"Add CCP");
				
				Int.GetComponent<CCP_Projector2>().Starter( MainTEM.gameObject.GetComponent<TrafficEditorManage>(), hit.point);
				
				Int.gameObject.tag		= "ET_CCP";
				
				Int.transform.parent	=	MainTEM.gameObject.transform.Find("NewControlPoints");

				Selection.activeGameObject = Int;
			}
			
			if(MainTEM.GetAddSemaphore())
			{
				GameObject Int 			= 	Instantiate(Resources.Load("Semaphores")) as GameObject;
				
				Undo.RegisterCreatedObjectUndo (Int,"Add Semaphore");
				
				Int.transform.position	= 	hit.point;

				Int.transform.localScale		=	new Vector3(Scale,Scale,Scale);
				
				Int.transform.parent	=	MainTEM.gameObject.transform.Find("TrafficLights");
				
				Selection.activeGameObject = Int;
			}
		}
	}

	private void DoneCreatingCCP()
	{
		if(!MainTEM.transform.Find("NewControlPoints"))
		{
			EditorUtility.DisplayDialog("No Points Created", "You have not created any points.\nPlease create at least 2 points to transform into a road.", "Close");
		}
		else
		{
			CCP_Projector2[] obj = MainTEM.transform.Find("NewControlPoints").GetComponentsInChildren<CCP_Projector2>();
			if (obj.Length >= 2)
			{
				CreateCCP();
			}
			else if (obj.Length == 1)
			{
				EditorUtility.DisplayDialog("Only One Point", "You have created only one point.\nWe need at least 2 points to create a road.\nPlease create at least 1 more point to transform into a road.", "Close");
			}
			else
			{
				EditorUtility.DisplayDialog("No Points Created", "You have not created any points.\nPlease create at least 2 points to transform into a road.", "Close");
			}
		}
	}

	private void CreateCCP()
	{
		MainTEM.SetFirstCCP();
		
		MainTEM.Zero_CCP();
		
		GameObject[] 	CCPP	= GameObject.FindGameObjectsWithTag("ET_CCP");
		
		for(int i=0; i<CCPP.Length; i++)
		{
			CCPP[i].GetComponent<CCP_Projector2>().Adjust_Semaphore();
		}
		
		for(int i=0; i<CCPP.Length; i++)
		{
			CCPP[i].GetComponent<CCP_Projector2>().Create_RealCPPs();
			MainTEM.SetNotFirstCCP();
		}
		
		Transform[] obj = MainTEM.transform.Find("NewControlPoints").GetComponentsInChildren<Transform>();
		
		GameObject	End	= new GameObject();
		
		for(int i=1; i<obj.Length; i++)
		{
			if(obj[i].gameObject.tag != "PD")
			{
				obj[i].parent = End.transform;
			}
		}
		
		DestroyImmediate(End.gameObject);
		
		MainTEM.Zero_CCP();
		
		MainTEM.Num_CCP	= 0;
		
		CreatingCCP = false;
		
		DestroyImmediate(MainTEM.transform.Find("NewControlPoints").gameObject);
		
		ManageCCP = 1;
	}
	
	private void SetTEMScale(float s)
	{
		MainTEM.SetScale(s);
	}

	private float GetTEMScale()
	{
		return MainTEM.GetScale();
	}

	private void SetTEMLanes(int l)
	{
		MainTEM.SetLanes (l);
	}

	private int GetTEMLanes()
	{
		return MainTEM.GetLanes();
	}

	private void SetTEMRoadSize(float s)
	{
		MainTEM.SetRoadSize(s);
	}

	private float GetTEMRoadSize()
	{
		return MainTEM.GetRoadSize();
	}

	
// ---------- Here Starts the GUI
	void	OnGUI()
	{
		if(Begin) {Initiate ();}
		
// ---------- Here Starts the Window Screen 
		ScrollPosition = GUI.BeginScrollView(new Rect(0,0,Screen.width,Screen.height-22), ScrollPosition, new Rect(0,0,200,500));


// ---------- Window Background 
		tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		tex.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.25f));
		tex.Apply();
		GUI.DrawTexture(new Rect(0, 0, Math.Max(Screen.width,200), Math.Max(Screen.height,500)), tex, ScaleMode.StretchToFill);


// ---------- Setting Button Styles 
		ButtonStyle = GUI.skin.GetStyle("button");
		TextStyle = GUI.skin.GetStyle("label");
		ButtonStyle.alignment = TextAnchor.MiddleCenter;
		ButtonStyle.fontSize = 14;


// ---------- Setting Bold Style 
		TextStyle.fontSize = 14;
		TextStyle.fontStyle = FontStyle.Bold;
		TextStyle.alignment = TextAnchor.UpperCenter;


// ---------- Printing the Bold Label 
		GUI.Label(new Rect(10, 10, Math.Max(Screen.width,200) - 20, 115),TitleBold, TextStyle);


// ---------- Setting Normal Style 
		TextStyle.fontSize = 14;
		TextStyle.fontStyle = FontStyle.Normal;
		TextStyle.alignment = TextAnchor.UpperCenter;


// ---------- Printing the Normal Label 
		GUI.Label(new Rect(10, 10, Math.Max(Screen.width,200) - 20, 115),Title, TextStyle);


// ---------- Here Starts the Main Window Area 
		GUILayout.BeginArea (new Rect(10, 135, Math.Max(Screen.width,200) - 20, 240));


// ---------- Initial Screen 
		if(!Started)
		{
			Title = "Welcome to the\n\n\nClick Start to begin\nor Click \"Video Tutorial\"\nto watch the Tutorial";
			TitleBold = "\nEasy Traffic\nUnity Plugin";

			if( GUILayout.Button("Start", ButtonStyle))
			{
				StartET(0,0);
			}
			
		}

		if(Started)
		{
// ---------- Main Screen 
			if(AddCars <= 0 && ManageCCP <= 0 && TrafficSettings <= 0 && Extras == false)
			{
				TitleBold = "Started";
				Title = "";

				if( GUILayout.Button("Manage Cars"))
				{
					AddCars = 1;
				}
				
				EditorGUILayout.Space();
				
				if( GUILayout.Button("Manage Roads"))
				{
					ManageCCP = 1;
				}

				EditorGUILayout.Space();

				ScaleString = Scale.ToString();
				if(ScaleString.Contains("."))
				{
					if(ScaleString.IndexOf(".") == ScaleString.Length - 2)
					{
						ScaleString = ScaleString + "0";
					}
				}
				else
				{
					ScaleString = ScaleString + ".00";
				}

				GUI.Label(new Rect(5,85, Math.Max(Screen.width , 200) - 30, 20),"Scale: " + ScaleString);

				ScaleFake = (int) GUI.HorizontalSlider(new Rect(10, 105, Math.Max(Screen.width , 200) - 40, 10), ScaleFake, -90, 90);

				if (ScaleFake >= 0)
				{
					ScaleNew = ScaleFake/10 + 1;
				}
				else
				{
					ScaleNew = 1 + ScaleFake/100;
				}

				if(ScaleNew != Scale)
				{
					Scale = ScaleNew;
					SetTEMScale(Scale);
				}
			}


// ---------- Manage Cars Screen (Manage Cars)
			if(AddCars == 1)
			{
				TitleBold = "Manage Cars";
				Title = "\nHere you can add cars\nto your prefabs list";

				if(ToggleAdvanced)
				{
					//Title += ",\nremove cars from\nyour list, and manage\nyour cars preferences";
				}

				Title +=".";

				if(GUILayout.Button("Add Cars"))
				{
					AddCars = 2;
				}
			}


// ---------- Add Cars to Prefabs List Screen (Manage Cars / Add Cars)
			if(AddCars == 2)
			{
				TitleBold = "Add Cars";
				Title = "\nYou are now adding\ncars to the Prefabs List.";

				if(!Selection.activeGameObject)
				{
					Title += "\n\nSelect an Object\nto add as a Car.\n\n";
					
					if(ToggleAdvanced)
					{
						OldManual = Manual;
						
						GUILayout.Label("Choose the Scale\nto be used.");
						
						GUILayout.BeginHorizontal();
						ButtonStyle.fontSize = 10;
						
						Auto = GUILayout.Toggle(Auto, "Auto", "Button");
						
						if(Auto){Manual = false;}
						
						Manual = GUILayout.Toggle(Manual, "Manual", "Button");
						
						if(Manual){Auto = false;}
						if (!Auto && !Manual){Auto = true;}
					}
				}
				else if(Selection.activeGameObject.GetComponent<Renderer>() || Selection.activeGameObject.transform.GetComponentInChildren<Renderer>() && Selection.activeGameObject.tag != "ET_TEM")
				{
					Title += "\nDo you want to add\n\nas a Car to the\nPrefabs List?";
					TitleBold += "\n\n\n\n" + Selection.activeGameObject.name;

					if(GUILayout.Button("Center View on\n" + Selection.activeGameObject.name))
					{
						SceneView.lastActiveSceneView.FrameSelected();
					}
					
					if(ToggleAdvanced)
					{
						OldManual = Manual;

						GUILayout.Label("Choose the Scale\nto be used.");

						GUILayout.BeginHorizontal();
						ButtonStyle.fontSize = 10;

						Auto = GUILayout.Toggle(Auto, "Auto", "Button");

						if(Auto){Manual = false;}

						Manual = GUILayout.Toggle(Manual, "Manual", "Button");

						if(Manual){Auto = false;}
						if (!Auto && !Manual){Auto = true;}

						ButtonStyle.fontSize = 14;
						GUILayout.EndHorizontal();
						
						SetScaleStyle();
						
						if(Auto)
						{
							GUILayout.Label ("The scale will be\nchosen automatically.");
							ResetDrawWire ();
							ResetRotation = GUILayout.Toggle (ResetRotation, "Reset Object Rotation");
						}

						if(Manual)
						{
							GUILayout.Label ("Chose the best\nstyle for the scale.");
							GUILayout.BeginHorizontal();
							ButtonStyle.fontSize = 10;
							if(ScaleAuto>1)
							{
								if(GUILayout.Button("<")){ScaleAuto--;}
							}
							else {GUILayout.Button(" ");}

							string ScaleName = "";

							if(ScaleAuto == 1) {ScaleName = "Bike";}
							if(ScaleAuto == 2) {ScaleName = "Car";}
							if(ScaleAuto == 3) {ScaleName = "Bus";}

							GUILayout.TextArea(ScaleName,TextStyle);

							if(ScaleAuto<3)
							{
								if(GUILayout.Button(">")){ScaleAuto++;}
							}
							else {GUILayout.Button(" ");}
							GUILayout.EndHorizontal();
							ButtonStyle.fontSize = 14;
							ResetRotation = GUILayout.Toggle (ResetRotation, "Reset Object Rotation");
							if(ResetRotation && !OldResetRotation)
							{
								Selection.activeGameObject.transform.rotation = Quaternion.identity;
								SetDrawWire(Selection.activeGameObject);
								OldResetRotation = true;
							}
						}
					}
					else
					{
						Manual = false;
						Auto = true;
						ResetRotation = true;
						OldResetRotation = false;
					}


					if(GUILayout.Button("Add\n" + Selection.activeGameObject.name + "\nas Car"))
					{
						if(GameObject.FindGameObjectWithTag("ET_TEM"))
						{
							if(EditorUtility.DisplayDialog("Add " + Selection.activeGameObject.name + " as car?", "Do you really want to add the GameObject \n\"" + Selection.activeGameObject.name + "\" to your cars list", "Yes", "No"))
							{
								Add_Car_Prefab(Selection.activeGameObject);
							}
						}
					}
				}
				else
				{
					Title += "\n\nInvalid Object.\nSelect a valid object.";
					
					if(ToggleAdvanced)
					{
						OldManual = Manual;
						
						GUILayout.Label("Choose the Scale\nto be used.");
						
						GUILayout.BeginHorizontal();
						ButtonStyle.fontSize = 10;
						
						Auto = GUILayout.Toggle(Auto, "Auto", "Button");
						
						if(Auto){Manual = false;}
						
						Manual = GUILayout.Toggle(Manual, "Manual", "Button");
						
						if(Manual){Auto = false;}
						if (!Auto && !Manual){Auto = true;}
					}
				}
			}
			
			
			if(ManageCCP >= 1 && ManageCCP != 5)
			{
				MainTEM.SetAddCPP();
			}
// ---------- Manage Roads Screen (Manage Roads)
			if(ManageCCP == 1)
			{
				TitleBold = "Manage Roads";
				Title = "\nHere you can create";
				
				if(ToggleAdvanced)
				{
					//Title += ",\nedit and manage";
				}
				
				Title +="\n your Roads";
				
				if(GUILayout.Button("Create New Road"))
				{
					ManageCCP = 2;
				}

				GUILayout.Space (120);

				GUI.Label(new Rect(5,35, Math.Max(Screen.width , 200) - 30, 20),"Number of car lanes: " + Lanes);
				
				LanesNew = (int) Mathf.Round(GUI.HorizontalSlider(new Rect(10, 55, Math.Max(Screen.width , 200) - 40, 10), LanesNew, 1, 2));

				
				GUI.Label(new Rect(5,85, Math.Max(Screen.width , 200) - 30, 20),"Road Size: " + SizeRoad);
				
				SizeRoadNew = Mathf.Round(GUI.HorizontalSlider(new Rect(10, 105, Math.Max(Screen.width , 200) - 40, 10), SizeRoadNew, 1, 5) * 10)/10;


				if(SizeRoadNew != SizeRoad || LanesNew != Lanes)
				{
					Lanes = LanesNew;
					SizeRoad = SizeRoadNew;
					SetTEMLanes(Lanes);
					SetTEMRoadSize(SizeRoad);
				}

				
				if(GUILayout.Button("Add Traffic Light"))
				{
					ManageCCP = 5;
				}
			}
			
// ---------- Manage Control Points Screen (Manage Roads / Create New Road)
			if(ManageCCP == 2)
			{
				TitleBold = "Create New Road";
				Title = "\nHere you can create";
				
				if(ToggleAdvanced)
				{
					//Title += ",\nedit and manage";
				}
				
				Title +="\n Control Points\nfor your Road.";
				
				if(GUILayout.Button("Add New\nControl Points"))
				{
					ManageCCP = 3;
					CreatingCCP = true;
					EditorUtility.DisplayDialog("Attention.", "You are now Creating new Control Points.\nNow click on the Scene view to add points to your new road.\nClick \"Done\" when you are done or click \"Pause\" to stop adding Control Points without finishing your road", "Close");
				}
			}
			
			
// ---------- Create Control Points Screen (Manage Roads / Create New Road / Add Control Points)
			if(ManageCCP == 3)
			{
				TitleBold = "Creating New Points";
				Title = "\nYou are now creating\nnew Control Points\nto create a new road.";

				if(GUILayout.Button("Done Creating\nControl Points"))
				{
					DoneCreatingCCP();
				}

				if(GUILayout.Button("Pause Creating\nControl Points"))
				{
					ManageCCP = 4;
						
					CreatingCCP = false;
				}
				
				if(GUILayout.Button("Cancel Creating\nControl Points"))
				{
					MainTEM.Zero_CCP();
					
					MainTEM.Num_CCP	= 0;
					
					CreatingCCP = false;
					
					DestroyImmediate(MainTEM.transform.Find("NewControlPoints").gameObject);
					
					ManageCCP = 1;
				}
			}
			
			
			// ---------- Paused Create Control Points Screen 
			if(ManageCCP == 4)
			{
				TitleBold = "Creating New Points";
				Title = "\n\nPaused";
				
				if(GUILayout.Button("Done Creating\nControl Points"))
				{
					DoneCreatingCCP();
				}
				
				if(GUILayout.Button("Resume Creating\nControl Points"))
				{
					ManageCCP = 3;
					
					CreatingCCP = true;
				}
				
				if(GUILayout.Button("Cancel Creating\nControl Points"))
				{
					MainTEM.Zero_CCP();
					
					MainTEM.Num_CCP	= 0;
					
					CreatingCCP = false;
					
					DestroyImmediate(MainTEM.transform.Find("NewControlPoints").gameObject);
					
					ManageCCP = 1;
				}
			}
			
			
			// ---------- Create Semaphore Screen
			if(ManageCCP == 5)
			{

				TitleBold = "Adding Traffic Lights";
				Title = "\n\n";

				MainTEM.SetAddSemaphore();
				CreatingCCP = true;
			}
		}

		GUILayout.EndArea ();
		
		
// ---------- Here Is the Advanced Video Tutorial Button Link 
		/*
		if(ToggleAdvanced)
		{
			if(GUI.Button(new Rect(30, Math.Max(Screen.height,500) - 125,Math.Max(Screen.width,200) - 60,20), "Advanced Tutorial", ButtonStyle))
			{
				Application.OpenURL("http://www.youtube.com/watch?v=XE9qsqCFaZA");
			}
		}
		*/
		
// ---------- Here Is the Video Tutorial Button Link 
		if(GUI.Button(new Rect(30, Math.Max(Screen.height,500) - 95,Math.Max(Screen.width,200) - 60,20), "Video Tutorial", ButtonStyle))
		{
			Application.OpenURL("https://www.youtube.com/watch?v=qz8kevu07Wk");
		}
		
		
// ---------- Here is the Back and the Main Buttons
		ButtonStyle.fontSize = 12;
		if(Started && ((AddCars + ManageCCP + TrafficSettings > 0) || Extras))
		{
			if( GUI.Button(new Rect(10, Math.Max(Screen.height,500) - 65,50,20), "Back", ButtonStyle))
			{
				CreatingCCP = false;
				AddCars = Math.Max(Math.Min(AddCars - 1,1), 0);
				ManageCCP = Math.Max(Math.Min(ManageCCP - 1,1), 0);
				TrafficSettings = Math.Max(Math.Min(TrafficSettings - 1,1), 0);
				Extras = false;
			}

			if( GUI.Button(new Rect(Math.Max(Screen.width,200)/2 - 25, Math.Max(Screen.height,500) - 65,50,20), "Main", ButtonStyle))
			{
				CreatingCCP = false;
				AddCars = 0;
				ManageCCP = 0;
				TrafficSettings = 0;
				Extras = false;
			}
		}
		
		
// ---------- Here is the Extras Button 
/*		ButtonStyle.fontSize = 12;
		if(Started && !Extras && !(AddCars + ManageCCP + TrafficSettings > 0))
		{
			if( GUI.Button(new Rect(Math.Max(Screen.width,200)/2 - 25, Math.Max(Screen.height,500) - 65,50,20), "Extras", ButtonStyle))
			{
				Extras = true;
			}
		}
*/
		
// ---------- Here is the Demo and the Tutorial Buttons
		ButtonStyle.fontSize = 12;
		if(Started && !Extras && !(AddCars + ManageCCP + TrafficSettings > 0))
		{
			if(DemoTutorialCooldown > 10)
			{/*
				if( GUI.Button(new Rect(10, Math.Max(Screen.height,500) - 65,50,20), "Tutor.", ButtonStyle))
				{
					if(EditorApplication.SaveCurrentSceneIfUserWantsTo())
					{
						EditorApplication.OpenScene("Assets/EasyTraffic/DemoScene/Tutorial.unity");
						ETTutorial TutorialWindow = (ETTutorial)EditorWindow.GetWindow (typeof (ETTutorial));
					}
				}*/

				if( GUI.Button(new Rect(Math.Max(Screen.width,200)/2 - 25, Math.Max(Screen.height,500) - 65,50,20), "Demo", ButtonStyle))
				{
					if(EditorApplication.SaveCurrentSceneIfUserWantsTo())
					{
						EditorApplication.OpenScene("Assets/EasyTraffic/DemoScene/Scene.unity");
					}
				}
			}
			else DemoTutorialCooldown++;
		}
		else DemoTutorialCooldown = 0;


// ---------- Here is the Close Button 
		ButtonStyle.fontSize = 12;
		if( GUI.Button(new Rect(Math.Max(Screen.width,200) - 60, Math.Max(Screen.height,500) - 65,50,20), "Close", ButtonStyle))
		{
			CreatingCCP = false;
			this.Close();
		}
		
		
// ---------- Here is the Advanced Options Toggle 
		if(Started)
		{
			ToggleAdvanced = GUI.Toggle(new Rect(10, Math.Max(Screen.height,500) - 40, Math.Max(Screen.width,200) - 3, 40), ToggleAdvanced, "Advanced");
		}
		if(!WarningAdvanced && ToggleAdvanced)
		{
			if(EditorUtility.DisplayDialog("Advanced Options", "\"With great power comes great responsibility!\"\n\t- Uncle Ben\n\nUse Advanced Options with care!", "Proceed", "No, please, I'm not ready!"))
			{
				WarningAdvanced = true;
			}
			else
			{
				ToggleAdvanced = false;
			}
		}

		
// ---------- Here is the Version Display 
		TextStyle.fontSize = 8;
		TextStyle.alignment = TextAnchor.UpperRight;
		GUI.Label(new Rect(0, Math.Max(Screen.height,500) - 38, Math.Max(Screen.width,200) - 3, 40), ETversion, TextStyle);


		GUI.EndScrollView();
	}
	
	
	/*
	void OnFocus()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}

	void OnSceneGUI(SceneView sceneView)
	{
		Handles.BeginGUI();
		if(Selection.activeGameObject)
		{
			Handles.DrawLine(Selection.activeGameObject.transform.position, new Vector3 (0,0,0));
		}
		Handles.DrawLine(new Vector3 (0,0,0), new Vector3 (1000,1000,1000));
		Handles.EndGUI();    
	}
	
	void OnDestroy()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}
	*/


	void OnInspectorUpdate()
	{
		CheckPrefabsCooldown++;
		Repaint();
		if(MainTEM)
		{
			if(CheckPrefabsCooldown >= 100)
			{
				Check_Car_Prefabs(5);
				CheckPrefabsCooldown = 0;
			}
		}
	}

	void OnSelectionChange()
	{
		/*
		TEMSelected = false;
		if(!TEMSelected)
		{
			if(Selection.activeGameObject)
			{
				if(Selection.activeGameObject.GetComponentInChildren<TrafficEditorManage>() || Selection.activeGameObject.GetComponent<TrafficEditorManage>())
				{
					TEMSelected = true;
				}
			}
		}
		*/

		OldResetRotation = false;
		if(Selection.activeGameObject && ToggleAdvanced && AddCars == 2 && Manual)
		{
			TargetUpdate(Selection.activeGameObject);
			SetDrawWire(Selection.activeGameObject);
		}
		else
		{
			ResetDrawWire ();
			TargetUpdate();
		}
	}

	void Update()
	{
		if(!ToggleAdvanced || AddCars != 2 || !Manual)
		{
			ResetDrawWire ();
			Manual = false;
			Auto = true;
			ScaleAuto = 2;
			OldScaleAuto = 2;
			OldResetRotation = false;
			if(LastTarget) TargetUpdate();
		}
		if(!LastTarget)
		{
			if(Selection.activeGameObject && ToggleAdvanced && AddCars == 2 && Manual)
			{
				TargetUpdate(Selection.activeGameObject);
			}
		}

		if (MainTEM == null && Started)
		{
			if(GameObject.FindGameObjectWithTag("ET_TEM"))
			{
				StartET(AddCars, ManageCCP);
			}
			else
			{
				Started = false;
				EditorUtility.DisplayDialog("Caution!", "Attention!\nWe found that the \"TrafficEditorManager\" object is missing. It may have been deleted. You may want to undo until the \"TrafficEditorManager\" is restored.", "Close");
			}
		}
		
		if(MainTEM)
		{
			if(Selection.activeGameObject)
			{
				MainTEM.SetCarTargeted();
				MainTEM.SetCarTarget(Selection.activeGameObject);
				if(Selection.activeGameObject.CompareTag("Semaphoro"))
				{
					Semaphore Sema = Selection.activeGameObject.GetComponent<Semaphore>();
					if(Sema.NeedUpdate ())
					{
						
						foreach (Transform child in Sema.gameObject.transform)
						{
							child.gameObject.SetActive(true);
						}

						SemiUnit[] semaf = Sema.gameObject.GetComponentsInChildren<SemiUnit>();
						foreach (SemiUnit sm in semaf) 
						{
							//sm.transform.gameObject.SetActive(true);
							sm.SemaControl = Selection.activeGameObject.name;
							if(int.Parse(sm.name.Remove (0,5)) == 1 && Sema.Direction1 == false)
							{
								sm.transform.gameObject.SetActive(false);
							}
							if(int.Parse(sm.name.Remove (0,5)) == 2 && Sema.Direction2 == false)
							{
								sm.transform.gameObject.SetActive(false);
							}
							if(int.Parse(sm.name.Remove (0,5)) == 3 && Sema.Direction3 == false)
							{
								sm.transform.gameObject.SetActive(false);
							}
							if(int.Parse(sm.name.Remove (0,5)) == 4 && Sema.Direction4 == false)
							{
								sm.transform.gameObject.SetActive(false);
							}
						}

					}
				}
			}
			else
			{
				MainTEM.SetCarNotTargeted();
			}
		}


		if (CreatingCCP)
		{
			CurrentEvent = MainTEM.CurrentEvent;
			
			Selection.activeGameObject	= MainTEM.gameObject;

			if (CurrentEvent.button == 0 && CurrentEvent.type == EventType.MouseDown)
			{
				Create_CCP ();

				if (ManageCCP ==5)
				{
					MainTEM.Zero_CCP();
					
					MainTEM.Num_CCP	= 0;
					
					CreatingCCP = false;
					
					DestroyImmediate(MainTEM.transform.Find("NewControlPoints").gameObject);
					
					ManageCCP = 1;
				}
			}

			Selection.activeGameObject	= MainTEM.gameObject;
		}
	}
}