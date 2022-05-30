using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingAlien : MonoBehaviour
{
    private Image _image;
    private GameObject _dialogueBox;
    private AudioSource _audioSource;
    private GameObject _nextLevelButton;

    [SerializeField][TextArea]
    private string _correctText, _incorrectText;

    [SerializeField]
    private AudioClip _correctSound, _incorrectSound;

    [SerializeField]
    private float _correctAnimTime, _incorrectAnimTime;

    void Start()
    {
        _image = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
        _dialogueBox = transform.Find("DialogueBox").gameObject;
        _nextLevelButton = transform.Find("NextLevelButton").gameObject;

        UI.Instance.OnCheckCubes += SaySomething;

        DisableSelf();
    }

    void SaySomething(bool isCorrect)
    {
        _image.enabled = true;
        _dialogueBox.SetActive(true);

        if (isCorrect)
        {
            _dialogueBox.GetComponentInChildren<Text>().text = _correctText;
            _nextLevelButton.SetActive(true);
            _audioSource.PlayOneShot(_correctSound);
        } 
        else
        {
            _dialogueBox.GetComponentInChildren<Text>().text = _incorrectText;
            StartCoroutine(PlaySoundsAndDisappear());
        }  
    }

    void DisableSelf()
    {
        _image.enabled = false;
        _dialogueBox.SetActive(false);
        _audioSource.Stop();
        _nextLevelButton.SetActive(false);
    }

    IEnumerator PlaySoundsAndDisappear()
    {

        _audioSource.clip = _incorrectSound;
        _audioSource.Play();
        yield return new WaitForSeconds(_incorrectAnimTime);
        DisableSelf();
    }
}
