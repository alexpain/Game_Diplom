using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = System.Random;

public class GenerateEvent : MonoBehaviour
{
    

    private static int a;

	public static int temp;

    void Start ()
    {
        temp = 0;

    }
	
	void Update ()
	{
        Random rand = new Random();
	    
	    if (temp == 0)
	    {
            temp = rand.Next(1000, 5000);
            Debug.Log(temp);
            
        }
        
        a++;
       
	    if (a > temp)
	    {
            Debug.Log(a);
	        a = 0;
	        temp = 0;
            SwichScene.evnt = 0;
            SwichScene.flag = true;
        }
	    
	}
}
