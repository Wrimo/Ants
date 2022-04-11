using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    int TimeToWinter = 10000;
    [SerializeField]
    bool useWinter = true;
    public bool Winter;
    int timer = 0;
    void Start()
    {
        if (PlayerPrefs.GetInt("Winter") == 1)
        {
            useWinter = true;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)
        {
            transform.parent = null; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (useWinter)
            CheckWinter();
    }
    private void CheckWinter()
    {
        int waitTime = TimeToWinter * UnityEngine.Random.Range(2, 6);
        if(Winter)
        {
            waitTime /= UnityEngine.Random.Range(2, 4);
        }
        if (timer >= waitTime)
        {
            timer = 0;
            Winter = !Winter;
        }
        else
        {
            timer++;
        }
    }
}
