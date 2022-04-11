using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairControlls : MonoBehaviour
{
    [SerializeField]
    GameObject fruitPrefab; 
    public CrosshairStates State; 
    Camera camera;
    SpriteRenderer spr;
    public bool CanClick; 
    void Start()
    {
        camera = Camera.main;
        spr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GoToMouse();
        SizeAdjust();
        StateManagement();
    }

    private void GoToMouse()
    {
        Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(position.x, position.y, 0);
    }
    private void StateManagement()
    {
        if (State == CrosshairStates.Off)
        {
            spr.color = Color.white;
            spr.enabled = false; 
        }
        else if (State == CrosshairStates.Fruit)
        {
            spr.color = new Color(1, 0.2895f, 0);
            spr.enabled = true;
        }
        else if(State == CrosshairStates.Boot)
        {
            spr.color = Color.black;
            spr.enabled = true;
        }
        boot();
    }
    private void boot()
    {
        if (Input.GetMouseButton(0) && State == CrosshairStates.Boot && CanClick)
        {
            gameObject.layer = 12;
        }
        else if (Input.GetMouseButton(0) && State == CrosshairStates.Fruit && CanClick)
        {
            spawnFruit();
        }
        else if (Input.GetMouseButton(1) && State == CrosshairStates.Fruit)
        {
            gameObject.layer = 13;
        }
        else
        {
            gameObject.layer = 0;
        }
    }
    private void spawnFruit()
    {
            GameObject a = GameObject.Instantiate(fruitPrefab);
            a.transform.position = new Vector3(UnityEngine.Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
                UnityEngine.Random.Range(transform.position.y - transform.localScale.x / 2, transform.position.y + transform.localScale.x / 2));
    }
    private void SizeAdjust()
    {
        if(Input.GetKey(KeyCode.LeftArrow) && transform.localScale.x > 5)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.05f, transform.localScale.y - 0.05f);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && transform.localScale.x < 25)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f);
        }
    }
}
public enum CrosshairStates
{
    Off,
    Fruit, 
    Boot
}
