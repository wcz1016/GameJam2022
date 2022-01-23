using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartMenu : MonoBehaviour
{
    public GameObject chooseLevelPanel;
    public void startGame()
    {
        SceneManager.LoadScene("Level0");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void openLevelPanel()
    {
        chooseLevelPanel.SetActive(true);
    }

    public void closeLevelPanel()
    {
        chooseLevelPanel.SetActive(false);
    }

    public void chooseLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex + 1);
    }
}
