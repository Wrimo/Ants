using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitItself : MonoBehaviour
{
    // Start is called before the first frame update
    public Fruit reff;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (reff != null)
        {
            if (reff.Winter && transform.parent == null)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 13)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
