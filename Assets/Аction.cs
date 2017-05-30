using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Аction : MonoBehaviour {

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
        MyUI.HidePanelFail();
    }


}
