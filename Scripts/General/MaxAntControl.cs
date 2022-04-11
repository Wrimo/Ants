using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAntControl : MonoBehaviour
{
    public int MaxAnts;
    public int CurrentAnts; 
    void Start()
    {
        MaxAnts = PlayerPrefs.GetInt("MaxAnts");
    }
}
