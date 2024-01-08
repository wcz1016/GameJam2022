using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class UI : MonoBehaviour
{
    public static UI Instance;

    public event Action<bool> OnCheckCubes;

    public GameObject MenuPanel;

    [SerializeField]
    private Text _usedNum;

    [SerializeField]
    private float _buttonSoundLastTime;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void CheckIsCorrect()
    {      
        PlayButtonSound();
        CubeManager.Instance.CheckIsCorrect();
    }

    public void SetUsedNum()
    {
        _usedNum.text = CubeManager.Instance.UsedNum.ToString();
    }

    public void OpenMenuPanel()
    {
        PlayButtonSound();
        MenuPanel.SetActive(true);
        Game.Instance.BlockInput = true;
    }

    public void CloseMenuPanel()
    {
        PlayButtonSound();
        MenuPanel.SetActive(false);
        Game.Instance.BlockInput = false;
    }

    public void GoBackToMainMenu()
    {
        PlayButtonSound();
        StartCoroutine(WaitSoundEndAndLoadScene(0));
    }

    public void GoToNextLevel()
    {
        PlayButtonSound();
        StartCoroutine(WaitSoundEndAndLoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator WaitSoundEndAndLoadScene(int index)
    {
        yield return new WaitForSeconds(_buttonSoundLastTime);
        SceneManager.LoadScene(index);
    }

    public void ResetCubes()
    {
        PlayButtonSound();
        CubeManager.Instance.ResetCubes();
        CloseMenuPanel();
    }

    private void PlayButtonSound()
    {
        _audioSource.Play();
    }

    public void InvokeCheckCubes(bool isCorrect)
    {
        OnCheckCubes.Invoke(isCorrect);
    }
}
