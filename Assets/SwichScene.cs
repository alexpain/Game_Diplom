
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


    public class SwichScene : MonoBehaviour
    {
    void OnMouseDown()
    {
        RaycastHit s = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2 oldPosition = new Vector2(transform.localPosition.x, transform.localPosition.z);
        Debug.Log("RayCast On");
        Application.LoadLevel(1);
    }
}
