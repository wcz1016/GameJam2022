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
}
