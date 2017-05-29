using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class Useful
	{
	
	public class Vehicle_Data{
		public Vector3 	Position;
		public bool		Worng_Side;
		
		public Vehicle_Data() 
			{
			Position = new Vector3(0,0,0);
			}
		~Vehicle_Data() { }
		}

	// Use this for initialization
	void Start () 
		{
		
		}
	
	// Update is called once per frame
	void Update () 
		{
		
		}
	
	public Vector2 Perpendicular_Reta2d(Vector2 A,bool lado)
		{
		float troc = 0.0f;
		
		if(!lado)
			{
			troc = A.x;
			A.x = A.y * -1;
			A.y = troc;
			}
		else
			{
			troc = A.y;
			A.y = A.x * -1;
			A.x = troc;
			}
		
		return A;
		}
	
	public Vector2 Perpendicular_Line2d(Vector2 A,bool side)
		{
		float change = 0.0f;
		
		if(!side)
			{
			change 	= A.x;
			A.x 	= A.y * -1;
			A.y 	= change;
			}
		else
			{
			change 	= A.y;
			A.y 	= A.x * -1;
			A.x 	= change;
			}
		
		return A;
		}
	
	public Vector3 Vector(Vector3 A, Vector3 B)
		{
		A.x = B.x - A.x;
		A.y = B.y - A.y;
		A.z = B.z - A.z;
		
		return A;
		}
	
	public Vector2 Vector(Vector2 A, Vector2 B)
		{
		A.x = B.x - A.x;
		A.y = B.y - A.y;
		
		return A;
		}
	
	public float Natural_Number(float A)
		{
		if(A < 0) { A *= -1; }
		
		return A;
		}
	
	public Vector2 Perpendicular_Vector(Vector3 A, int Origin)
		{
		Vector2 reply = new Vector2(0,0);
		switch(Origin){
			case 0:{
				reply.x = A.z;
				reply.y = A.y;
				}break;
			
			case 1:{
				reply.x = A.x;
				reply.y = A.z;
				}break;
			
			case 2:{
				reply.x = A.x;
				reply.y = A.y;
				}break;
			}
		
		return reply;
		}
	
		/* Teorema de Pitagoras */
	public float Distance(Vector3 A, Vector3 B)
		{
		Vector3 R;
		
		R = Vector( A, B);
		
		float dis = Mathf.Sqrt( (R.x*R.x) + (R.y*R.y) + (R.z*R.z) );
		
		return dis;
		}
	
	public float Distance(Vector2 A, Vector2 B)
		{
		Vector3 Reply;
		
		Reply = Vector( A, B);
		
		float length = Mathf.Sqrt( (Reply.x * Reply.x) + (Reply.y * Reply.y) );
		
		return length;
		}
	
	
	public Vector3 Euler_Rotation(Vector3 Vector_Mod, Vector3 Axis, int Choice, float Angle)
		{
		Vector3 reply = new Vector3(0,0,0);
		
		if(Vector_Size(Axis) > 0.0f)
			{
			Vector_Mod.x -= Axis.x;
			Vector_Mod.y -= Axis.y;
			Vector_Mod.z -= Axis.z;
			}
		
		switch(Choice){
			case 0:{// Rotation in Axis X
				reply.x = Vector_Mod.x;
				reply.y = (Vector_Mod.y * Mathf.Cos(Angle)) - (Vector_Mod.z * Mathf.Sin(Angle));
				reply.z = (Vector_Mod.y * Mathf.Sin(Angle)) + (Vector_Mod.z * Mathf.Cos(Angle));
				}break;
			
			case 1:{// Rotation in Axis Y
				reply.x = (Vector_Mod.z * Mathf.Sin(Angle)) + (Vector_Mod.x * Mathf.Cos(Angle));
				reply.y = Vector_Mod.y;
				reply.z = (Vector_Mod.z * Mathf.Cos(Angle)) - (Vector_Mod.x * Mathf.Sin(Angle));
				}break;
			
			case 2:{// Rotation in Axis Z
				reply.x = (Vector_Mod.x * Mathf.Cos(Angle)) - (Vector_Mod.y * Mathf.Sin(Angle));
				reply.y = (Vector_Mod.x * Mathf.Sin(Angle)) + (Vector_Mod.y * Mathf.Cos(Angle));
				reply.z = Vector_Mod.z;
				}break;
			}
		
		if(Vector_Size(Axis) > 0.0f)
			{
			reply.x +=	Axis.x;
			reply.y += 	Axis.y;
			reply.z += 	Axis.z;
			}
		
		return reply;
		}
	
	public Vector3 Euler_Rotation(Vector3 Vector_Mod, int Choice, float Angle)
		{
		Vector3 reply = new Vector3(0,0,0);
		
		switch(Choice){
			case 0:{// Rotation in Axis X
				reply.x = Vector_Mod.x;
				reply.y = (Vector_Mod.y * Mathf.Cos(Angle)) - (Vector_Mod.z * Mathf.Sin(Angle));
				reply.z = (Vector_Mod.y * Mathf.Sin(Angle)) + (Vector_Mod.z * Mathf.Cos(Angle));
				}break;
			
			case 1:{// Rotation in Axis Y
				reply.x = (Vector_Mod.z * Mathf.Sin(Angle)) + (Vector_Mod.x * Mathf.Cos(Angle));
				reply.y = Vector_Mod.y;
				reply.z = (Vector_Mod.z * Mathf.Cos(Angle)) - (Vector_Mod.x * Mathf.Sin(Angle));
				}break;
			
			case 2:{// Rotation in Axis Z
				reply.x = (Vector_Mod.x * Mathf.Cos(Angle)) - (Vector_Mod.y * Mathf.Sin(Angle));
				reply.y = (Vector_Mod.x * Mathf.Sin(Angle)) + (Vector_Mod.y * Mathf.Cos(Angle));
				reply.z = Vector_Mod.z;
				}break;
			}
		
		return reply;
		}
	
	
	public Vector3 Vector_Addition(Vector3 V, Vector3 W)
		{
		Vector3 Reply = new Vector3(0,0,0);
		
		Reply.x = V.x + W.x;
		Reply.y = V.y + W.y;
		Reply.z = V.z + W.z;
		
		return Reply;
		}
	
	public Vector2 Vector_Addition(Vector2 V, Vector2 W)
		{
		Vector2 Reply = new Vector2(0,0);
		
		Reply.x = V.x + W.x;
		Reply.y = V.y + W.y;
		
		return Reply;
		}
	
	public Vector3 Vector_Addition(Vector3 V, float Sum)
		{
		Vector3 Reply = new Vector3(0,0,0);
		
		Reply.x = V.x + Sum;
		Reply.y = V.y + Sum;
		Reply.z = V.z + Sum;
		
		return Reply;
		}
	
	public Vector3 Vector_Product(Vector3 V, Vector3 W)
		{
		Vector3 Reply = new Vector3(0,0,0);
		
		Reply.x = (V.y * W.z) - (W.y * V.z);
		Reply.y = (W.x * V.z) - (W.z * V.x);
		Reply.z = (V.x * W.y) - (V.y * W.x);
		
		return Reply;
		}
	
	public float Vector_Size(Vector3 A)
		{
		Vector3 Length = new Vector3(0,0,0);
		
		return Distance(Length, A);
		}
	
	public float Vector_Size(Vector2 A)
		{
		float length = Mathf.Sqrt( (A.x*A.x) + (A.y*A.y) );
		
		return length;
		}
	
	public Vector3 Reduces_Vector(Vector3 A,float Pressure)
		{
		float Size = Vector_Size(A);
		
		A.x += (Pressure * (A.x / Size));
		A.y += (Pressure * (A.y / Size));
		A.z += (Pressure * (A.z / Size));
		
		return A;
		}
	
	
	public Vector3 Power_Vector(Vector3 A, float Pressure)
		{
		float Size = Vector_Size(A) + Pressure;
		
		A.x += A.x/Size;
		A.y += A.y/Size;
		A.z += A.z/Size;
		
		return A;
		}
	
	public Vector3 Adjust_Size(Vector3 A, float Size)
		{
		float T  = Vector_Size(A);
		float MT = Natural_Number( T - Size );
		Size = Size - (MT/T);
		
		A.x *= Size;
		A.y *= Size;
		A.z *= Size;
		
		return A;
		}
	
	public Vector2 Adjust_Size(Vector2 A, float Size)
		{
		A.x *= Size;
		A.y *= Size;
		
		return A;
		}
	
	public Vector3 Normalize_Vector(Vector3 A)
		{
		float Size = Vector_Size(A);
		
		A.x /= Size;
		A.y /= Size;
		A.z /= Size;
		
		return A;
		}
	
	
	public Vector3 Vectors_Multiplication(Vector3 V, Vector3 W)
		{
		Vector3 Reply = new Vector3(0,0,0);
		
		Reply.x = V.x * W.x;
		Reply.y = V.y * W.y;
		Reply.z = V.z * W.z;
		
		return Reply;
		}
	
	public Vector2 Vectors_Multiplication(Vector2 V, Vector2 W)
		{
		Vector2 Reply = new Vector2(0,0);
		
		Reply.x = V.x * W.x;
		Reply.y = V.y * W.y;
		
		return Reply;
		}
	
	public float Scalar_Product(Vector3 U, Vector3 V)
		{
		float r = (U.x * V.x) + (U.y * V.y) + (U.z * V.z);
		
		return r;
		}
	
	public float Scalar_Product(Vector2 U, Vector2 V)
		{
		float r = (U.x * V.x) + (U.y * V.y);
		
		return r;
		}
	
	public float AngleBetweenVectors( Vector3 A, Vector3 B) 
		{
		Vector2 u = Perpendicular_Vector(A,1);
		Vector2 v = Perpendicular_Vector(B,1);
		
		u = Vector(u,v);
		
		Vector2 zeta = v;
		v.y += 100; 
		v = Vector(v,zeta);
		
		float SclProduct = Scalar_Product(u, v);

		float x = SclProduct / ( Vector_Size(u) * Vector_Size(v) );
			
		float q = Mathf.Acos(x);
		
		
		q = q * Mathf.Rad2Deg;
		
		u = Perpendicular_Vector(A,1);
		v = Perpendicular_Vector(B,1);
		v.y += 100; 
		if( v.x > u.x ) { q *= -1; }
		
		return q;
		}
	
	public float AngleBetweenVectors( Vector2 A, Vector2 B)
		{
		Vector2 u = A;
		Vector2 v = B;
		
		u = Vector(u,v);
		
		Vector2 zeta = v;
		v.y += 100; 
		v = Vector(v,zeta);
		
		float SclProduct = Scalar_Product(u, v);

		float x = SclProduct / ( Vector_Size(u) * Vector_Size(v) );
			
		float q = Mathf.Acos(x); 
		
		q = q * Mathf.Rad2Deg; 
		
		u = A;
		v = B;
		v.y += 100; 
		if( v.x > u.x ) { q *= -1; }
		
		return q;
		}
	
	
	public float Turning_Aside(float A, float B)
		{
		int diedro1 = 0;
		int diedro2 = 0;
		
		if(A < 0) 			{ A = 360 + A; }
		else if(A > 360)	{ A = A - 360; }
		if(B < 0) 			{ B = 360 + B; }
		else if(B > 360)	{ B = B - 360; }
		
		if ((A >=0) && (A <=90)) 		{ diedro1 = 1; }
		else if ((A >90)  && (A <=180)) { diedro1 = 2; }
		else if ((A >180) && (A <=270)) { diedro1 = 3; }
		else        					{ diedro1 = 4; }
		
		if ((B >=0) && (B <=90)) 		{ diedro2 = 1; }
		else if ((B >90)  && (B <=180)) { diedro2 = 2; }
		else if ((B >180) && (B <=270)) { diedro2 = 3; }
		else        					{ diedro2 = 4; }
		
		float calc = Natural_Number(diedro2 - diedro1);
		
		if(calc <= 1) { calc = B - A; }
		else
			{
			if( diedro2 < diedro1 ) { calc = (360 + B) - A; }
			else 					{ calc = B - (360 + A); }
			}
		
		return calc;
		}
	
	
	public Vector3 LineEquation (Vector2 N1, Vector2 N2)
		{
		Vector3 reply = new Vector3(0,0,0);
		
		reply.x = N1.y - N2.y;
		reply.y = N2.x - N1.x;
		reply.z = (N1.x * N2.y) - (N2.x * N1.y);
		
		return reply;
		}
	
	
	public Vector2 Adjusts_Size(Vector2 A, Vector2 B, float Pressure)
		{
		Vector2 Vet = Vector(A,B);
		
		Vet = Adjust_Size(Vet, Pressure);
		
		B = Vector_Addition(A, Vet);
		
		return B;
		}
	
	
	public Vector2 Intersection_Point(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2)
		{
		A2 		= Adjusts_Size(A1,A2, 100);
		B2 		= Adjusts_Size(B1,B2, 100);
		
		Vector3 R1 = LineEquation(A1,A2);
		Vector3 R2 = LineEquation(B1,B2);
		
		
		float div = R1.y;
		float m1 = 1;
		float h1 = 1;
						
		m1 = -R1.x/div;
		h1 = -R1.z/div;
		
		div = R2.y;
		float m2 = 1;
		float h2 = 1;
			
		m2 = -R2.x/div;
		h2 = -R2.z/div; 
		
		Vector2 rps = new Vector2(0,0);
		
		rps.x =	( m1 + (m2 * -1) );
		
		rps.x = (h2 + (h1*-1)) / rps.x;
		rps.y = (m1 * rps.x) + h1;
		
		return rps;
		}
	
	public Vector3 Percentage_Vector(Vector3 A, float Percentage)
		{
		A.x = ( A.x * Percentage ) / 100;
		A.y = ( A.y * Percentage ) / 100;
		A.z = ( A.z * Percentage ) / 100;
		
		return A;
		}
	
	public Vector2 Percentage_Vector(Vector2 A, float Percentage)
		{
		A.x = ( A.x * Percentage ) / 100;
		A.y = ( A.y * Percentage ) / 100;
		
		return A;
		}
	
	public Vector3 Size_Subtracts(Vector3 A, float negative)
		{
		float Size = negative + 1;
		
		A.x = A.x / Size;
		A.y = A.y / Size;
		A.z = A.z / Size;
		
		return A;
		}
	
	public Vector3 Intensity_Vector(Vector3 A, float intensity)
		{
		A.x = A.x * intensity;
		A.y = A.y * intensity;
		A.z = A.z * intensity;
		
		return A;
		}
	
	public string Decimal_Time(float dec)
		{
		string reply = "";
		
		double minutes = dec / 60;
		
		minutes = Math.Truncate(minutes);
		
		double rest = (dec % 60);
		
		
		if(rest > 60)
			{
			minutes++;
			rest = rest - 60;
			}
		
		reply = rest.ToString();
		
		int i = 0;
		
		string sum = "";
		
		while (i < 2)
			{
			if(i < reply.Length )
				{
				if(( reply[i].ToString() == "." ) || ( reply[i].ToString() == "-" ))
					{
					sum = "0"+sum;
					i = 10;
					}
				else
					{
					sum = sum + reply[i];
					}
				}
			else
				{
				sum = "00";
				}
			i++;
			}
		
		reply = minutes.ToString()+" : "+sum;
		
		return reply;
		}
	
	
	public int Integer_Number(float Real)
		{
		int reply = (int) Math.Round (Real);
		
		
		return reply;
		}
	
	public string RealNumber(float number)
		{
		string a = number.ToString();
		string reply = "";
		
		int i = 0;
		int ctn = 0;
		
		while( i < a.Length)
			{
			if(ctn >= 2) { i = a.Length; }
			else
				{
				reply = reply+a[i];
				
				if((ctn > 0)||( a[i] == '.' ))
					{
					ctn++;
					}
				}
			
			i++;
			}
		
		return reply;
		}
	
	public	float	Magnetude(float Base, float Relation)
		{
		Base	=	Relation / Base;
		
		return	Base;
		}
	
	
	public float Produto_Escalar (Vector3 U, Vector3 V)
		{
		float r = (U.x * V.x) + (U.y * V.y) + (U.z * V.z);
		
		return r;
		}
	
	
	public float AnguloEntreVetores( Vector3 A, Vector3 B ) // Retorna o angulo entre dois vetores\pontos
		{
		float ProdEsc = Produto_Escalar(A, B);
		ProdEsc = ProdEsc / ( Vector_Size(A) * Vector_Size(B) );
			
		ProdEsc = Mathf.Acos(ProdEsc); // Recebe o angulo em Radianos
		float angulo = ProdEsc * Mathf.Rad2Deg; // Converte de Radianos para Graus
		
		return angulo;
		}
	
	}