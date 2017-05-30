using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUI : MonoBehaviour
{
    public GameObject Layout;
    public static List<GameObject> ListUI;
    void Start () {
        Layout.gameObject.SetActive(false);
        if (ListUI == null)
        {
            ListUI = new List<GameObject>();
        }

        ListUI.Add(Layout);
    }
	
    public static void SetActive(bool status)
    {
        foreach (var item in ListUI)
        {
            if (item.name.Contains("Button"))
            {
                item.gameObject.SetActive(status);
            }
        }
    }

    public static void GetPanel()
    {
        foreach (var item in ListUI)
        {
            if (item.name == "Panel")
            {
                item.gameObject.SetActive(true);
            }
        }
    }
    public static void GetPanelFail()
    {
        foreach (var item in ListUI)
        {
            if (item.name == "Panel (1)")
            {
                item.gameObject.SetActive(true);
            }
        }
    }
    public static void HidePanelFail()
    {
        foreach (var item in ListUI)
        {
            item.gameObject.SetActive(false);
        }
    }
}
