using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Cube : MonoBehaviour
{
    public int XPos;
    public int YPos;
    public int ZPos;

    [SerializeField]
    private AudioClip _selectSound;
    [SerializeField]
    private AudioClip _unselectSound;

    private bool _active = false;

    private Color _defaultColor;

    private AudioSource _audioSource;

    private void Start()
    {
        _defaultColor = GetComponent<Renderer>().material.color;
        _audioSource = GetComponent<AudioSource>();
    }


    private void OnMouseDown()
    {
        if (UI.Instance.MenuPanel.activeSelf)
            return;

        if (!_active)
        {
            SelectSelf();
            _audioSource.PlayOneShot(_selectSound);
            // TODO: CubeManager.AddCube(int xPos, yPos, zPos)
            CubeManager.Instance.AddLeftPos(new Vector2Int(ZPos, YPos));
            CubeManager.Instance.AddRightPos(new Vector2Int(XPos, YPos));
            CubeManager.Instance.UsedNum += 1;
        }
        else
        {
            UnselectSelf();
            _audioSource.PlayOneShot(_unselectSound);
            CubeManager.Instance.RemoveLeftPos(new Vector2Int(ZPos, YPos));
            CubeManager.Instance.RemoveRightPos(new Vector2Int(XPos, YPos));
            CubeManager.Instance.UsedNum -= 1;
        }

        UI.Instance.SetUsedNum();
    }

    private void SelectSelf()
    {
        _active = true;
        
        GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void UnselectSelf()
    {
        _active = false;
        
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        gameObject.GetComponent<Renderer>().material.color = _defaultColor;
    }
}
