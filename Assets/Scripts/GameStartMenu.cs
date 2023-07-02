using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartMenu : MonoBehaviour
{
    //TODO: will unity inspector also show attribute with '_'?
    [SerializeField]
    private GameObject _chooseLevelPanel;

    [SerializeField]
    private float _buttonSoundLastTime;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        StartCoroutine(ChooseLevelCoroutine(0));
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameCoroutine());
    }

    public void OpenLevelPanel()
    {
        PlayButtonSound();
        _chooseLevelPanel.SetActive(true);
    }

    public void CloseLevelPanel()
    {
        PlayButtonSound();
        _chooseLevelPanel.SetActive(false);
    }

    public void ChooseLevel(int levelIndex)
    {
        StartCoroutine(ChooseLevelCoroutine(levelIndex));    
    }

    private IEnumerator ChooseLevelCoroutine(int levelIndex)
    {
        PlayButtonSound();
        yield return new WaitForSeconds(_buttonSoundLastTime);
        SceneManager.LoadScene(levelIndex + 1);
    }

    private IEnumerator ExitGameCoroutine()
    {
        PlayButtonSound();
        yield return new WaitForSeconds(_buttonSoundLastTime);
        Application.Quit();
    }

    private void PlayButtonSound()
    {
        _audioSource.Play();
    }
}
