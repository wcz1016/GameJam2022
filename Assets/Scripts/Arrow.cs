using UnityEngine;

public enum ArrowType { Red, Blue };

class Arrow : MonoBehaviour
{
    // TODO: should be serializable private
    public ArrowType ArrowType;
    public Transform NearEndMarker;
    public Transform FarEndMarker;

    private bool _isDragging = false;
    private Vector3 _lastMousePosition;

    private float _rayCastDistance;
    
    private Vector3 _positiveDirection;
    
    // TODO: A very bad design, better be a static or put into a singleton
    private GameObject[] _allArrows;

    private void Start()
    {
        _allArrows = GameObject.FindGameObjectsWithTag("Arrow");
        SetAxisLimit();
    }

    void OnMouseDown()
    {
        _rayCastDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        _isDragging = true;

        foreach (GameObject arrow in _allArrows)
        {
            if (arrow != gameObject)
                arrow.SetActive(false);
        }

        switch (ArrowType)
        {
            case ArrowType.Red:
                _positiveDirection = transform.parent.right;
                break;
            case ArrowType.Blue:
                _positiveDirection = -transform.parent.forward;
                break;
        }
    }

    void OnMouseUp()
    {
        _isDragging = false;
        // If we can clip and use discrete maker/index, we can use index as condition instead of distance
        if (Vector3.Distance(transform.position, NearEndMarker.position) < 0.01f)
        {
            foreach (GameObject arrow in _allArrows)
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
        if (_isDragging && Vector3.Distance(_lastMousePosition, Input.mousePosition) > 0.01f)
        {
            // Maybe not the right way to just use distance for raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(_rayCastDistance);
            Vector3 movementAlongDirection = Vector3.Project(rayPoint - transform.position, _positiveDirection);

            transform.position = transform.position + movementAlongDirection;

            // Clamp
            Vector3 locPosition = transform.localPosition;
            switch(ArrowType)
            {
                case ArrowType.Red:
                    locPosition.x = Mathf.Clamp(locPosition.x, NearEndMarker.localPosition.x, FarEndMarker.localPosition.x);
                    break;
                case ArrowType.Blue:
                    locPosition.z = Mathf.Clamp(locPosition.z, FarEndMarker.localPosition.z, NearEndMarker.localPosition.z);
                    break;

            }
            transform.localPosition = locPosition;

            _lastMousePosition = Input.mousePosition;
        }
    }

    private void SetAxisLimit()
    {
        switch (ArrowType)
        {
            case ArrowType.Red:
                CubeManager.Instance.XAxisLimit = transform.localPosition.x;
                break;
            case ArrowType.Blue:
                CubeManager.Instance.ZAxisLimit = transform.localPosition.z;
                break;
        }
    }
}