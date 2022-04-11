using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject fruit;
    int timer;
    Fruit reff;
    void Start()
    {
        reff = Camera.main.GetComponent<Fruit>();
        SpawnFruit();
    }

    private void SpawnFruit()
    {
        int r = UnityEngine.Random.Range(1, 8);
        Vector3 startPos = transform.position;
        for (int i = 0; i < r; i++)
        {
            GameObject a = GameObject.Instantiate(fruit) as GameObject;
            a.GetComponent<FruitItself>().reff = reff;

            a.transform.position = new Vector3(startPos.x + UnityEngine.Random.Range(-1.0f, 1.0f), startPos.y + UnityEngine.Random.Range(-1.0f, 1.0f));
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!reff.Winter)
        {
            int val = UnityEngine.Random.Range(1, 10000 - timer);
            if (val == 1 || val == 2)
            {
                SpawnFruit();
                timer = 0;
            }
            else if (val == 3)
            {
                transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-3.0f, 3.0f), transform.position.y + UnityEngine.Random.Range(-3.0f, 3.0f));
            }
            else
            {
                timer++;
            }
        }
    }
}
