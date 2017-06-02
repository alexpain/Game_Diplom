using UnityEngine;
//using UnityEditor;
using System.Collections;
using System;

public class CCP_Projector2 : MonoBehaviour 
{
	public	GameObject			Arrows;
	
	public	float				Distances;
	
	bool				first	=	false;
	
	Vector3				Before;
	
	public	int					Nodes;
	
	Useful				UTL;
	
	public	LineCCP2			Line;
	
	public	TrafficEditorManage	MainTEM;
	
	public	CCP_Projector2		Next;
	
	public	Transform			Prior;
	
	public	bool				Dead_end;
	
	public	int					MyNumber;
	
	public	bool				Add_Semaphore;
	
	Vector3[]			SemaphorePos;
	
	public	Transform			Inter;
	
	public	void ChangeFirst(bool ft)
	{
		first = ft;
	}
	
	
	
	public	bool Intersect(Vector2 A1, Vector2 A2)
	{
		GameObject PD1;
		
		GameObject PD2;
		
		bool resp = false;
		
		int i = 0;
		
		GameObject[] PDS = GameObject.FindGameObjectsWithTag("PD");
		
		PD1 = GameObject.Find("PD01");
		
		while( i<PDS.Length-1)
		{
			PD2 = PD1.GetComponent<CCP>().NextCCP.gameObject;
			
			if(!PD1.GetComponent<CCP>().Destroy_Car)
			{
				Vector2 B1 = new Vector2(PD2.transform.position.x,  PD2.transform.position.z);
				
				Vector2 B2 = new Vector2(PD1.transform.position.x,    PD1.transform.position.z);
				
				Vector2 Int = new Vector2(0,0);
				
				Int = UTL.Intersection_Point(A1, A2, B1, B2) ;
				
				if(Inter)
				{
					Inter.position	= new Vector3(Int.x,0,Int.y);
				}
				
				if( ( ((Int.x >= A1.x) && (Int.x <= A2.x)) || ((Int.x >= A2.x) && (Int.x <= A1.x)) ) && 
				   ( ((Int.y >= A1.y) && (Int.y <= A2.y)) || ((Int.y >= A2.y) && (Int.y <= A1.y)) ) )
				{
					
					if( (((Int.x >= B1.x) && (Int.x <= B2.x)) || ((Int.x >= B2.x) && (Int.x <= B1.x)) ) && 
					   (((Int.y >= B1.y) && (Int.y <= B2.y)) || ((Int.y >= B2.y) && (Int.y <= B1.y)) ) )
					{						
						Add_Semaphore 	= true;
						
						resp			= true;
						
						float y = ((transform.position.y - Prior.position.y)/2) + Prior.position.y;
						
						Array.Resize(ref SemaphorePos, SemaphorePos.Length+1);
						
						SemaphorePos[SemaphorePos.Length-1]	= new Vector3(Int.x, y, Int.y);
						
						i = PDS.Length+1;
					}
					else
					{
						//Add_Semaphore 	= false;
					}
					
				}
				else
				{
					//Add_Semaphore 	= false;
				}
				
				
			}
			else
			{
				
			}
			i++;
			
			PD1 = PD2;
		}
		
		return resp;
	}
	
	// Coloca os Semaforos nos cruzamentos 
	public void Adjust_Semaphore()
	{
		
		{
			if((MainTEM.Begin_Count > 0) && (Prior != null) )
			{
				
				
				Vector2 A1 = new Vector2(transform.position.x,transform.position.z);
				
				Vector2 A2 = new Vector2(Prior.position.x,    Prior.position.z);
				
				if(Prior.GetComponent<CCP_Projector2>().Nodes == 0)
				{					
					Intersect(A1, A2);
					
				}
				else
				{
					
					LineCCP2 Nd = Prior.GetComponentInChildren<LineCCP2>();
					
					int i = 0;
					
					if(Nd.Vertex.Length > 0)
					{
						while(i < Nd.Vertex.Length+1 )
						{
							
							if(i == 0) 
							{
								A1 = new Vector2(Nd.Vertex[i].x,      Nd.Vertex[i].z); 
								A2 = new Vector2(Prior.position.x,    Prior.position.z);
							}
							else if(i == Nd.Vertex.Length)
							{
								A1 = new Vector2(transform.position.x,    transform.position.z);
								A2 = new Vector2(Nd.Vertex[i-1].x,        Nd.Vertex[i-1].z); 
							}
							else
							{
								A1 = new Vector2(Nd.Vertex[i].x,Nd.Vertex[i].z);
								A2 = new Vector2(Nd.Vertex[i-1].x,Nd.Vertex[i-1].z); 
							}
							
							//if(Intersect(A1, A2)) { i = Nd.Vertex.Length+2; }
							
							Intersect(A1, A2);
							
							i++;
						}
					}
				}
				
			}
			
		}
	}
	
