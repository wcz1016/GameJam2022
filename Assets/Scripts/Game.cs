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

    private Vector3 cubePrefabScale;
    private Vector3 initialPositon;

    private float farthestX, farthestY, farthestZ;
    private float nearestX, nearestY, nearestZ;

    private GameObject[,] leftShadows;
    private GameObject[,] rightShadows;

    // Start is called before the first frame update
    void Start()
    {
        cubePrefabScale = cubePrefab.transform.localScale;
        initialPositon = initialTransform.position;
        initCubes();
        initArrows();
        CubeManager.Instance.init();
        initAnswer();
        initShadow();
        UI.Instance.OnCheckCubes += FinishLevelIfCorrect;
    }

    void initArrows()
    {   
        // 整个大正方体的六个面坐标，对于 x 和 z，远的意思是离镜头远，对于 y ，远的意思是离地面远
        farthestX = initialPositon.x + (cubeNumber / 2 ) * cubePrefabScale.x;
        farthestY = initialPositon.y + (cubeNumber / 2 ) * cubePrefabScale.y;
        farthestZ = initialPositon.z - (cubeNumber / 2 ) * cubePrefabScale.z;

        nearestX = initialPositon.x - (cubeNumber / 2 ) * cubePrefabScale.x;
        nearestY = initialPositon.y - (cubeNumber / 2 ) * cubePrefabScale.y;
        nearestZ = initialPositon.z + (cubeNumber / 2 ) * cubePrefabScale.z;

        GameObject leftArrow = Instantiate(leftArrowPrefab, new Vector3(
           farthestX + cubePrefabScale.x,  farthestY + cubePrefabScale.y, nearestZ + cubePrefabScale.z
            ), Quaternion.identity);
        leftArrow.GetComponent<Arrow>().farthestPostion = new Vector3(
            farthestX + cubePrefabScale.x, farthestY + cubePrefabScale.y, farthestZ
            );

        GameObject frontArrow = Instantiate(frontArrowPrefab, new Vector3(
            nearestX - cubePrefabScale.x, farthestY + cubePrefabScale.y, farthestZ
            ), Quaternion.identity);
        frontArrow.GetComponent<Arrow>().farthestPostion = new Vector3(
            farthestX, farthestY + cubePrefabScale.y, farthestZ
            );
    }

    void initCubes()
    {
        for (int x = 0; x < cubeNumber; x++)
        {
            for (int y = 0; y < cubeNumber; y++)
            {
                for (int z = 0; z < cubeNumber; z++)
                {
                    GameObject cube = Instantiate(cubePrefab, new Vector3(
                        initialPositon.x + (x - cubeNumber / 2) * cubePrefabScale.x,
                        initialPositon.y + (y - cubeNumber / 2) * cubePrefabScale.y,
                        initialPositon.z + (z - cubeNumber / 2) * cubePrefabScale.z
                    ), Quaternion.identity);
                    cube.GetComponent<Cube>().XPos = x;
                    cube.GetComponent<Cube>().YPos = y;
                    cube.GetComponent<Cube>().ZPos = z;
                }
            }
        }
    }

    void initAnswer()
    {
        CubeManager.Instance.leftAnswerPositions = new HashSet<Vector2Int>(shadowMap.leftPos);
        CubeManager.Instance.rightAnswerPositions = new HashSet<Vector2Int>(shadowMap.rightPos);
    }
    void initShadow()
    {
        // 左侧答案投影
        foreach(Vector2Int vec in CubeManager.Instance.leftAnswerPositions)
        {
            int z = vec.x, y = vec.y;
            Instantiate(shadowPrefab, new Vector3(
                        leftWall.transform.position.x - leftWall.transform.localScale.x / 2 - 0.01f,
                        nearestY + y * shadowPrefab.transform.localScale.y,
                        farthestZ + z * shadowPrefab.transform.localScale.z
                       ), Quaternion.identity);
        }
        // 右侧答案投影
        foreach (Vector2Int vec in CubeManager.Instance.rightAnswerPositions)
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
