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

    private HashSet<Vector2Int> leftSelectedPositions;

    private HashSet<Vector2Int> rightSelectedPositions;

    public HashSet<Vector2Int> leftAnswerPositions;

    public HashSet<Vector2Int> rightAnswerPositions;

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
        leftSelectedPositions = new HashSet<Vector2Int>();
        rightSelectedPositions = new HashSet<Vector2Int>();
        leftAnswerPositions = new HashSet<Vector2Int>();
        rightAnswerPositions = new HashSet<Vector2Int>();
        leftAnswerPositions.Add(new Vector2Int(0,0));
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
        // debugPoss();
        return leftSelectedPositions.SetEquals(leftAnswerPositions)
             && rightSelectedPositions.SetEquals(rightAnswerPositions);
    }

    void debugPoss()
    {
        foreach(Vector2Int vec in leftSelectedPositions)
        {
            Debug.Log(vec);
        }
        foreach(Vector2Int vec in rightSelectedPositions)
        {
            Debug.Log(vec);
        }
    }
}
