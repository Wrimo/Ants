using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int TeamNum;
    public float Opacity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
        string colorName = "";
        if (TeamNum == 1)
        {
            spr.color = Color.black;
            colorName = "Black";
        }
        else if (TeamNum == 2)
        {
            spr.color = Color.blue;
            colorName = "Blue";
        }
        else if(TeamNum == 3)
        {
            spr.color = Color.red;
            colorName = "Red";
        }
        else if(TeamNum == 4)
        {
            spr.color = Color.green;
            colorName = "Green";
        }
        else if(TeamNum == 5)
        {
            spr.color = Color.grey;
            colorName = "Grey";
        }
        else if(TeamNum == 6)
        {
            spr.color = Color.cyan;
            colorName = "Cyan";
        }
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, Opacity);
        if (gameObject.name.Contains("Colony"))
        {
            gameObject.GetComponentInChildren<HiveCollector>().Color = colorName;
        }
    }

    // Update is called once per frame
    void Update()
    {
   
    }
}
