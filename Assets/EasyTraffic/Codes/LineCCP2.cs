using UnityEngine;
using System.Collections;
using System;

public class LineCCP2 : MonoBehaviour 
	{

	public	TrafficEditorManage	Manager;
			
			bool				first	= false;
	
			int					Nodes;
	
			Transform[]			Limits;
	
			Useful				UTL;
	
			float				SizeLimits;
	
			Vector3				Apice;
	
	public	Vector3				Mediana;
	
			Vector3 			N1;
	
	public	Vector3[]			Vertex;
	
	public	Transform			Inter;
	
	
	public	void Number_Nodes(int nb)
		{
		Nodes	= nb;
		}
	
	public	void SetUp(TrafficEditorManage MM, Transform L1, Transform L2)
		{
		UTL			= new Useful();
		
		Vertex		= new Vector3[0];
		
		GetComponent<LineRenderer>().SetWidth(0.35f,0.35f);
		
		GetComponent<LineRenderer>().SetColors(new Color(0,1,0,1), new Color(1,0,0,1) );
				
		Manager		= MM;
		
		first		= true;
		
		Limits		= new Transform[2];
		
		Limits[0]	= L1;
		Limits[1]	= L2;
		
		Apice		= new Vector3(0,0,0);
		
		Mediana		= new Vector3(0,0,0);
		
		N1			= new Vector3(0,0,0);
		
		Limits[0].GetComponent<CCP_Projector2>().Line = this.GetComponent<LineCCP2>();
		
		Limits[0].GetComponent<CCP_Projector2>().Reajusta_Direcao(Limits[1].position);
		
		Number_Nodes(Limits[0].GetComponent<CCP_Projector2>().Nodes);
		
		}
	
		
	
	bool	CrossLine(Vector3 R)
		{
		Vector3 F	= (Limits[1].position - Limits[0].position) / 2;
		
		Vector3	Troca = F;
		
		F	= UTL.Normalize_Vector(new Vector3(-Troca.z, Troca.y, Troca.x)) + Mediana;
		
		Vector2	A1 = new Vector2(F.x,F.z);
		
		Vector2	A2 = new Vector2(R.x,R.z);
		
		Vector2 B1 = new Vector2(Limits[0].position.x, Limits[0].position.z);
		
		Vector2 B2 = new Vector2(Limits[1].position.x, Limits[1].position.z);
		
		if(B1.x == B2.x) { B2.x += 0.01f; }
		if(B1.y == B2.y) { B2.y -= 0.01f; }
		
		Vector2 Int = UTL.Intersection_Point(A1, A2, B1, B2);
		
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
				return true;
				}
			else
				{
				return false;
				}
			}
		else
			{
			return false;
			}
		}
	
	
	void Calc_Apice()
		{
		SizeLimits	= UTL.Vector_Size(Limits[1].position - Limits[0].position);
		
		Mediana 	= ((Limits[1].position - Limits[0].position) / 2) + Limits[0].position;
		
		N1 			= (Limits[0].GetComponent<CCP_Projector2>().Arrows.transform.position - Mediana);// / SizeLimits;
		
		
		Apice 		= Mediana + N1; 
		
		bool mud 	= CrossLine(Apice);
		
		////////////////////////////////////////////////////////////////
		
		Apice			= (Limits[1].position - Limits[0].position) / 2;
		
		Vector3	Troca = Apice;
		
		if(!mud)
			{
			Apice		= new Vector3(-Troca.z, Troca.y, Troca.x);
			}
		else
			{
			Apice		= new Vector3(Troca.z, Troca.y,-Troca.x);
			}
		
		Apice 		= Mediana + Apice;
		}
	
	
	
	void ColdProssessing(int Index, Vector3 Point)
		{
		Vector3	Corredor 	= new Vector3(0,0,0);
		
		Vector3	Base		= new Vector3(0,0,0);
		
		Vector3	Origem		= new Vector3(0,0,0);
		
		float 	sizeVt 		= UTL.Vector_Size(Point - Limits[0].position);
		
		float 	MedSize		= UTL.Vector_Size(Apice - Mediana);
		
		if( sizeVt <= (SizeLimits/2))
			{
			Corredor	= Apice - Limits[0].position;
			Base		= Limits[0].position;
			Origem		= Limits[0].position;
			}
		else
			{
			Corredor	= Limits[1].position - Apice;
			Base		= Apice;
			sizeVt		= sizeVt - (SizeLimits/2);
			Origem		= Limits[1].position;
			}
		sizeVt 			= sizeVt/(SizeLimits/2);
			
		Corredor 		= (Corredor * sizeVt) + Base;
		
		Corredor		= (UTL.Normalize_Vector(Corredor - Mediana) * MedSize  ) + Mediana ;
		
		Corredor		=  Corredor - Point;
				
		sizeVt			= ( UTL.Vector_Size(N1) ) /  SizeLimits;
		
		Corredor		*= (sizeVt * 2 );		
		
		////////////////////////////////////////////////////////////////////////////////////
		
		Point			+= Corredor;
		
		////////////////////////////////////////////////////////////////////////////////////
		
		float			a = UTL.Vector_Size(Apice - Point);
		float			b = UTL.Vector_Size(Apice - Origem);
		
		if(a <= b)		{ sizeVt 	=  1 - (a / b) ; }
		else 			{ sizeVt 	=  0.0f; }
		
		Point			+= (N1) * sizeVt;
		
		GetComponent<LineRenderer>().SetPosition(Index, Point);
		
		Vertex[Index-1] = Point;	
		}
	
	
	
	// Use this for initialization
	void Start () 
		{
		
		}
	
	
	void OnDrawGizmos()
		{
		if(first)
			{
			if(Limits[1] != null)
				{
			
				Vector3	VetSum		= (Limits[1].position - Limits[0].position) / (Nodes+1);
				Vector3	Prior		= Limits[0].position;
			
				Calc_Apice();
			
				int sum 	= Nodes+2;
			
				GetComponent<LineRenderer>().SetVertexCount(sum);
			
				Balancing_Nodes();
			
				for(int i=0; i<sum; i++)
					{
					if(i == 0)
						{
						GetComponent<LineRenderer>().SetPosition(i, Limits[0].position);
						}
					else if(i == (sum - 1))
						{
						GetComponent<LineRenderer>().SetPosition(i, Limits[1].position);
						}
					else
						{
						Prior += VetSum;
					
						ColdProssessing(i, Prior);
						}
					}
				}
			}
		}
	
	
	void Balancing_Nodes()
		{
		if(Vertex.Length != Nodes)
			{
			int ctn 	= Nodes - Vertex.Length;
			
			int index	= Vertex.Length;
			
			Array.Resize(ref Vertex, Nodes);
			
			if(ctn > 0)
				{
				for(int i=0; i<ctn; i++)
				Vertex[i+index] 	= new Vector3(0,0,0);
				}
			
			}
		}
	
	}