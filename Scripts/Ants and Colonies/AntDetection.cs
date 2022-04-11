using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntDetection : MonoBehaviour
{

    public int EnemiesNearby;
    public int AlliesNearby;
    public int ObjectNearby;
    public GameObject closestObject;
    public GameObject closestEnemy;
    float closestObjDist;
    float closestEnemyDist;

    int team;

    void Start()
    {
        closestObjDist = 9999;
        closestEnemyDist = 9999;
        EnemiesNearby = 0;
        AlliesNearby = 0;
        ObjectNearby = 0;
        team = gameObject.GetComponentInParent<Team>().TeamNum;
        gameObject.GetComponent<CircleCollider2D>().radius = gameObject.GetComponentInParent<AntController>().detectionRadius;
    }
    void Update()
    {
        if (transform.parent == null)
        {
            GameObject.Destroy(this.gameObject);
        }
        if(closestObject == null)
        {
            closestObjDist = 9999;
        }
        if(closestEnemy == null)
        {
            closestEnemyDist = 9999;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.gameObject.layer == 9)
        {
            if (col.gameObject.GetComponent<Team>() == null)
            {
                ObjectNearby++;
                compareDistances(col.gameObject, ref closestObject, ref closestObjDist);

            }
            else if (col.gameObject.GetComponent<Team>().TeamNum == team)
            {
                ObjectNearby++;
                compareDistances(col.gameObject, ref closestObject, ref closestObjDist);

            }
        }
        if (col.gameObject.layer == 10)
        {
            if (col.gameObject.GetComponent<Team>().TeamNum == team && col.transform != transform.parent)
            {
                AlliesNearby++;
            }
            else if (col.transform != transform.parent)
            {
                EnemiesNearby++;
                compareDistances(col.gameObject, ref closestEnemy, ref closestEnemyDist);
            }

        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            if (col.gameObject.GetComponent<Team>() == null)
            {
                ObjectNearby--;
            }
            else if (col.gameObject.GetComponent<Team>().TeamNum == team)
            {
                ObjectNearby--;
            }
        }
        if (col.gameObject.layer == 10)
        {
            if (col.gameObject.GetComponent<Team>().TeamNum == team)
            {
                AlliesNearby--;
            }
            else
            {
                EnemiesNearby--;
            }

        }
    }

    private void compareDistances(GameObject col, ref GameObject globalObject, ref float floatDist)
    {
        float dist = Vector2.Distance(col.transform.position, transform.position);
        if (dist < floatDist)
        {
            if(col.gameObject.transform.parent == null)
            {
                floatDist = dist;
                globalObject = col.gameObject;
            }
            
        }
    }
}
