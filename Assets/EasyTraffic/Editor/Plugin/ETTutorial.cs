using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

public class ETTutorial : EditorWindow
{
	private	static		Texture2D tex = new Texture2D (1, 1, TextureFormat.RGBA32, false);
	static	string		Title;
	static	string		TitleBold;
	static	GUIStyle	ButtonStyle;
	static	GUIStyle	TextStyle;


// ---------- This Adds the Easy Traffic item on the Window menu 
	//[MenuItem ("Window/Easy Traffic Tutorial")]



// ---------- Here are the Initialization Functions for the Plugin Window and Traffic Editor Manager
	static	void Easy_Traffic()
	{
		ETTutorial window 	= (ETTutorial)EditorWindow.GetWindow (typeof (ETTutorial));
		window.title 		= "Easy Traffic";
		window.minSize		= new Vector2(200,500);
		window.maxSize		= new Vector2(200,500);
		window.Show();
	}

	void	Start()
	{
		tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		tex.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.85f));

		tex.Apply();
	}

	void	Initiate()
	{
	}


// ---------- Here Starts the Functions used by this Plugin on the Unity Editor
	

// ---------- Here Starts the GUI
	void	OnGUI()
	{
	}
		
	void OnInspectorUpdate()
	{
	}

	void Update()
	{
	}
}