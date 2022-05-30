using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingAlien : MonoBehaviour
{
    private Image _image;
    private GameObject _dialogueBox;
    private AudioSource _audioSource;

    [SerializeField]
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

        UI.Instance.OnCheckCubes += SaySomething;

        DisableSelf();
    }

    void SaySomething(bool isCorrect)
    {
        _image.enabled = true;
        _dialogueBox.SetActive(true);

        var text = isCorrect ? _correctText : _incorrectText;
        _dialogueBox.GetComponentInChildren<Text>().text = text;

        StartCoroutine(PlaySoundsAndDisappear(isCorrect));
    }

    void DisableSelf()
    {
        _image.enabled = false;
        _dialogueBox.SetActive(false);
        _audioSource.Stop();
    }

    IEnumerator PlaySoundsAndDisappear(bool isCorrect)
    {
        if (isCorrect)
        {
            _audioSource.PlayOneShot(_correctSound);
            yield return new WaitForSeconds(_correctAnimTime);
        }
        else
        {
            _audioSource.clip = _incorrectSound;
            _audioSource.Play();
            yield return new WaitForSeconds(_incorrectAnimTime);
        }
        DisableSelf();
    }
}
