using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public  Text pointsText;
    private static int point;

    public int Point
    {
        get { return point; }
        set { point += value; }
    }

	void Start ()
	{
        Point = PlayerPrefs.GetInt("point");
        pointsText = GetComponent<Text>();
        pointsText.text = Point.ToString();
	}

    public void SavePoint()
    {
        PlayerPrefs.SetInt("point",point);
    }
    
}
