using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverUI : MonoBehaviour
{
    [SerializeField]
    CrosshairControlls crosshair;
    void Start()
    {
        crosshair.CanClick = true; 
    }
    public void mouseEnter()
    { 
        crosshair.CanClick = false;
    }
    public void mouseExit()
    {
        crosshair.CanClick = true; 
    }
}
