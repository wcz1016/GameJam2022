using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game _instance;
    public static Game Instance => _instance;

    public GameObject frontArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject shadowPrefab;

    public GameObject leftWall;
    public GameObject rightWall;

    [SerializeField]
    private int _cubeNumber = 11;
    public int CubeNumber => _cubeNumber;

    public ShadowMap shadowMap;

    public GameObject leftImage;
    public GameObject rightImage;

    private Vector3 _cubePrefabScale;

    public Vector3 OriginPosition => gameObject.transform.position;

    private GameObject[,] leftShadows;
    private GameObject[,] rightShadows;

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

    void Start()
    {
        _cubePrefabScale = CubeManager.Instance.CubePrefab.transform.localScale;
        // InitCubes();
        InitArrows();
        CubeManager.Instance.Init();
        InitAnswer();
        InitShadow();
        UI.Instance.OnCheckCubes += FinishLevelIfCorrect;
    }

    void InitArrows()
    {
        int halfCubeNum = _cubeNumber / 2;
        // make arrows slightly higher
        float posY = OriginPosition.y + (halfCubeNum + 1) * _cubePrefabScale.y;

        // The far and near is related to camera
        float farthestX = OriginPosition.x + (halfCubeNum - 1) * _cubePrefabScale.x;
        float farthestZ = OriginPosition.z - (halfCubeNum - 1) * _cubePrefabScale.z;

        float nearestX = OriginPosition.x - (halfCubeNum + 1) * _cubePrefabScale.x;
        float nearestZ = OriginPosition.z + (halfCubeNum + 1) * _cubePrefabScale.z;

        Vector3 leftArrowPosition = new Vector3(farthestX + _cubePrefabScale.x, posY, nearestZ);
        Vector3 rightArrowPosition = new Vector3(nearestX, posY, farthestZ - _cubePrefabScale.x);

        // TODO: is this kind of Instantiate a gameobject -> init its script process right?
        // If so, maybe axis should also be inited here?
        GameObject leftArrow = Instantiate(leftArrowPrefab, leftArrowPosition, Quaternion.identity, gameObject.transform);
        GameObject rightArrow = Instantiate(frontArrowPrefab, rightArrowPosition, Quaternion.identity, gameObject.transform);

        GameObject leftArrowNearEndMarker = new GameObject("LeftArrowNearEndMarker");
        leftArrowNearEndMarker.transform.position = leftArrow.transform.position;
        leftArrowNearEndMarker.transform.SetParent(transform);

        GameObject rightArrowNearEndMarker = new GameObject("RightArrowNearEndMarker");
        rightArrowNearEndMarker.transform.position = rightArrow.transform.position;
        rightArrowNearEndMarker.transform.SetParent(transform);

        GameObject farEndMarker = new GameObject("FarEndMarker");
        farEndMarker.transform.position = new Vector3(farthestX, posY, farthestZ);
        farEndMarker.transform.SetParent(transform);

        leftArrow.GetComponent<Arrow>().NearEndMarker = leftArrowNearEndMarker.transform;
        leftArrow.GetComponent<Arrow>().FarEndMarker = farEndMarker.transform;

        rightArrow.GetComponent<Arrow>().NearEndMarker = rightArrowNearEndMarker.transform;
        rightArrow.GetComponent<Arrow>().FarEndMarker = farEndMarker.transform;
    }

    void InitAnswer()
    {
        CubeManager.Instance.leftAnswerIndexes = new HashSet<Vector2Int>(shadowMap.leftPos);
        CubeManager.Instance.rightAnswerIndexes = new HashSet<Vector2Int>(shadowMap.rightPos);
    }

    void InitShadow()
    {
        // weird, but leave it as it is now
        float farthestZ = OriginPosition.z - (_cubeNumber / 2) * _cubePrefabScale.z;
        float nearestX = OriginPosition.x - (_cubeNumber / 2) * _cubePrefabScale.x;
        float nearestY = OriginPosition.y - (_cubeNumber / 2) * _cubePrefabScale.y;
        // Left side awnser shadows
        foreach (Vector2Int vec in CubeManager.Instance.leftAnswerIndexes)
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
