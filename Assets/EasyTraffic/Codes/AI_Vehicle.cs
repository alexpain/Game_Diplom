using UnityEngine;
using System.Collections;

public class AI_Vehicle : MonoBehaviour 
	{

			bool	 	Speed_Reduce;			// Needs to reduce velocity
		
			int			CCP_Compares;			// Variable that compares the vehicle CCP
 			bool 		Exe_Correction;
			float		Changing_Lane_Time;		// Changing lane Time
	public 	bool 		DoNot_ChangingLane;		// Flag for avoid changing lane
	
			Useful	 	UTL = new Useful();  	// Utility object
	
			bool 		Initiate = false;		// Flag to iniciate the processing
	
			float  	 	Time_Destruct;			// Vehicle destruction time
			bool 		deviation_of_vehicle;   // Vehicle deflection
	
	
	public 	GameObject 	Horn;					// GameObject Sound Horn 
			float		Time_Horn;				// Horn time 
			float		Time_Orient;			// Horn time count default
		
	
	
	
	// Use this for initialization
	void Start ()
		{
		Changing_Lane_Time 		= 0.0f;
		
		Speed_Reduce 			= false;
		
		Exe_Correction			= false;
		Time_Destruct			= 3.2f;
		
		deviation_of_vehicle	= false;
		
		Time_Horn				= 0.0f;
		Time_Orient				= Random.Range(4,7);
		
		}
	
	
		// Vehicle changing lane function
	void Changing_Lane()
		{
		if( Changing_Lane_Time <= 0.0f)
			{
			int qtd_faixas = 0;
			int pd = gameObject.GetComponent<Vehicle_Control>().Current_CCP;

			qtd_faixas = GameObject.Find("PD0"+pd).GetComponent<CCP>().Lanes;

				{
				int i = Random.Range(1,15);
				int t = Random.Range(1,15);
			
				if( ( i == t ) && (gameObject.GetComponent<Vehicle_Control>().Vector_Size >= 11) )
					{
					int faixa_atual = gameObject.GetComponent<Vehicle_Control>().Lane;
					
					int adicao = Random.Range(1,5);
					if((adicao >=1) && (adicao <=2)) 	{ adicao = -1; }
					else    							{ adicao = 1; }
					
					
					faixa_atual += adicao;
				
					if( faixa_atual <= 0) { faixa_atual = 1; }
					else if( faixa_atual > qtd_faixas ) { faixa_atual = qtd_faixas; }

					gameObject.GetComponent<Vehicle_Control>().Lane = faixa_atual;
					
					Changing_Lane_Time = Random.Range(7,10);
					}
				}
			}
		else if(gameObject.GetComponent<Vehicle_Control>().Aceleration > 0.25f)
			{
			Changing_Lane_Time -= Time.deltaTime;
			
			if( Changing_Lane_Time <= 0.0f)
				{
				deviation_of_vehicle = false;
				}
			}
		
		}
	
	
	
		// Verify the situation with other vehicles 
	void Comparing_Other_Vehicles()
		{
		Speed_Reduce 		= false;
		
		if(gameObject.GetComponentInChildren<Vehicle_Detector>())
			{
			Vehicle_Detector[] DV = gameObject.GetComponentsInChildren<Vehicle_Detector>();
			foreach (Vehicle_Detector rd in DV)
				{
				if(rd.VehicleAlert)
					{
					
					if( !deviation_of_vehicle )
						{
						deviation_of_vehicle	= true;
						Changing_Lane_Time 		= 0.0f;
						}
					
					if(Time_Horn <= 0.0f)  	{ Time_Horn = Time_Orient; }
					Speed_Reduce 				= true;
					
					rd.VehicleAlert	= false;
					}
				}
			
			
			Sides_Detector[] DL = gameObject.GetComponentsInChildren<Sides_Detector>();
			foreach (Sides_Detector rl in DL)
				{
				
				if(rl.Invert)
					{
					
					//if(rl.Side) { gameObject.GetComponent<Vehicle_Control>().Lane = 1; }
					//else  		{ gameObject.GetComponent<Vehicle_Control>().Lane = 2; }
					
					rl.Invert = false;
					}
				}
			
			}
		
		
		}
	
	
	
		// Increases and reduces the vehicle's velocity
	void Aceleration_Reduce()
		{
		float press_vol = UTL.Natural_Number(gameObject.GetComponent<Vehicle_Control>().Gear_Pressure)/(gameObject.GetComponent<Vehicle_Control>().Max_Gear * 100);
				
		gameObject.GetComponent<Vehicle_Control>().Aceleration  +=  Time.deltaTime * 3.75f;
		
		gameObject.GetComponent<Vehicle_Control>().Aceleration  -= press_vol; 
		
		
		if(Speed_Reduce)
			{
			
			float frenador = 4.5f; 
			
			float calc = gameObject.GetComponent<Vehicle_Control>().Aceleration / frenador;
			
			gameObject.GetComponent<Vehicle_Control>().Aceleration -= calc;
			
			if(gameObject.GetComponent<Vehicle_Control>().Aceleration < 0.0f)
				{
				gameObject.GetComponent<Vehicle_Control>().Aceleration = 0;
				}
			}
		}
	
	
	
		// Maintain the vehicle inside the lane until somebody hits it
	void Segue_Linha()
		{
		Vector2 PD1 = gameObject.GetComponent<Vehicle_Control>().Fake_CCP1;
		Vector2 PD2 = gameObject.GetComponent<Vehicle_Control>().Fake_CCP2;
		
		Vector2 Vei = UTL.Perpendicular_Vector(gameObject.transform.position,1);
		
		Vector2 Analise = UTL.Vector(PD1,PD2);
		
		float resp = 0.0f;
		
		if( UTL.Natural_Number(Analise.y) > UTL.Natural_Number(Analise.x) )
			{
			resp = (Vei.y - PD1.y) / Analise.y;
			
			resp = Analise.x * resp;
			resp = resp + PD1.x;
			
			Vector3 norm = gameObject.transform.position;
			
			resp = ((resp - norm.x)/2) + norm.x;
			
			gameObject.transform.position = new Vector3(resp,norm.y,norm.z);
			}
		else
			{
			resp = (Vei.x - PD1.x) / Analise.x;
			
			resp = Analise.y * resp;
			resp = resp + PD1.y;
			
			Vector3 norm = gameObject.transform.position;
			
			resp = ((resp - norm.z)/2) + norm.z;
			
			gameObject.transform.position = new Vector3(norm.x,norm.y,resp);
			}
		}
		
	
	
		// Destructs vehicle with contact 
	void Destruido_Contato()
		{
		
		if(Time_Destruct <= 0.0f)
			{
			gameObject.GetComponent<Vehicle_Control>().SpawnSmoke();

			Destroy(this.gameObject);
			}
		}
	
	
		// Horn function 
	void Buzinar()
		{
		if(Time_Horn >= Time_Orient)
			{
			GameObject SpawnHorn = GameObject.Instantiate(Horn, gameObject.transform.position, Quaternion.identity) as GameObject;
			SpawnHorn.name = "Horn";
			SpawnHorn.transform.parent = transform.parent.parent.Find("Effects");


			Time_Orient = Random.Range(4,7);
			
			Time_Horn -= 0.01f;
			}
			
		if(Time_Horn > 0.0f){ Time_Horn -= Time.deltaTime; }
		}
		
	
	
	// Update is called once per frame
	void Update () 
		{
		
		if(gameObject.GetComponent<Vehicle_Control>())
			{
			if(!Initiate)
				{
				Vector2 Meio 					= gameObject.GetComponent<Vehicle_Control>().Normal;
				if(UTL.Vector_Size(Meio) > 0)
					{
					float y	 						= gameObject.transform.position.y;
					gameObject.transform.position 	= new Vector3(Meio.x,y,Meio.y);
					Initiate = true;
					}
				}
			else
				{	
				Destruido_Contato();
				
				Buzinar();
				
				Comparing_Other_Vehicles();
				
				if(!DoNot_ChangingLane) { Changing_Lane(); }
			
				Segue_Linha();
				if(!Exe_Correction)
					{
					Exe_Correction = true;
					}
				else
					{
					if(gameObject.GetComponent<Vehicle_Control>().Current_CCP != CCP_Compares)
						{
						CCP_Compares = gameObject.GetComponent<Vehicle_Control>().Current_CCP;
						Exe_Correction = false;
						}
					Exe_Correction = false;
					}
				
				Aceleration_Reduce();
				}
			}
		}
	
	}