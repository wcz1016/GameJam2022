using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private static CubeManager _instance;

    public static CubeManager Instance { get { return _instance; } }

    public float ZAxisLimit;

    public float XAxisLimit;

    private GameObject[] _allCubes;

    private List<Vector2Int> leftSelectedIndexes;

    private List<Vector2Int> rightSelectedIndexes;

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

    public void Init()
    {
        _allCubes = GameObject.FindGameObjectsWithTag("Cube");
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
            if (cube.transform.position.x < XAxisLimit || cube.transform.position.z > ZAxisLimit)
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

    public void resetCubes()
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
}
