using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private static CubeManager _instance;

    public static CubeManager Instance { get { return _instance; } }

    public float ZAxisLimit;

    public float XAxisLimit;

    private List<GameObject> _allCubes;

    private List<Vector2Int> leftSelectedIndexes;

    private List<Vector2Int> rightSelectedIndexes;

    [SerializeField]
    private GameObject _cubePrefab;
    public GameObject CubePrefab => _cubePrefab;

    public GameObject LeftArrowPrefab;
    public GameObject RightArrowPrefab;

    private GameObject _leftArrow;
    private GameObject _rightArrow;

    [SerializeField]
    private float _rotationSpeed;

    public HashSet<Vector2Int> leftAnswerIndexes;

    public HashSet<Vector2Int> rightAnswerIndexes;

    public int UsedNum;

    private bool _isTrackingMouse;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if (Game.Instance.BlockInput)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            _isTrackingMouse = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isTrackingMouse = false;
        }

        // TODO: Try apply smooth here
        if (_isTrackingMouse)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Rotate(transform.up, -h * _rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Camera.main.transform.right, v * _rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    public void Init()
    {
        InitCubes();
        InitArrows();
        leftSelectedIndexes = new List<Vector2Int>();
        rightSelectedIndexes = new List<Vector2Int>();
        leftAnswerIndexes = new HashSet<Vector2Int>();
        rightAnswerIndexes = new HashSet<Vector2Int>();
        // TODO: why? May for debugging?
        leftAnswerIndexes.Add(new Vector2Int(0,0));
        UsedNum = 0;
    }

    // Maybe move this to cube prefab, so they decide for themselves?
    public void AdjustCubesVisibility()
    {
        foreach(var cube in _allCubes)
        {
            if (cube.transform.localPosition.x < XAxisLimit || cube.transform.localPosition.z > ZAxisLimit)
                cube.SetActive(false);
            else
                cube.SetActive(true);
        }
    }

    public void AddLeftPos(Vector2Int pos)
    {
        leftSelectedIndexes.Add(pos);
    }

    public void AddRightPos(Vector2Int pos)
    {
        rightSelectedIndexes.Add(pos);
    }

    public void RemoveLeftPos(Vector2Int pos)
    {
        leftSelectedIndexes.Remove(pos);
    }
    
    public void RemoveRightPos(Vector2Int pos)
    {
        rightSelectedIndexes.Remove(pos);
    }

    public bool IsCorrect()
    {
        DebugPoss();
        return leftAnswerIndexes.SetEquals(new HashSet<Vector2Int>(leftSelectedIndexes))
            && rightAnswerIndexes.SetEquals(new HashSet<Vector2Int>(rightSelectedIndexes));
    }

    void DebugPoss()
    {
        Debug.Log("left");
        HashSet<Vector2Int> leftSelectedPostionSet = new HashSet<Vector2Int>(leftSelectedIndexes);
        foreach (Vector2Int vec in leftSelectedPostionSet)
        {
            Debug.Log(vec);
        }
        Debug.Log("right");
        HashSet<Vector2Int> rightSelectedPostionSet = new HashSet<Vector2Int>(rightSelectedIndexes);
        foreach (Vector2Int vec in rightSelectedPostionSet)
        {
            Debug.Log(vec);
        }
    }
    void DebugAnswers()
    {
        Debug.Log("left");
        foreach (Vector2Int vec in leftAnswerIndexes)
        {
            Debug.Log(vec);
        }
        Debug.Log("right");
    
        foreach (Vector2Int vec in rightAnswerIndexes)
        {
            Debug.Log(vec);
        }
    }

    public void InitArrows()
    {
        int cubeNumber = Game.Instance.CubeNumber;
        int halfCubeNum = cubeNumber / 2;
        Vector3 originPosition = Game.Instance.OriginPosition;
        Vector3 _cubePrefabScale = _cubePrefab.transform.localScale;

        // make arrows slightly higher
        float posY = originPosition.y + (halfCubeNum + 1) * _cubePrefabScale.y;

        // The far and near is related to camera
        float farthestX = originPosition.x + (halfCubeNum - 1) * _cubePrefabScale.x;
        float farthestZ = originPosition.z - (halfCubeNum - 1) * _cubePrefabScale.z;

        float nearestX = originPosition.x - (halfCubeNum + 1) * _cubePrefabScale.x;
        float nearestZ = originPosition.z + (halfCubeNum + 1) * _cubePrefabScale.z;

        Vector3 leftArrowPosition = new Vector3(farthestX + _cubePrefabScale.x, posY, nearestZ);
        Vector3 rightArrowPosition = new Vector3(nearestX, posY, farthestZ - _cubePrefabScale.x);

        // TODO: is this kind of Instantiate a gameobject -> init its script process right?
        // If so, maybe axis should also be inited here?
        _leftArrow = Instantiate(LeftArrowPrefab, leftArrowPosition, Quaternion.identity, gameObject.transform);
        _rightArrow = Instantiate(RightArrowPrefab, rightArrowPosition, Quaternion.identity, gameObject.transform);

        GameObject leftArrowNearEndMarker = new GameObject("LeftArrowNearEndMarker");
        leftArrowNearEndMarker.transform.position = _leftArrow.transform.position;
        leftArrowNearEndMarker.transform.SetParent(transform);

        GameObject rightArrowNearEndMarker = new GameObject("RightArrowNearEndMarker");
        rightArrowNearEndMarker.transform.position = _rightArrow.transform.position;
        rightArrowNearEndMarker.transform.SetParent(transform);

        GameObject farEndMarker = new GameObject("FarEndMarker");
        farEndMarker.transform.position = new Vector3(farthestX, posY, farthestZ);
        farEndMarker.transform.SetParent(transform);

        _leftArrow.GetComponent<Arrow>().NearEndMarker = leftArrowNearEndMarker.transform;
        _leftArrow.GetComponent<Arrow>().FarEndMarker = farEndMarker.transform;

        _rightArrow.GetComponent<Arrow>().NearEndMarker = rightArrowNearEndMarker.transform;
        _rightArrow.GetComponent<Arrow>().FarEndMarker = farEndMarker.transform;
    }

    public void ResetCubes()
    {
        foreach(GameObject cube in _allCubes)
        {
            cube.GetComponent<Cube>().UnselectSelf();
        }
        leftSelectedIndexes.Clear();
        rightSelectedIndexes.Clear();
        UsedNum = 0;
        UI.Instance.SetUsedNum();
    }

    public void InitCubes()
    {
        Vector3 originPosition = Game.Instance.OriginPosition;
        int cubeNumber = Game.Instance.CubeNumber;
        Vector3 _cubePrefabScale = _cubePrefab.transform.localScale;

        _allCubes = new List<GameObject>();

        for (int x = 0; x < cubeNumber; x++)
        {
            for (int y = 0; y < cubeNumber; y++)
            {
                for (int z = 0; z < cubeNumber; z++)
                {
                    Vector3 cubePosition = new Vector3(originPosition.x +(x - cubeNumber / 2) * _cubePrefabScale.x,
                        originPosition.y + (y - cubeNumber / 2) * _cubePrefabScale.y,
                        originPosition.z + (z - cubeNumber / 2) * _cubePrefabScale.z);

                    GameObject cube = Instantiate(_cubePrefab, cubePosition, Quaternion.identity, this.gameObject.transform);
                    _allCubes.Add(cube);
                    cube.GetComponent<Cube>().XIndex = x;
                    cube.GetComponent<Cube>().YIndex = y;
                    cube.GetComponent<Cube>().ZIndex = z;
                }
            }
        }
    }

    public void CheckIsCorrect()
    {
        Game.Instance.BlockInput = true;

        _leftArrow.SetActive(true); 
        _rightArrow.SetActive(true);
        _leftArrow.GetComponent<Arrow>().Reset();
        _rightArrow.GetComponent<Arrow>().Reset();
        AdjustCubesVisibility();
        
        transform.DORotateQuaternion(Quaternion.identity, 0.5f).OnComplete(() =>
        {
            Game.Instance.BlockInput = false;
            UI.Instance.InvokeCheckCubes(IsCorrect());
        });
    }
}
