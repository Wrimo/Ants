using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject colonyPrefab;
    [SerializeField]
    GameObject plantPrefab;
    // Start is called before the first frame update
    void Start()
    {
        placeColonies(colonyPrefab, PlayerPrefs.GetInt("Colony"));
        placeObjectRandomly(plantPrefab, PlayerPrefs.GetInt("Plants"));
    }

    private void placeColonies(GameObject colonies, int amount)
    {
        int pattern = PlayerPrefs.GetInt("SpawnPattern");
        int spread = PlayerPrefs.GetInt("Spread");
        if (pattern == 0)
        {
            placeObjectRandomly(colonies, amount);
        }
        else if (pattern == 1)
        {
            for (int i = 0; i < amount; i++)
            {
                float theta = (2 * Mathf.PI / amount) * i;
                float x = Mathf.Cos(theta) * spread;
                float y = Mathf.Sin(theta) * spread;

                GameObject a = GameObject.Instantiate(colonies) as GameObject;
                if (a.GetComponent<Team>() != null)
                {
                    a.GetComponent<Team>().TeamNum = i + 1;
                }
                a.transform.position = new Vector3(x, y, 0);
            }
        }
        else if (pattern == 2)
        {
            for (int i = 0; i < amount; i++)
            {
                int y = 0;
                int x = -30 + spread * i;
              
                GameObject a = GameObject.Instantiate(colonies) as GameObject;
                if (a.GetComponent<Team>() != null)
                {
                    a.GetComponent<Team>().TeamNum = i + 1;
                }
                a.transform.position = new Vector3(x, y, 0);
            }

        }
    }

    private void placeObjectRandomly(GameObject prefab, int amount)
    {
        Vector3 previousPos = new Vector3();
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-40, 40), UnityEngine.Random.Range(-24, 24));
            while (Vector3.Distance(pos, previousPos) < 2)
            {
                pos = new Vector3(UnityEngine.Random.Range(-40, 40), UnityEngine.Random.Range(-24, 24));
            }
            GameObject a = GameObject.Instantiate(prefab) as GameObject;
            if (a.GetComponent<Team>() != null)
            {
                a.GetComponent<Team>().TeamNum = i + 1;
            }
            a.transform.position = pos;
            previousPos = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
