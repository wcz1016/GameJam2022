using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public CinemachineFreeLook cam;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cam.enabled = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            cam.enabled = false;
        }
    }
}
