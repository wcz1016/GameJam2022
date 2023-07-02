using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public Transform initialTransform;

    public GameObject cubePrefab;
    public GameObject frontArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject shadowPrefab;

    public GameObject leftWall;
    public GameObject rightWall;

    public int cubeNumber = 4;

    public ShadowMap shadowMap;

    public GameObject leftImage;
    public GameObject rightImage;

    private Vector3 _cubePrefabScale;
    private Vector3 _initialPositon;

    private float farthestX, farthestY, farthestZ;
    private float nearestX, nearestY, nearestZ;

    private GameObject[,] leftShadows;
    private GameObject[,] rightShadows;

    void Start()
    {
        _cubePrefabScale = cubePrefab.transform.localScale;
        _initialPositon = initialTransform.position;
        InitCubes();
        InitArrows();
        CubeManager.Instance.init();
        InitAnswer();
        InitShadow();
        UI.Instance.OnCheckCubes += FinishLevelIfCorrect;
    }

    void InitArrows()
    {   
        // The far and near is related to camera
        // TODO: is the reverse calculation of z axis related to left/right hand coordinate?
        farthestX = _initialPositon.x + (cubeNumber / 2 ) * _cubePrefabScale.x;
        farthestY = _initialPositon.y + (cubeNumber / 2 ) * _cubePrefabScale.y;
        farthestZ = _initialPositon.z - (cubeNumber / 2 ) * _cubePrefabScale.z;

        nearestX = _initialPositon.x - (cubeNumber / 2 ) * _cubePrefabScale.x;
        nearestY = _initialPositon.y - (cubeNumber / 2 ) * _cubePrefabScale.y;
        nearestZ = _initialPositon.z + (cubeNumber / 2 ) * _cubePrefabScale.z;

        // TODO: is this kind of Instantiate a gameobject -> init its script process right?
        GameObject leftArrow = Instantiate(leftArrowPrefab, new Vector3(
           farthestX + _cubePrefabScale.x,  farthestY + _cubePrefabScale.y, nearestZ + _cubePrefabScale.z
            ), Quaternion.identity);
        leftArrow.GetComponent<Arrow>().FarthestPosition = new Vector3(
            farthestX + _cubePrefabScale.x, farthestY + _cubePrefabScale.y, farthestZ
            );

        GameObject frontArrow = Instantiate(frontArrowPrefab, new Vector3(
            nearestX - _cubePrefabScale.x, farthestY + _cubePrefabScale.y, farthestZ
            ), Quaternion.identity);
        frontArrow.GetComponent<Arrow>().FarthestPosition = new Vector3(
            farthestX, farthestY + _cubePrefabScale.y, farthestZ
            );
    }

    void InitCubes()
    {
        for (int x = 0; x < cubeNumber; x++)
        {
            for (int y = 0; y < cubeNumber; y++)
            {
                for (int z = 0; z < cubeNumber; z++)
                {
                    GameObject cube = Instantiate(cubePrefab, new Vector3(
                        _initialPositon.x + (x - cubeNumber / 2) * _cubePrefabScale.x,
                        _initialPositon.y + (y - cubeNumber / 2) * _cubePrefabScale.y,
                        _initialPositon.z + (z - cubeNumber / 2) * _cubePrefabScale.z
                    ), Quaternion.identity);
                    cube.GetComponent<Cube>().XPos = x;
                    cube.GetComponent<Cube>().YPos = y;
                    cube.GetComponent<Cube>().ZPos = z;
                }
            }
        }
    }

    void InitAnswer()
    {
        CubeManager.Instance.leftAnswerIndexes = new HashSet<Vector2Int>(shadowMap.leftPos);
        CubeManager.Instance.rightAnswerIndexes = new HashSet<Vector2Int>(shadowMap.rightPos);
    }

    void InitShadow()
    {
        // Left side awnser shadows
        foreach(Vector2Int vec in CubeManager.Instance.leftAnswerIndexes)
        {
            int z = vec.x, y = vec.y;
            Instantiate(shadowPrefab, new Vector3(
                        leftWall.transform.position.x - leftWall.transform.localScale.x / 2 - 0.01f,
                        nearestY + y * shadowPrefab.transform.localScale.y,
                        farthestZ + z * shadowPrefab.transform.localScale.z
                       ), Quaternion.identity);
        }
        // Right side awnser shadows
        foreach (Vector2Int vec in CubeManager.Instance.rightAnswerIndexes)
        {
            int x = vec.x, y = vec.y;
            Instantiate(shadowPrefab, new Vector3(
                        nearestX + x * shadowPrefab.transform.localScale.z,
                        nearestY + y * shadowPrefab.transform.localScale.y,
                        rightWall.transform.position.z + rightWall.transform.localScale.z / 2 + 0.01f
                        ), Quaternion.Euler(0, 90, 0));
        }
    }

    public void FinishLevelIfCorrect(bool isCorrect)
    {
        if (isCorrect)
        {
            leftImage.gameObject.GetComponent<Renderer>().material.DOFade(1, 2);
            rightImage.gameObject.GetComponent<Renderer>().material.DOFade(1, 2);
        }
    }
}
