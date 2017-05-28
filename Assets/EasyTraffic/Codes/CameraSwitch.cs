using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour 
	{
	
	public	Camera[]	Cam;
		
			int			CountCamera;
	
			int			Switch;
	
	public	float		TimeSwitch;
	
			float		DownTime;

	// Use this for initialization
	void Start () 
		{
		if(TimeSwitch == 0.0f) {TimeSwitch = 5.0f; }
		
		DownTime	= TimeSwitch;
		
		CountCamera = Cam.Length;
		
		Switch		= 0;
		
		if(CountCamera > 0)
			{
			Cam[0].enabled = true;
			
			for(int i=1; i<CountCamera; i++)
				{
				Cam[i].enabled = false;
				}
			}
		}
	
	// Update is called once per frame
	void Update () 
		{
		if(CountCamera > 0)
			{
			DownTime -= Time.deltaTime;
			
			if(DownTime <= 0.0f)
				{
				Cam[Switch].enabled = false;
				
				Switch++;
				
				if(Switch > CountCamera-1) { Switch = 0; }
				
				Cam[Switch].enabled = true;
				
				DownTime = TimeSwitch;
				}
			}
		}
	
	void OnGUI()
		{
		if(CountCamera > 0)
			{
			Rect muda = new Rect(10, 10,100,40);
			
			GUI.TextArea(muda,"Camera: "+(Switch+1));
			}
		}
	
	}