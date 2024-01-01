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
}
