using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UI : MonoBehaviour
{
    public Text usedNum;
    public GameObject menuPanel;
    

    public void checkIsCorrect()
    {
        if (CubeManager.Instance.isCorrect())
        {
            GameObject.Find("Main Camera").GetComponent<Game>().finishLevel();
        }
        else
        {
            // ʱ��ԭ���ύʧ�ܵĻ�û���κ���ʾ���������������ʾ������ UI ����ʾ�ͺ���
            Debug.Log("wrong");
        }      
    }

    public void setUsedNum()
    {
        usedNum.text = CubeManager.Instance.usedNum.ToString();
    }

    public void openMenuPanel()
    {
        menuPanel.SetActive(true);
    }

    public void closeMenuPanel()
    {
        menuPanel.SetActive(false);
    }

    public void goBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void resetCubes()
    {
        CubeManager.Instance.resetCubes();
        closeMenuPanel();
    }
}
