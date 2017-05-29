using UnityEngine;
using System.Collections;

/// <summary>
/// Semaphore. - Semaphore Manager
/// </summary>

public class Semaphore : MonoBehaviour 
{
	private int		DirUpdate;
	private SemiUnit[] List;
	public bool		Direction1 = true;
	public bool		Direction2 = true;
	public bool		Direction3 = true;
	public bool		Direction4 = true;
	public float 	TimeSema;		// Light switch standard time (all semaphores)
	
			float 	TimeAtack;		// Light switch gameplay (all semaphores)
			float 	SingleTime;		// Light switch standard time single semaphore (unity)
			bool  	Yelow;
	
			int 	Qtd_Semaphoro;	// Number of semaphores managers in the scene
			int		Qtd_Fake_Semaphoro;
	private SemiUnit[] semaf;
			int 	At_Semaphoro;
	
			bool	Fixed;

	public bool NeedUpdate()
	{
		int CheckUpdate = 0;
		if(Direction1) CheckUpdate +=1;
		if(Direction2) CheckUpdate +=2;
		if(Direction3) CheckUpdate +=4;
		if(Direction4) CheckUpdate +=8;
		if(CheckUpdate != DirUpdate)
		{
			DirUpdate = CheckUpdate;
			return true;
		}
		else return false;
	}

	public	bool ReturnState()
	{
		return Fixed;
	}
	
	public	void ChangeFixed(bool fil)
	{
		Fixed = fil;
	}
	
	// Use this for initialization
	void Start () 
	{
		Fixed	= false;
		
		if(TimeSema == 0.0f)
		{
			TimeSema = 4.5f;
		}
		
		SingleTime = TimeSema / 4;
			
		TimeAtack = 0.01f;
		
		semaf = gameObject.GetComponentsInChildren<SemiUnit>();

		foreach (SemiUnit sm in semaf) 
		{
			sm.transform.gameObject.SetActive(true);
			sm.SemaControl = gameObject.name;
			if(int.Parse(sm.name.Remove (0,5)) == 1 && Direction1 == false)
			{
				sm.transform.gameObject.SetActive(false);
			}
			if(int.Parse(sm.name.Remove (0,5)) == 2 && Direction2 == false)
			{
				sm.transform.gameObject.SetActive(false);
			}
			if(int.Parse(sm.name.Remove (0,5)) == 3 && Direction3 == false)
			{
				sm.transform.gameObject.SetActive(false);
			}
			if(int.Parse(sm.name.Remove (0,5)) == 4 && Direction4 == false)
			{
				sm.transform.gameObject.SetActive(false);
			}
		}
		semaf = gameObject.GetComponentsInChildren<SemiUnit>();
		Qtd_Semaphoro = semaf.Length;
		Qtd_Fake_Semaphoro = Mathf.Max (Qtd_Semaphoro, 2);
		At_Semaphoro = Random.Range(0,Qtd_Semaphoro - 1);
		}
	
	// Update is called once per frame
	void Update () 
	{
		if(TimeAtack <= SingleTime && At_Semaphoro < Qtd_Semaphoro)
		{
			semaf[At_Semaphoro].Take_Control(1);
		}
		
		

		TimeAtack -= Time.deltaTime;
		if(TimeAtack <= 0.0f)
		{
			TimeAtack = TimeSema;
			
			At_Semaphoro++;
			
			if(At_Semaphoro >= Qtd_Fake_Semaphoro) { At_Semaphoro = 0; }
			
			
			for(int i=0; i<=Qtd_Fake_Semaphoro-1; i++)
			{
				Debug.Log (i);
				if(i == At_Semaphoro)
				{
					if(i < Qtd_Semaphoro)
					{
						semaf[i].Take_Control(0);
					}
				}
				else
				{
					if(i < Qtd_Semaphoro)
					{
						semaf[i].Take_Control(2);
					}
				}
			}
			
		}
	}
	
}
