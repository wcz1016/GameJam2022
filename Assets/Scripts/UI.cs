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

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // 时间原因提交失败的话没有任何提示，如果这里能有提示音或者 UI 的提示就好了
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
