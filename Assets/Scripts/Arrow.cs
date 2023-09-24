using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { x, y, z };

class Arrow : MonoBehaviour
{
    // TODO: should be serializable private
    public Direction MyDirection;
    public Vector3 FarthestPosition;

    private Vector3 _initialPosition;

    private bool _isDragging = false;
    // TODO: A very bad naming and design, will remove it if use concrete moving 
    private float distance;
    
    // TODO: Better be a static or put into a singleton
    private GameObject[] allArrows;

    private Vector3 _directionVector;

    private void Start()
    {
        _initialPosition = gameObject.transform.position;
        allArrows = GameObject.FindGameObjectsWithTag("Arrow");
        switch (MyDirection) {
            case Direction.x:
                _directionVector = new Vector3(1, 0, 0);
                break;
            case Direction.y:
                _directionVector = new Vector3(0, 1, 0);
                break;
            case Direction.z:
                _directionVector = new Vector3(0, 0, 1);
                break;
        }
        SetAxisLimit();
    }

    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        _isDragging = true;
        foreach(GameObject arrow in allArrows)
        {
            if (arrow != gameObject)
                arrow.SetActive(false);
        }
        
    }

    void OnMouseUp()
    {
        _isDragging = false;
        if (gameObject.transform.position.Equals(_initialPosition))
        {
            foreach (GameObject arrow in allArrows)
            {
                arrow.SetActive(true);
            }
        }

        // TODO: will these look more proper using events/callback? not a high prior though
        SetAxisLimit();
        CubeManager.Instance.AdjustCubesVisibility();
    }

    void Update()
    {
        if (_isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            Vector3 movement = rayPoint - transform.position;
            Vector3 distPosition = transform.position + Vector3.Project(movement, _directionVector);

            if (MyDirection == Direction.x)
                distPosition.x = Mathf.Clamp(distPosition.x, _initialPosition.x, FarthestPosition.x);
            else if(MyDirection == Direction.z)
                distPosition.z = Mathf.Clamp(distPosition.z, FarthestPosition.z, _initialPosition.z);

            transform.position = distPosition;
        }
    }

    private void SetAxisLimit()
    {
        switch (MyDirection)
        {
            case Direction.x:
                CubeManager.Instance.XAxisLimit = transform.position.x;
                break;
            case Direction.z:
                CubeManager.Instance.ZAxisLimit = transform.position.z;
                break;
        }
    }
}