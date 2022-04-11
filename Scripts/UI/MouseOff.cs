using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOff : MonoBehaviour
{
    UIManager ui;
    void Start()
    {
        ui = Camera.main.GetComponent<UIManager>();
    }

    void OnMouseDown()
    { 
        ui.colonyData = null;

    }
}
