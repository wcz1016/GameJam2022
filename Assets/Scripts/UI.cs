using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    public void checkIsCorrect()
    {
        if (CubeManager.Instance.isCorrect())
        {
            Debug.Log("ÕýÈ·");
        } else {
            Debug.Log("´íÎó");
        }
                
    }
}
