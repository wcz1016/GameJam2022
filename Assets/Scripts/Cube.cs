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
        if (GameObject.Find("MenuPanel"))
            return;
        if (!active)
        {
            selectCube();
            CubeManager.Instance.addLeftPos(new Vector2Int(zPos, yPos));
            CubeManager.Instance.addRightPos(new Vector2Int(xPos, yPos));
            CubeManager.Instance.usedNum += 1;
        }
        else
        {
            deselectCube();
            CubeManager.Instance.removeLeftPos(new Vector2Int(zPos, yPos));
            CubeManager.Instance.removeRightPos(new Vector2Int(xPos, yPos));
            CubeManager.Instance.usedNum -= 1;
        }
        GameObject.FindGameObjectWithTag("UI").GetComponent<UI>().setUsedNum();
    }

    public void selectCube()
    {
        active = true;
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    public void deselectCube()
    {
        active = false;
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        gameObject.GetComponent<Renderer>().material.color = defaultColor;
    }
}
