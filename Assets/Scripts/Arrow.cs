using UnityEngine;

public enum ArrowType { Red, Blue };

class Arrow : MonoBehaviour
{
    // TODO: should be serializable private
    public ArrowType ArrowType;
    public Transform NearEndMarker;
    public Transform FarEndMarker;
    public int Index { get => _index; }

    private bool _isDragging = false;
    private Vector3 _lastMousePosition;

    private float _rayCastDistance;
    
    private Vector3 _positiveDirection;

    // maybe uint
    private int _index;
    private int _maxIndex;
    private float _cubeLength;
    private float _nearEndConcernedTransformValueAbs;
    private Vector3 _lastArrowPosition;

    private void Start()
    {
        switch (ArrowType)
        {
            case ArrowType.Red:
                _positiveDirection = -transform.parent.right;
                break;
            case ArrowType.Blue:
                _positiveDirection = transform.parent.forward;
                break;
        }
        _cubeLength = CubeManager.Instance.CubePrefab.gameObject.transform.localScale.x;
        _nearEndConcernedTransformValueAbs = Mathf.Abs(GetConcernedTransformValue(NearEndMarker.localPosition));
        _maxIndex = CubeManager.Instance.CubeNumber - 1;
        _index = _maxIndex;
    }

    public void Reset()
    {
        SetIndex(_maxIndex);
        ClipToCurrentIndex();
    }

    float GetConcernedTransformValue(Vector3 aVector)
    {
        switch (ArrowType)
        {
            case ArrowType.Red:
                return aVector.x;
            case ArrowType.Blue:
                return aVector.z;
            default:
                return 0.0f;
        }
    }

    void OnMouseDown()
    {
        if (Game.Instance.BlockInput)
            return;

        _rayCastDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        _isDragging = true;
        _lastArrowPosition = transform.position;
        CubeManager.Instance.SelectArrow(gameObject);
    }

    void OnMouseUp()
    {
        if (Game.Instance.BlockInput)
            return;

        _isDragging = false;
        if (_index == _maxIndex)
        {
            CubeManager.Instance.UnSelectArrow();
        }
        ClipToCurrentIndex();
    }

    void Update()
    {
        if (_isDragging && Vector3.Distance(_lastMousePosition, Input.mousePosition) > 0.01f)
        {
            _lastMousePosition = Input.mousePosition;
            // Maybe not the right way to just use distance for raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(_rayCastDistance);
            Vector3 movementAlongDirection = Vector3.Project(rayPoint - _lastArrowPosition, _positiveDirection);

            if (movementAlongDirection.magnitude < CubeManager.Instance.ArrowMoveDeadZone)
                return;

            transform.position = _lastArrowPosition + movementAlongDirection;

            float concernedTransformValueAbs = Mathf.Abs(GetConcernedTransformValue(transform.localPosition));
            int index = Mathf.FloorToInt((concernedTransformValueAbs) / _cubeLength) - 1;
            
            if (GetConcernedTransformValue(transform.localPosition) * GetConcernedTransformValue(NearEndMarker.localPosition) < 0.0f || index < 0)
            {
                SetIndex(0);
                ClipToCurrentIndex();
            }
            else if (concernedTransformValueAbs > _nearEndConcernedTransformValueAbs)
            {
                SetIndex(_maxIndex);
                ClipToCurrentIndex();
            }
            else
            {
                SetIndex(index);
                // TODO: not really working
                if (IsCloseTo(concernedTransformValueAbs, (index) * _cubeLength))
                    ClipToCurrentIndex();
            }
        }
    }

    void SetIndex(int anIndex)
    {
        if (_index != anIndex)
        {
            _index = anIndex;
            CubeManager.Instance.AdjustCubesVisibility();
        } 
    }

    void ClipToCurrentIndex()
    {
        transform.localPosition = NearEndMarker.localPosition - (_maxIndex - _index) * _cubeLength * _positiveDirection;
    }

    bool IsCloseTo(float a, float b)
    {
        if (a >= b - CubeManager.Instance.ArrowMoveDeadZone && a <= b + CubeManager.Instance.ArrowMoveDeadZone)
            return true;
        else
            return false;
    }
}