	public void	Reajusta_Direcao(Vector3 dir)
	{
		Vector3	Vet = UTL.Normalize_Vector(dir - Line.transform.position);
		
		Arrows.transform.position = transform.position + Vet;
	}
	
	public void Starter(TrafficEditorManage StarterManager, Vector3 PosNow)
	{
		SemaphorePos = new Vector3[0];
		
		Add_Semaphore = false;
		
		UTL = new Useful();
		
		Dead_end = true;
		
		MainTEM = StarterManager;
		
		Nodes = 0;
		
		Before = gameObject.transform.position;
		
		Distances = 0.0f;
		
		Arrows = Instantiate(Resources.Load("Arrow1")) as GameObject;
		Arrows.name	= "Arrow01";
		
		Arrows.transform.position = gameObject.transform.position;
		Arrows.transform.Translate(0,0,-3);
		
		Arrows.transform.parent = gameObject.transform; 
		
		
		first = true;
		
		string NewName = "CCP_Projector";
		
		name = NewName+MainTEM.Num_CCP;
		
		
		gameObject.transform.position	= PosNow;
		
		if(MainTEM.Num_CCP > 0)
		{
			int sum = MainTEM.Num_CCP-1;
			
			NewName += sum;
			
			GameObject 	NewCCPT = GameObject.Find(NewName);
			
			NewCCPT.GetComponent<CCP_Projector2>().Dead_end = false;
			
			if(!NewCCPT.GetComponent<CCP_Projector2>().Dead_end)
			{
				NewCCPT.GetComponent<CCP_Projector2>().Next = this.GetComponent<CCP_Projector2>();
				
				Prior						= NewCCPT.transform;
				
				GameObject LineCCP 			= new GameObject();
				LineCCP.name				= "LineCCP"+sum;
				
				LineCCP.transform.parent	= NewCCPT.transform;
				
				LineCCP.gameObject.tag		= "ET_LINE";
				
				LineCCP.AddComponent<LineRenderer>();
				
				LineCCP.GetComponent<LineRenderer>().material = Resources.Load("LineCCP2", typeof(Material)) as Material;
				
				LineCCP.AddComponent<LineCCP2>();
				
				LineCCP.GetComponent<LineCCP2>().SetUp(MainTEM, NewCCPT.transform , gameObject.transform );
			}
			
		}
		
		MainTEM.Add_CCP();
		
		MyNumber = MainTEM.Return_CPPProjectors();
	}
	
	
	void OnDrawGizmos()
	{
		if(Line)
		{
			Line.Number_Nodes(Nodes);
		}
		
		Vector3	Set = new Vector3(0,0,0);
		if(Line)	{ Set = Line.Mediana; 			}
		else 		{ Set = gameObject.transform.position; }
		
		
		if( (first) && (Arrows) )
		{
			Before		= Set - Before;
			
			Vector3	Repos = (Arrows.transform.position - Set) + Arrows.transform.position;
			
			Arrows.transform.LookAt(Repos);
			
			Repos 	= Arrows.transform.position + Before;
			Repos.y = Set.y;
			
			Arrows.transform.position	= Repos;
			
			Distances = UTL.Distance(Set, Arrows.transform.position );
			
			Before			= Set;
		}
		
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////
	
	public	void Create_RealCPPs()
	{
		GameObject NewRealCPP = Instantiate(Resources.Load("PD0")) as GameObject;
		
		NewRealCPP.transform.position = transform.position;
		
		NewRealCPP.transform.parent = MainTEM.transform.Find("ControlPoints");
		
		int soma = MainTEM.Add_CPP();
		
		NewRealCPP.name = "PD0"+soma;
		
		NewRealCPP.GetComponent<CCP>().CCPID = 1;
		
		NewRealCPP.GetComponent<CCP>().MainTEM = MainTEM;

		NewRealCPP.GetComponent<CCP>().SetSize(MainTEM.GetRoadSize());
		NewRealCPP.GetComponent<CCP>().SetLanes(MainTEM.GetLanes());
		NewRealCPP.GetComponent<CCP>().SetScale(MainTEM.GetScale());


		if(!Dead_end)
		{
			for(int i=0; i<Line.Vertex.Length; i++)
			{
				GameObject Td 			= Instantiate(Resources.Load("PD0")) as GameObject;
				
				Td.transform.position	= Line.Vertex[i];
				
				Td.transform.parent		= MainTEM.transform.Find("ControlPoints");
				
				Td.name				= "PD0"+(MainTEM.Add_CPP());
				
				Td.GetComponent<CCP>().MainTEM = MainTEM;
				
				Td.GetComponent<CCP>().CCPID = 2;
			}
		}

		if (MainTEM.IsFirstCCP())
		{
			NewRealCPP.GetComponent<CCP>().Spawn_Direction1 = true;
			NewRealCPP.GetComponent<CCP>().Destroy_Direction2 = true;
			MainTEM.SetNotFirstCCP();
		}

		if(Dead_end)
		{
			NewRealCPP.GetComponent<CCP>().Spawn_Direction2 = true;
			NewRealCPP.GetComponent<CCP>().Destroy_Direction1 = true;
		}




		if(Add_Semaphore)
		{
			
			for(int i=0; i<SemaphorePos.Length; i++)
			{
				GameObject.FindGameObjectWithTag("ET_TEM").GetComponent<TrafficEditorManage>().Add_Semaphore(SemaphorePos[i]);
			}
		}
		
	}
}




/*
 * using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class CCP_Projector2 : MonoBehaviour 
{
	public	GameObject			Arrows;
	
	public	float				Distances;
	
	bool				first	=	false;
	
	Vector3				Before;
	
	public	int					Nodes;
	
	Useful				UTL;
	
	public	LineCCP2			Line;
	
	public	TrafficEditorManage	MainTEM;
	
	public	CCP_Projector2		Next;
	
	public	Transform			Prior;
	
	public	bool				Dead_end;
	
	public	int					MyNumber;
	
	public	bool				Add_Semaphore;
	
	Vector3[]			SemaphorePos;
	
	public	Transform			Inter;
	
	public	void ChangeFirst(bool ft)
	{
		first = ft;
	}
	
	
	
	public	bool Intersect(Vector2 A1, Vector2 A2)
	{
		GameObject PD1;
		
		GameObject PD2;
		
		bool resp = false;
		
		int i = 0;
		
		GameObject[] PDS	= GameObject.FindGameObjectsWithTag("PD");
		
		PD1				    = GameObject.Find("PD01");
		
		while( i<PDS.Length-1)
		{
			PD2				    = PD1.GetComponent<CCP>().NextCCP.gameObject;
			
			if(!PD1.GetComponent<CCP>().Destroy_Car)
			{
				Vector2 	B1 = new Vector2(PD2.transform.position.x,  PD2.transform.position.z);
				
				Vector2 	B2 = new Vector2(PD1.transform.position.x,    PD1.transform.position.z);
				
				Vector2 Int = new Vector2(0,0);
				
				Int = UTL.Intersection_Point(A1, A2, B1, B2) ;
				
				if(Inter)
				{
					Inter.position	= new Vector3(Int.x,0,Int.y);
				}
				
				if( ( ((Int.x >= A1.x) && (Int.x <= A2.x)) || ((Int.x >= A2.x) && (Int.x <= A1.x)) ) && 
				   ( ((Int.y >= A1.y) && (Int.y <= A2.y)) || ((Int.y >= A2.y) && (Int.y <= A1.y)) ) )
				{
					
					if( (((Int.x >= B1.x) && (Int.x <= B2.x)) || ((Int.x >= B2.x) && (Int.x <= B1.x)) ) && 
					   (((Int.y >= B1.y) && (Int.y <= B2.y)) || ((Int.y >= B2.y) && (Int.y <= B1.y)) ) )
					{						
						Add_Semaphore 	= true;
						
						resp			= true;
						
						float y = ((transform.position.y - Prior.position.y)/2) + Prior.position.y;
						
						Array.Resize(ref SemaphorePos, SemaphorePos.Length+1);
						
						SemaphorePos[SemaphorePos.Length-1]	= new Vector3(Int.x, y, Int.y);
						
						i = PDS.Length+1;
					}
					else
					{
						//Add_Semaphore 	= false;
					}
					
				}
				else
				{
					//Add_Semaphore 	= false;
				}
				
				
			}
			else
			{
				
			}
			i++;
			
			PD1 = PD2;
		}
		
		return resp;
	}
	
	// Coloca os Semaforos nos cruzamentos 
	public void Adjust_Semaphore()
	{
		
		{
			if((MainTEM.Begin_Count > 0) && (Prior != null) )
			{
				
				
				Vector2 A1 = new Vector2(transform.position.x,transform.position.z);
				
				Vector2 A2 = new Vector2(Prior.position.x,    Prior.position.z);
				
				if(Prior.GetComponent<CCP_Projector2>().Nodes == 0)
				{					
					Intersect(A1, A2);
					
				}
				else
				{
					
					LineCCP2 Nd = Prior.GetComponentInChildren<LineCCP2>();
					
					int i = 0;
					
					if(Nd.Vertex.Length > 0)
					{
						while(i < Nd.Vertex.Length+1 )
						{
							
							if(i == 0) 
							{
								A1 = new Vector2(Nd.Vertex[i].x,      Nd.Vertex[i].z); 
								A2 = new Vector2(Prior.position.x,    Prior.position.z);
							}
							else if(i == Nd.Vertex.Length)
							{
								A1 = new Vector2(transform.position.x,    transform.position.z);
								A2 = new Vector2(Nd.Vertex[i-1].x,        Nd.Vertex[i-1].z); 
							}
							else
							{
								A1 = new Vector2(Nd.Vertex[i].x,Nd.Vertex[i].z);
								A2 = new Vector2(Nd.Vertex[i-1].x,Nd.Vertex[i-1].z); 
							}
							
							//if(Intersect(A1, A2)) { i = Nd.Vertex.Length+2; }
							
							Intersect(A1, A2);
							
							i++;
						}
					}
				}
				
			}
			
		}
	}
	
	public void	Reajusta_Direcao(Vector3 dir)
	{
		Vector3	Vet		= UTL.Normalize_Vector(dir - Line.transform.position);
		
		Arrows.transform.position = transform.position + Vet;
	}
	
	public	void Starter(TrafficEditorManage StarterManager, Vector3 PosNow)
	{
		SemaphorePos	= new Vector3[0];
		
		Add_Semaphore	= false;
		
		//SemaphorePos	= new Vector3(0,0,0);
		
		UTL				= new Useful();
		
		Dead_end		= true;
		
		MainTEM			= StarterManager;
		
		Nodes			= 0;
		
		Before			= gameObject.transform.position;
		
		Distances		= 0.0f;
		
		Arrows 		= Instantiate(Resources.Load("Arrow1")) as GameObject;
		Arrows.name	= "Arrow01";
		
		Arrows.transform.position = gameObject.transform.position;
		Arrows.transform.Translate(0,0,-3);
		
		Arrows.transform.parent = gameObject.transform; 
		
		
		first			= true;
		
		string	NewName = "CCP_Projector";
		
		name			= NewName+MainTEM.Num_CCP;
		
		
		gameObject.transform.position	= PosNow;
		
		if(MainTEM.Num_CCP > 0)
		{
			int sum 	= MainTEM.Num_CCP-1;
			
			NewName 	+= sum;
			
			GameObject 	NewCCPT = GameObject.Find(NewName);
			
			NewCCPT.GetComponent<CCP_Projector2>().Dead_end = false;
			
			if(!NewCCPT.GetComponent<CCP_Projector2>().Dead_end)
			{			
				NewCCPT.GetComponent<CCP_Projector2>().Next = this.GetComponent<CCP_Projector2>();
				
				Prior						= NewCCPT.transform;
				
				GameObject LineCCP 			= new GameObject();
				LineCCP.name				= "LineCCP"+sum;
				
				LineCCP.transform.parent	= NewCCPT.transform;
				
				LineCCP.gameObject.tag		= "ET_LINE";
				
				LineCCP.AddComponent<LineRenderer>();
				
				LineCCP.GetComponent<LineRenderer>().material = Resources.Load("LineCCP2", typeof(Material)) as Material;
				
				LineCCP.AddComponent<LineCCP2>();
				
				LineCCP.GetComponent<LineCCP2>().SetUp(MainTEM, NewCCPT.transform , gameObject.transform );
			}
			
		}
		
		MainTEM.Add_CCP();
		
		MyNumber = MainTEM.Return_CPPProjectors();
	}
	
	
	void OnDrawGizmos()
	{
		if(Line)
		{
			Line.Number_Nodes(Nodes);
		}
		
		Vector3	Set = new Vector3(0,0,0);
		if(Line)	{ Set = Line.Mediana; 			}
		else 		{ Set = gameObject.transform.position; }
		
		
		if( (first) && (Arrows) )
		{
			Before		= Set - Before;
			
			Vector3	Repos = (Arrows.transform.position - Set) + Arrows.transform.position;
			
			Arrows.transform.LookAt(Repos);
			
			Repos 	= Arrows.transform.position + Before;
			Repos.y = Set.y;
			
			Arrows.transform.position	= Repos;
			
			Distances = UTL.Distance(Set, Arrows.transform.position );
			
			Before			= Set;
		}
		
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////
	
	public	void Create_RealCPPs()
	{
		
		if(!Dead_end)
		{
			GameObject NewRealCPP 			= Instantiate(Resources.Load("PD0")) as GameObject;
			
			bool	inter			= false;
			
			NewRealCPP.transform.position	= transform.position;
			
			NewRealCPP.transform.parent	= MainTEM.transform;
			
			int soma 				= MainTEM.Add_CPP();
			
			if(MainTEM.Return_CountCPP() <= 1) 
			{ 
				MainTEM.Add_CPP();
				MainTEM.Begin_Count = soma;
			}
			
			NewRealCPP.name				= "PD0"+soma;

			NewRealCPP.GetComponent<CCP>().CCPID = 1;
			
			for(int i=0; i<Line.Vertex.Length; i++)
			{
				inter					= true;
				
				GameObject Td 			= Instantiate(Resources.Load("PD0")) as GameObject;
				
				Td.transform.position	= Line.Vertex[i];
				
				Td.transform.parent		= MainTEM.transform;
				
				Td.name				= "PD0"+(MainTEM.Add_CPP());

				Td.GetComponent<CCP>().MainTEM = MainTEM;

				Td.GetComponent<CCP>().CCPID = 2;
			}
			
			
			if( Next.Dead_end) 
			{
				GameObject Mk1 			= Instantiate(Resources.Load("PD0")) as GameObject;
				
				Mk1.name				= "PD0"+(MainTEM.Begin_Count+1);
				
				Mk1.transform.position	= GameObject.Find("PD0"+MainTEM.Begin_Count).transform.position;
				
				if(MainTEM.Num_CCP > 2)
				{
					Mk1.transform.position  = ( UTL.Normalize_Vector(GameObject.Find("PD0"+(MainTEM.Begin_Count+2)).transform.position - Mk1.transform.position) * 6 ) + Mk1.transform.position;
				}
				else
				{
					if(inter)
					{
						Mk1.transform.position  = ( UTL.Normalize_Vector(GameObject.Find("PD0"+(MainTEM.Begin_Count+2)).transform.position - Mk1.transform.position) * 6 ) + Mk1.transform.position;
					}
					else
					{
						Mk1.transform.position  = ( UTL.Normalize_Vector(Next.gameObject.transform.position - Mk1.transform.position ) * 6 ) + Mk1.transform.position;
					}
				}
				
				
				Mk1.transform.parent	= MainTEM.transform;
				
				Mk1.GetComponent<CCP>().Spawn_Car		= true;

				Mk1.GetComponent<CCP>().MainTEM = MainTEM;

				Mk1.GetComponent<CCP>().CCPID = 3;
				
				///////////////////////////////////////////////////////
				
				GameObject Mk2 			= Instantiate(Resources.Load("PD0")) as GameObject;
				
				Mk2.name				= "PD0"+(MainTEM.Count_CPPManager+1);
				
				Mk2.transform.position	= GameObject.Find("PD0"+MainTEM.Count_CPPManager).transform.position;
				
				Mk2.transform.position  = ( UTL.Normalize_Vector( Mk2.transform.position - Next.gameObject.transform.position) * 6 ) +Next.gameObject.transform.position;
				
				if(MainTEM.Num_CCP <= 2)
				{
					Mk2.transform.position  += ( Next.gameObject.transform.position - Mk2.transform.position ) / 1.5f;
				}
				
				Mk2.transform.parent	= MainTEM.transform;
				
				Mk2.GetComponent<CCP>().Spawn_Car		= true;
				
				Mk2.GetComponent<CCP>().Opposite_direct	= true;

				Mk2.GetComponent<CCP>().MainTEM = MainTEM;

				Mk2.GetComponent<CCP>().CCPID = 4;
				
				///////////////////////////////////////////////////////
				
				GameObject Td2 			= Instantiate(Resources.Load("PD0")) as GameObject;
				
				Td2.transform.position	= Next.gameObject.transform.position;
				
				Td2.transform.parent	= MainTEM.transform;
				
				MainTEM.Add_CPP();
				
				Td2.name				= "PD0"+(MainTEM.Add_CPP());
				
				Td2.GetComponent<CCP>().Destroy_Car	= true;

				Td2.GetComponent<CCP>().MainTEM = MainTEM;

				Td2.GetComponent<CCP>().CCPID = 5;
			}
			
		}
		
		if(Add_Semaphore)
		{
			
			for(int i=0; i<SemaphorePos.Length; i++)
			{
				GameObject.FindGameObjectWithTag("ET_TEM").GetComponent<TrafficEditorManage>().Add_Semaphore(SemaphorePos[i]);
			}
		}
		
	}
}
*/