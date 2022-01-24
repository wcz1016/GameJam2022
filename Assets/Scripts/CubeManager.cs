using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private static CubeManager _instance;

    public static CubeManager Instance { get { return _instance; } }

    public float zAxisLimit;

    public float xAxisLimit;

    private GameObject[] allCubes;

    private List<Vector2Int> leftSelectedPositions;

    private List<Vector2Int> rightSelectedPositions;

    public HashSet<Vector2Int> leftAnswerPositions;

    public HashSet<Vector2Int> rightAnswerPositions;

    public int usedNum;

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

    public void init()
    {
        allCubes = GameObject.FindGameObjectsWithTag("Cube");
        leftSelectedPositions = new List<Vector2Int>();
        rightSelectedPositions = new List<Vector2Int>();
        leftAnswerPositions = new HashSet<Vector2Int>();
        rightAnswerPositions = new HashSet<Vector2Int>();
        leftAnswerPositions.Add(new Vector2Int(0,0));
        usedNum = 0;
    }

    public void adjustCubesVisibility()
    {
        foreach(GameObject cube in allCubes)
        {
            if (cube.transform.position.x < xAxisLimit || cube.transform.position.z > zAxisLimit)
                cube.SetActive(false);
            else
                cube.SetActive(true);
        }
    }

    public void addLeftPos(Vector2Int pos)
    {
        leftSelectedPositions.Add(pos);
    }

    public void addRightPos(Vector2Int pos)
    {
        rightSelectedPositions.Add(pos);
    }

    public void removeLeftPos(Vector2Int pos)
    {
        leftSelectedPositions.Remove(pos);
    }
    
    public void removeRightPos(Vector2Int pos)
    {
        rightSelectedPositions.Remove(pos);
    }

    public bool isCorrect()
    {
        return leftAnswerPositions.SetEquals(new HashSet<Vector2Int>(leftSelectedPositions))
            && rightAnswerPositions.SetEquals(new HashSet<Vector2Int>(rightSelectedPositions));
        
    }

    void debugPoss()
    {
        Debug.Log("left");
        HashSet<Vector2Int> leftSelectedPostionSet = new HashSet<Vector2Int>(leftSelectedPositions);
        foreach (Vector2Int vec in leftSelectedPostionSet)
        {
            Debug.Log(vec);
        }
        Debug.Log("right");
        HashSet<Vector2Int> rightSelectedPostionSet = new HashSet<Vector2Int>(rightSelectedPositions);
        foreach (Vector2Int vec in rightSelectedPostionSet)
        {
            Debug.Log(vec);
        }
    }
    void debugAnswers()
    {
        Debug.Log("left");
        foreach (Vector2Int vec in leftAnswerPositions)
        {
            Debug.Log(vec);
        }
        Debug.Log("right");
    
        foreach (Vector2Int vec in rightAnswerPositions)
        {
            Debug.Log(vec);
        }
    }

    public void resetCubes()
    {
        foreach(GameObject cube in allCubes)
        {
            cube.GetComponent<Cube>().deselectCube();
        }
        leftSelectedPositions.Clear();
        rightSelectedPositions.Clear();
        usedNum = 0;
        GameObject.FindGameObjectWithTag("UI").GetComponent<UI>().setUsedNum();
    }
}
