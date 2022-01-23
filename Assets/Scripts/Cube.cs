using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Cube : MonoBehaviour
{
    public int xPos;
    public int yPos;
    public int zPos;

    private bool active = false;

    private Color defaultColor;

    private void Start()
    {
        defaultColor = gameObject.GetComponent<Renderer>().material.color;
    }
    private void OnMouseDown()
    {
        active = !active;
        if (active)
        {
            gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            CubeManager.Instance.addLeftPos(new Vector2Int(zPos, yPos));
            CubeManager.Instance.addRightPos(new Vector2Int(xPos, yPos));
            CubeManager.Instance.usedNum += 1;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            gameObject.GetComponent<Renderer>().material.color = defaultColor;
            CubeManager.Instance.removeLeftPos(new Vector2Int(zPos, yPos));
            CubeManager.Instance.removeRightPos(new Vector2Int(xPos, yPos));
            CubeManager.Instance.usedNum -= 1;
        }
        GameObject.Find("Canvas").GetComponent<UI>().setUsedNum();
    }
}
