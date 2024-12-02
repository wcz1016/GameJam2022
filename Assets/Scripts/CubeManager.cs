using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private static CubeManager _instance;

    public static CubeManager Instance { get { return _instance; } }

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
    private List<GameObject> _allArrows;

    private Vector3 _originPosition;
    [SerializeField]
    private int _cubeNumber;
    public int CubeNumber { get => _cubeNumber; }

    [SerializeField]
    private float _arrowMoveDeadZone;
    public float ArrowMoveDeadZone { get => _arrowMoveDeadZone; }

    public HashSet<Vector2Int> leftAnswerIndexes;

    public HashSet<Vector2Int> rightAnswerIndexes;

    public int UsedNum;

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

    private void Start()
    {
        _originPosition = gameObject.transform.position;
        Vector3 cubePrefabScale = _cubePrefab.transform.localScale;
        _originPosition.x += (_cubeNumber / 2) * cubePrefabScale.x;
        _originPosition.y += (_cubeNumber / 2) * cubePrefabScale.y;
        _originPosition.z -= (_cubeNumber / 2) * cubePrefabScale.z;
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
            if (cube.GetComponent<Cube>().XIndex > _rightArrow.GetComponent<Arrow>().Index
                || cube.GetComponent<Cube>().ZIndex > _leftArrow.GetComponent<Arrow>().Index)
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
        Vector3 cubePrefabScale = _cubePrefab.transform.localScale;

        // make arrows slightly higher
        float posY = _originPosition.y + cubePrefabScale.y;

        // The far and near is related to camera

        float nearestX = _originPosition.x - _cubeNumber * cubePrefabScale.x;
        float nearestZ = _originPosition.z + _cubeNumber * cubePrefabScale.z;

        Vector3 leftArrowPosition = new Vector3(_originPosition.x + cubePrefabScale.x, posY, nearestZ);
        Vector3 rightArrowPosition = new Vector3(nearestX, posY, _originPosition.z - cubePrefabScale.x);

        // TODO: is this kind of Instantiate a gameobject -> init its script process right?
        // If so, maybe axis should also be inited here?
        _leftArrow = Instantiate(LeftArrowPrefab, leftArrowPosition, Quaternion.identity, gameObject.transform);
        _rightArrow = Instantiate(RightArrowPrefab, rightArrowPosition, Quaternion.identity, gameObject.transform);

        GameObject farEndMarker = new GameObject("FarEndMarker");
        farEndMarker.transform.position = new Vector3(_originPosition.x, posY, _originPosition.z);
        farEndMarker.transform.SetParent(transform);

        GameObject leftArrowNearEndMarker = new GameObject("LeftArrowNearEndMarker");
        leftArrowNearEndMarker.transform.position = _leftArrow.transform.position;
        leftArrowNearEndMarker.transform.SetParent(farEndMarker.transform);

        GameObject rightArrowNearEndMarker = new GameObject("RightArrowNearEndMarker");
        rightArrowNearEndMarker.transform.position = _rightArrow.transform.position;
        rightArrowNearEndMarker.transform.SetParent(farEndMarker.transform);

        _leftArrow.GetComponent<Arrow>().NearEndMarker = leftArrowNearEndMarker.transform;
        _leftArrow.GetComponent<Arrow>().FarEndMarker = farEndMarker.transform;
        _leftArrow.transform.SetParent(farEndMarker.transform);

        _rightArrow.GetComponent<Arrow>().NearEndMarker = rightArrowNearEndMarker.transform;
        _rightArrow.GetComponent<Arrow>().FarEndMarker = farEndMarker.transform;
        _rightArrow.transform.SetParent(farEndMarker.transform);

        _allArrows = new List<GameObject> { _leftArrow, _rightArrow };
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
        Vector3 cubePrefabScale = _cubePrefab.transform.localScale;
        _allCubes = new List<GameObject>();

        for (int x = 0; x < _cubeNumber; x++)
        {
            for (int y = 0; y < _cubeNumber; y++)
            {
                for (int z = 0; z < _cubeNumber; z++)
                {
                    Vector3 cubePosition = new Vector3(-x * cubePrefabScale.x, -y * cubePrefabScale.y, z * cubePrefabScale.z) + _originPosition;
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

    public void SelectArrow(GameObject anArrow)
    {
        foreach(var arrow in _allArrows)
        {
            if (arrow != anArrow)
                arrow.SetActive(false);
        }
    }

    public void UnSelectArrow()
    {
        foreach (var arrow in _allArrows)
        {
            arrow.SetActive(true);
        }
    }
}
