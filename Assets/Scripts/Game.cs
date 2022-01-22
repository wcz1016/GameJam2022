using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform initialTransform;
    public GameObject cubePrefab;
    public GameObject frontArrowPrefab;
    public GameObject leftArrowPrefab;

    public int cubeNumber = 4;

    private Vector3 cubePrefabScale;
    private Vector3 initialPositon;

    // Start is called before the first frame update
    void Start()
    {
        cubePrefabScale = cubePrefab.transform.localScale;
        initialPositon = initialTransform.position;
        initCubes();
        initArrows();
        CubeManager.Instance.init();
    }

    void initArrows()
    {   
        // 整个大正方体的六个面坐标，对于 x 和 z，远的意思是离镜头远，对于 y ，远的意思是离地面远
        float farthestX = initialPositon.x + (cubeNumber / 2 ) * cubePrefabScale.x;
        float farthestY = initialPositon.y + (cubeNumber / 2 ) * cubePrefabScale.y;
        float farthestZ = initialPositon.z - (cubeNumber / 2 ) * cubePrefabScale.z;

        float nearestX = initialPositon.x - (cubeNumber / 2 ) * cubePrefabScale.x;
        float nearestY = initialPositon.y - (cubeNumber / 2 ) * cubePrefabScale.y;
        float nearestZ = initialPositon.z + (cubeNumber / 2 ) * cubePrefabScale.z;

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
                    Instantiate(cubePrefab, new Vector3(
                        initialPositon.x - (x - cubeNumber / 2) * cubePrefabScale.x,
                        initialPositon.y - (y - cubeNumber / 2) * cubePrefabScale.y,
                        initialPositon.z - (z - cubeNumber / 2) * cubePrefabScale.z
                    ), Quaternion.identity);
                }
            }
        }
    }
}
