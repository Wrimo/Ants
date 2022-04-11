using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AntController : MonoBehaviour
{
    [SerializeField]
    GameObject corpsePrefab;
    [SerializeField]
    Sprite box;
    [SerializeField]
    Sprite circle; 

    [SerializeField]
    Vector3 target;


    Vector3 hivePosition;
    public GameObject hive;
    public States AntState;

    float startTime;
    float speed;
    public float detectionRadius;
    float MaxEnergy;
    float curEnergy;

    [SerializeField]
    bool needsToRest;
    bool goback;
    bool permissionToRest; 
    Vector3 backLoc;
    Vector3 moveDir;
    bool isCarrying;
    public bool hasTarget;
    bool atBase;
    SpriteRenderer spr;
    int team;
    float attackPow;
    GameObject enemyRef;
    AntDetection detection;
    HiveCollector hiveCollector;
    MaxAntControl maxAnt;
    void Start()
    {
        detection = gameObject.GetComponentInChildren<AntDetection>();
        team = gameObject.GetComponent<Team>().TeamNum;
        spr = gameObject.GetComponent<SpriteRenderer>();
        hiveCollector = hive.GetComponent<HiveCollector>();
        maxAnt = Camera.main.GetComponent<MaxAntControl>();

        MaxEnergy = UnityEngine.Random.Range(PlayerPrefs.GetFloat("Energy"), 400 + PlayerPrefs.GetFloat("Energy"));
        speed = UnityEngine.Random.Range(PlayerPrefs.GetFloat("Speed"), 4 + PlayerPrefs.GetFloat("Speed"));
        detectionRadius = UnityEngine.Random.Range(10f, 15f);
        attackPow = UnityEngine.Random.Range(PlayerPrefs.GetFloat("Strength"), 0.5f + PlayerPrefs.GetFloat("Strength"));

        needsToRest = false;
        curEnergy = MaxEnergy;
        goback = false;
        startTime = Time.time;
        hivePosition = hive.transform.position;
        AntState = States.Exploring;
        isCarrying = false;
        hasTarget = false;
        permissionToRest = false; 

        NewTarget();

    }


    void Update()
    {
        Movement();
        ScanArea();
        EnergyManagement();
        Combat();
    }
    private void EnergyManagement()
    {
        CheckEnergy();
        EnergyDecay();
        Rest();
    }
    private void Movement()
    {
        Move();
        CheckToReturn();
        MoveToBase();
        checkCarrying(); 
    }
    private void Combat()
    {
        readjustAim();
        shouldRetreat();
        combatVisual();
    }
    private void ScanArea()
    {
        CheckSurroundings();
        IsThereAnything();
    }
    //We want to continually being at the correct position of the enemy rather than where they were previously. This method takes care of that. 
    private void readjustAim()
    {
        if (AntState == States.Attacking)
        {
            if (enemyRef == null)
            {
                NewTarget();
                return;
            }
            target = enemyRef.transform.position;
            AdjustDirection();
        }
    }

    //Changes the ant to a circle when attacking
    private void combatVisual()
    {
        if (AntState == States.Attacking)
            spr.sprite = circle;
        else
            spr.sprite = box; 
    }

    //The rather fun logic to determine if the ant should retreat from combat or keep fighting. 
    int timer = 0;
    private void shouldRetreat()
    {
        if (AntState == States.Attacking)
        {
            int retreatChance = 0;
            retreatChance += detection.AlliesNearby - detection.EnemiesNearby;
            if (enemyRef.GetComponent<AntController>().curEnergy < curEnergy)
            {
                retreatChance += 4;
            }
            if (curEnergy < MaxEnergy / 2)
                retreatChance -= 5;
            retreatChance += Mathf.RoundToInt(attackPow - enemyRef.GetComponent<AntController>().attackPow);

            if (retreatChance <= -5)
            {
                goRest();
            }
            else if (retreatChance <= 5 && timer == 15)
            {
                int ran = UnityEngine.Random.Range(1, 500 + retreatChance * 10);
                if (ran == 1)
                {
                    goRest();
                }
                timer = 0;
            }
            else
            {
                timer++;
            }

        }
    }
    //currently trying to rework this method to use a child collider
    private void CheckSurroundings()
    {
        //This method use OverlapCircle to check the area around an ant for any objects to interact with
        //Currently only works with collectible objects, but I'm hoping to add detecting ants from other colonies
        if (!isCarrying && !hasTarget)
        {

            if (detection.EnemiesNearby > 0 && detection.closestEnemy != null)
            {

                target = detection.closestEnemy.transform.position;
                enemyRef = detection.closestEnemy;
                AdjustDirection();
                hasTarget = true;
                AntState = States.Attacking;
            }
            else if (detection.ObjectNearby > 0 && detection.closestObject != null)
            {
                AntState = States.Collecting;
                target = detection.closestObject.transform.position;
                AdjustDirection();
                hasTarget = true;
            }
        }
    }

    //This method moves the ant to a new location if it reaches it target and finds it missing (ie another ant has already picked up the targetted fruit)
    private void IsThereAnything()
    {
        if (AntState == States.Collecting)
        {
            AdjustDirection();
            if (Vector2.Distance(transform.position, target) <= 0.1f && transform.childCount == 1)
            {
                hasTarget = false;
                NewTarget();
            }
        }

    }
    private void ReturnToBase()
    {
        AntState = States.Returning;
    }
    //A simple method that moves the ant back to base
    private void MoveToBase()
    {
        if (AntState == States.Returning)
        {
            target = new Vector3(hivePosition.x, hivePosition.y);
            AdjustDirection();
        }
    }

    //I'd have some problems with isCarrying, this should fix it
    private void checkCarrying()
    {
        if(transform.childCount == 1)
        {
            isCarrying = false; 
        }
    }

    //A method that ends autoexploring and has the ant return to base. 
    private void CheckToReturn()
    {

        if (AntState == States.Exploring && !atBase)
        {
            hasTarget = false;
            int val = UnityEngine.Random.Range(1, 1500 - Mathf.FloorToInt(Vector2.Distance(transform.position, target)));
            if (val == 1)
            {
                ReturnToBase();
            }
        }
    }

    //Controls energy and coloring. Kills objects if energy is too low. 
    private void EnergyDecay()
    {
        if (curEnergy <= 0)
        {
            Kill();
        }
        float aValue = curEnergy / MaxEnergy;
        if (aValue < 0.3f)
        {
            aValue = 0.3f;
        }
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, aValue);
    }

    //The code to kill the ant and spawn a corpse
    //Also drops anything the ant is carrying
    private void Kill()
    {
        transform.localScale = new Vector3(0.25f, 0.25f);
        GameObject c = GameObject.Instantiate(corpsePrefab) as GameObject;
        c.GetComponent<Team>().TeamNum = team;
        c.transform.position = this.transform.position;
        c.transform.localScale = new Vector3(0.09f, 0.09f);
        if (transform.childCount > 0)
        {
            transform.DetachChildren();
        }
        if(permissionToRest)
        {
            hiveCollector.incomingAntsToRest--; 
        }
        transform.position = new Vector3(-10000, -10000);
        StartCoroutine(reallyKill());
    }

    //This ienumerator is a hacky solution to get around Unity not detecting collider disappearing as opposed to moving out of anoher collider
    IEnumerator reallyKill()
    {
        yield return new WaitForEndOfFrame();
        maxAnt.CurrentAnts--; 
        hiveCollector.AntCount--; 
        GameObject.Destroy(this.gameObject);
    }

    //The simple logic that moves the ant
    //Also shows trajectory of movement in the scene view
    private void Move()
    {
        Debug.DrawRay(transform.position, moveDir);
        if (AntState != States.Resting)
        {
            transform.position += moveDir * speed * Time.deltaTime;
            curEnergy -= speed / 100;
        }

    }

    //This method picks a random direction for the ant to move in
    private void NewTarget()
    {
        target = new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f));
        AntState = States.Exploring;
        AdjustDirection();
    }

    //This method takes the target position and converts into a direction we can add to the current position 
    private void AdjustDirection()
    {
        moveDir = (target - transform.position).normalized;
    }

    //Checks if energy is low. If it is, return to base. 
    private void CheckEnergy()
    {
        if (curEnergy <= MaxEnergy / 2  && AntState != States.Resting && AntState != States.Attacking)
        {
            goRest();
        }
    }

    //This method adds the ant to the queue of ants that need to rest
    private void goRest()
    {
        if (!needsToRest)
        {
            hiveCollector.RestList.Enqueue(this.gameObject);
            needsToRest = true; 
        }
    }
    public void HeadHomeAndRest()
    {
        permissionToRest = true;
        AntState = States.Returning;
    }

    //This method takes care of recharging ants when they are resting. 
    private void Rest()
    {
        if (AntState == States.Resting)
        {
            if (curEnergy < MaxEnergy)
            {
                curEnergy += 2.5f;
            }
            else if (curEnergy >= MaxEnergy)
            {
                curEnergy = MaxEnergy;
                needsToRest = false;
                GoBackOnReturn();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //Layer 8 is the hive
        if (col.gameObject.layer == 8 && col.gameObject.GetComponent<Team>().TeamNum == team)
        {
            atBase = true;
            if (AntState == States.Returning)
            {
                if (hiveCollector.FruitCount > 0 && permissionToRest)
                {
                    hiveCollector.FruitCount--;
                    hiveCollector.incomingAntsToRest--;
                    AntState = States.Resting;
                    permissionToRest = false;
                    needsToRest = false;
                }
                else
                {
                    GoBackOnReturn();
                }
          

            }

        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            if (col.gameObject.GetComponent<Team>().TeamNum == team)
                atBase = false;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 8 && col.gameObject.GetComponent<Team>().TeamNum == team)
        {
            if (hiveCollector.FruitCount > 0 && permissionToRest)
            {
                hiveCollector.FruitCount--;
                hiveCollector.incomingAntsToRest--;
                AntState = States.Resting;
                permissionToRest = false;
                needsToRest = false;
            }
            //Drop off anything the ant is carrying
            if (isCarrying && col.gameObject.GetComponent<Team>().TeamNum == team)
            {
                if (transform.childCount != 1)
                {
                    transform.GetChild(1).transform.SetParent(hive.transform);
                }
                isCarrying = false;
            }
        }
        //Layer 9 is collectible suchs as fruit and corpses.
        else if (col.gameObject.layer == 9)
        {
            //Only pick up the fruit if the ant isn't already carrying something and the object isn't already being carried. 
            if (!isCarrying && col.gameObject.transform.parent == null)
            {
                if (col.gameObject.GetComponent<Team>() == null || col.gameObject.GetComponent<Team>().TeamNum == team)
                {
                    isCarrying = true;
                    ReturnToBase();
                    col.gameObject.transform.SetParent(this.gameObject.transform);
                    hasTarget = false;
                    CheckNeedToReturn();
                }
            }
        }
        else if (col.gameObject.layer == 10 && col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<Team>().TeamNum != team)
            {
                col.gameObject.GetComponent<AntController>().curEnergy -= UnityEngine.Random.Range(0.25f + attackPow, 1f + attackPow);
            }
        }
        else if (col.gameObject.layer == 12)
        {
            curEnergy -= MaxEnergy / 4; 
        }
    }
    //If we remember somewhere were food is, go back there
    private void GoBackOnReturn()
    {
        //If the ant doesn't know where anything is, move in a random direction.
        if (!goback)
        {
            NewTarget();
        }
        //But if it does remember where something is, go back there. 
        else if (goback)
        {
            target = backLoc;
            AntState = States.Collecting;
            goback = false;
            AdjustDirection();
        }
    }

    //Checks if there any other objects to pick up in area of last object picked up. 
    private void CheckNeedToReturn()
    {

        if (detection.ObjectNearby > 1)
        {
            goback = true;
            backLoc = transform.position;
        }
        else
        {
            goback = false;
        }
    }
}
