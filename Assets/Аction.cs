using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Аction : MonoBehaviour
{
    private static int maxPoint;
    void Start()
    {
        maxPoint = 100;
    }
    
    public void OnMouseDown()
    {
        Debug.Log("asdasd");
        MyUI.SetActive(true);

    }

    public void Back()
    {
        Application.LoadLevel(0);
    }

    public void SetАction()
    {
        Points points = new Points();
        points.Point = maxPoint;
        MyUI.SetActive(false);
        MyUI.GetPanel();
        
    }
    public void GetPanelFail()
    {
        MyUI.SetActive(false);
        MyUI.GetPanelFail();
    }

    public void HidePanelFail()
    {
        maxPoint = maxPoint - 10;
        MyUI.HidePanelFail();
    }


}
