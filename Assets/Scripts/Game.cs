using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game _instance;
    public static Game Instance => _instance;

    public GameObject shadowPrefab;

    public GameObject leftWall;
    public GameObject rightWall;

    [SerializeField]
    private int _cubeNumber = 11;
    public int CubeNumber => _cubeNumber;

    public ShadowMap shadowMap;

    public GameObject leftPainting;
    public GameObject rightPainting;

    private Vector3 _cubePrefabScale;

    public Vector3 OriginPosition => gameObject.transform.position;

    public bool BlockInput;

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
        CubeManager.Instance.Init();
        InitAnswer();
        InitShadow();
        UI.Instance.OnCheckCubes += ShowPainting;
        BlockInput = false;
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

    public void ShowPainting(bool isCorrect)
    {
        if (isCorrect)
        {
            leftPainting.gameObject.GetComponent<Renderer>().material.DOFade(1, 2);
            rightPainting.gameObject.GetComponent<Renderer>().material.DOFade(1, 2);
        }
    }
}
