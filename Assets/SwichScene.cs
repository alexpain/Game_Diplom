﻿using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

public class SwichScene : MonoBehaviour
{
    public GameObject item;
    public Texture texture;
    public static List<GameObject> list;
    public static bool flag;
    public static int evnt;
    public static int fire;
    void Start()
    {
        if (list == null)
        {
            list = new List<GameObject>();
        }
        
        list.Add(item);
        evnt = 0;

    }

    void Update()
    {
        Random rand = new Random();

        //if (evnt == 0)
        //{
        //    evnt = rand.Next(0, list.Count);
        //    Debug.Log(evnt);
        //}
        if (flag && list[fire] == item)
        {
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        
    }
    //TODO Нужно потом выпилить рандом отсюда
    void OnMouseDown()
    {
        Random rand = new Random();
        if (evnt == 0)
        {
            evnt = rand.Next(0, list.Count);
            Debug.Log(evnt);
        }
        Debug.Log(flag);

        if (list[fire] == item)
        {
            Debug.Log(fire);
        }
        if (flag && list[fire] == item)
        {
            flag = false;
            Application.LoadLevel(1);
        }
    }
}
