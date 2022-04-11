using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveCollector : MonoBehaviour
{
    [SerializeField]
    GameObject antPrefab;
    [SerializeField]
    int startingAnts;

    public int incomingAntsToRest;
    public int team;
    public int FruitCount;
    public int TotalFruitCount;
    public int AntCount;
    public int CorpseCount;
    public int AntHigh;
    public string Color;
    public Queue<GameObject> RestList;
    UIManager ui;
    bool canMake = true;

    MaxAntControl antControl; 


    int spawnType;
    int maxAnts;

    void Start()
    {
        startingAnts = PlayerPrefs.GetInt("Ants");
        maxAnts = PlayerPrefs.GetInt("MaxAnts");
        spawnType = PlayerPrefs.GetInt("Spawn");

        ui = Camera.main.GetComponent<UIManager>();
        team = gameObject.GetComponentInParent<Team>().TeamNum;
        antPrefab.GetComponent<Team>().TeamNum = team;
        antControl = Camera.main.GetComponent<MaxAntControl>();

        RestList = new Queue<GameObject>();

        AntCount = 0;
        AntHigh = 0;
        FruitCount = 0;
        CorpseCount = 0;
        TotalFruitCount = 0;
        incomingAntsToRest = 0;

        StartCoroutine(createInitial());

    }
    IEnumerator createInitial()
    {
        for (int i = 0; i < startingAnts; i++)
        {
            createAnt();
            yield return new WaitForSeconds(0.01f);
        }
    }
    void Update()
    {
        adjustAntRecord();
        CheckToMakeAnt();
        callRest();
    }
    private void adjustAntRecord()
    {
        if (AntCount > AntHigh)
        {
            AntHigh = AntCount;
        }
    }
    private void callRest()
    {
        if (FruitCount > 2 + incomingAntsToRest && RestList.Count > 0)
        {
            GameObject ant = RestList.Dequeue();

            if (ant != null)
            {
                ant.GetComponent<AntController>().HeadHomeAndRest();
                incomingAntsToRest++;
            }
        }
    }
    public void createAnt()
    {
        if (antControl.CurrentAnts < antControl.MaxAnts)
        {
            GameObject a = GameObject.Instantiate(antPrefab) as GameObject;

            a.transform.position = this.transform.position;
            a.GetComponent<AntController>().hive = this.gameObject;
            a.name = "Ant -  " + team.ToString();
            a.GetComponent<Team>().TeamNum = team;
            AntCount++;
            antControl.CurrentAnts++; 
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 && col.gameObject.GetComponentInParent<Team>().TeamNum == team)
        {
            if (col.gameObject.tag == "Fruit")
            {
                TotalFruitCount += 2;
                FruitCount += 2;
                if (spawnType == 1)
                {
                    createAnt();
                }
            }
            else if (col.gameObject.tag == "Corpse")
            {
                CorpseCount++;
            }
            GameObject.Destroy(col.gameObject);
        }
    }
    private void CheckToMakeAnt()
    {
        if (spawnType == 0)
        {
            if (FruitCount > 3 && canMake && FruitCount > AntCount)
            {
                createAnt();
                FruitCount -= 3;
                canMake = false;
                StartCoroutine(reset());
            }
        }
    }
    IEnumerator reset()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 3.0f));
        canMake = true;
    }
    void OnMouseDown()
    {
        ui.colonyData = this;
    }
}
