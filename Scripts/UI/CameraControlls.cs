using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour
{
    Vector3 newPosition;
    bool canZoom;
    bool isMoving;

    public float zoom = 10F;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom > 1)
        {
            zoom -= 1;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < 50)
        {
            zoom += 1;
        }

        Camera cam = GetComponent<Camera>();
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos;

        if (Physics.Raycast(ray, out hit))
        {
            targetPos = hit.point;
        }
        cam.orthographicSize = zoom;

        float xMove = 0;
        float yMove = 0;
        const float move = 0.05f;
        if (Input.GetKey(KeyCode.A))
        {
            xMove -= move;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xMove += move;
        }
        if (Input.GetKey(KeyCode.W))
        {
            yMove += move;
        }
        if (Input.GetKey(KeyCode.S))
        {
            yMove -= move;
        }
        transform.position = new Vector3(transform.position.x + xMove, transform.position.y + yMove, -10);

    }

}
