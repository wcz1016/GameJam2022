using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { x, y, z };

class Arrow : MonoBehaviour
{
  
    public Direction direction;
    public Vector3 farthestPostion;
    private Vector3 initialPosition;

    private bool dragging = false;
    private float distance;
    
    private GameObject[] allArrows;
    private Vector3 directionVector;

    private void Start()
    {
        initialPosition = gameObject.transform.position;
        allArrows = GameObject.FindGameObjectsWithTag("Arrow");
        switch (direction) {
            case Direction.x:
                directionVector = new Vector3(1, 0, 0);
                break;
            case Direction.y:
                directionVector = new Vector3(0, 1, 0);
                break;
            case Direction.z:
                directionVector = new Vector3(0, 0, 1);
                break;
        }
        setAxisLimit();
    }

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
        foreach(GameObject arrow in allArrows)
        {
            if (arrow != gameObject)
                arrow.SetActive(false);
        }
        
    }

    void OnMouseUp()
    {
        dragging = false;
        if (gameObject.transform.position.Equals(initialPosition))
        {
            foreach (GameObject arrow in allArrows)
            {
                arrow.SetActive(true);
            }
        }
        setAxisLimit();
        CubeManager.Instance.adjustCubesVisibility();
    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            Vector3 moveMent = rayPoint - transform.position;
            Vector3 distPostion = transform.position + Vector3.Project(moveMent, directionVector);

            if (isFartherThan(distPostion, farthestPostion))
                transform.position = farthestPostion;
            else if (isFartherThan(initialPosition, distPostion))
                transform.position = initialPosition;
            else
                transform.position = distPostion;
        }
    }

    // 因为实在不知道怎么描述，暂定离镜头远的方向为 far
    private bool isFartherThan(Vector3 pos1, Vector3 pos2)
    {
        switch (direction)
        {
            case Direction.x:
                return pos1.x > pos2.x;
            case Direction.z:
                return pos1.z < pos2.z;
            default: return false;
        }
    }

    private void setAxisLimit()
    {
        switch (direction)
        {
            case Direction.x:
                CubeManager.Instance.xAxisLimit = transform.position.x;
                break;
            case Direction.z:
                CubeManager.Instance.zAxisLimit = transform.position.z;
                break;
        }
    }
}