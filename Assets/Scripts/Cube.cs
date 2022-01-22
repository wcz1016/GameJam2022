using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Cube : MonoBehaviour
{
    private bool active = false;
    private void OnMouseDown()
    {
        active = !active;
        if (active)
        {
            gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            gameObject.GetComponent<Renderer>().material.color = Color.black;
        }
    }
}
