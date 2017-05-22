using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UI;
using Random = System.Random;

public class SwichScene : MonoBehaviour
{
    public GameObject item;
    public static List<GameObject> list;
    public static bool flag;
    public static int evnt;
    void Start()
    {
        if (list == null)
        {
            list = new List<GameObject>();
        }
        
        list.Add(item);
        evnt = 0;
        //Debug.Log(list.Count);
    }

    void Update()
    {
        Random rand = new Random();

        if (evnt == 0)
        {
            evnt = rand.Next(0, list.Count);
            Debug.Log(evnt);
        }
        if (flag && list[evnt] == item)
        {
            //item = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //item.renderer.material.mainTexture = Resources.Load(@"New Folder\Materials\road.mat");
        }
        
    }

    void OnMouseDown()
    {
        Debug.Log(flag);
        if (list[evnt] == item)
        {
            Debug.Log("asdasdasdasdasdasddddddddddddddddddddddddddd");
        }
        if (flag && list[evnt] == item)
        {
            flag = false;
            Application.LoadLevel(1);
        }
    }
}
