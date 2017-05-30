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
        ListUI[5].gameObject.SetActive(status);
        ListUI[4].gameObject.SetActive(status);
        ListUI[3].gameObject.SetActive(status);
        ListUI[2].gameObject.SetActive(status);
    }

    public static void GetPanel()
    {
        ListUI[1].gameObject.SetActive(true);
    }
    public static void GetPanelFail()
    {
        ListUI[0].gameObject.SetActive(true);
    }
    public static void HidePanelFail()
    {
        ListUI[5].gameObject.SetActive(false);
        ListUI[4].gameObject.SetActive(false);
        ListUI[3].gameObject.SetActive(false);
        ListUI[2].gameObject.SetActive(false);
        ListUI[1].gameObject.SetActive(false);
        ListUI[0].gameObject.SetActive(false);
    }
}
