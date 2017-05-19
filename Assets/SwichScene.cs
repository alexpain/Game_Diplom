using System;
using BitBenderGames;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwichScene : MonoBehaviour
{
    
    private TouchInputController touchInputController;
    public GameObject apartment;
    public void Awake()
    {
        //apartment = FindObjectOfType<Camera>();
        Application.targetFrameRate = 60;
        touchInputController = apartment.GetComponent<TouchInputController>();
        

        touchInputController.OnFingerDown += OnFingerDown;

    
    }
    private void OnFingerDown(Vector3 screenPosition)
    {
        Application.LoadLevel(1);

    }

}