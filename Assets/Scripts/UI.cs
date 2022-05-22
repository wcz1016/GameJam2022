using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UI : MonoBehaviour
{
    public static UI Instance;

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

    public void SetUsedNum()
    {
        _usedNum.text = CubeManager.Instance.usedNum.ToString();
    }

    public void OpenMenuPanel()
    {
        PlayButtonSound();
        MenuPanel.SetActive(true);
    }

    public void CloseMenuPanel()
    {
        PlayButtonSound();
        MenuPanel.SetActive(false);
    }

    public void GoBackToMainMenu()
    {
        StartCoroutine(GoBackToMainMenuCoroutine());
    }

    private IEnumerator GoBackToMainMenuCoroutine()
    {
        PlayButtonSound();
        yield return new WaitForSeconds(_buttonSoundLastTime);
        SceneManager.LoadScene(0);
    }

    public void ResetCubes()
    {
        PlayButtonSound();
        CubeManager.Instance.resetCubes();
        CloseMenuPanel();
    }

    private void PlayButtonSound()
    {
        _audioSource.Play();
    }
}
