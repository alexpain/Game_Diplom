using UnityEngine;
using System.Collections;

/// <summary>
/// Lights. - Semaphore lights code
/// </summary> 

public class Lights : MonoBehaviour 
	{
	public bool Green;		// Green light control
	public bool Red;		// Red light control
	public bool Yelow;		// Yelow light control
	
	public int TypeLight;	// Current semaphore light

	// Use this for initialization
	void Start () 
		{
		if(Green)
			{
			Red			= false;
			Yelow		= false;
			TypeLight	= 0;
			}
		else if(Red)
			{
			Green		= false;
			Yelow		= false;
			TypeLight	= 2;
			}
		else
			{
			Red			= false;
			Green		= false;
			TypeLight	= 1;
			}
		}
	
	// Update is called once per frame
	void Update () 
		{
		
		}
	
	}
