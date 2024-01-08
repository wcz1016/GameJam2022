using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Cube : MonoBehaviour
{
    public int XIndex;
    public int YIndex;
    public int ZIndex;

    [SerializeField]
    private AudioClip _selectSound;
    [SerializeField]
    private AudioClip _unselectSound;

    private bool _selected = false;

    private Color _defaultColor;

    private AudioSource _audioSource;

    private void Start()
    {
        _defaultColor = GetComponent<Renderer>().material.color;
        _audioSource = GetComponent<AudioSource>();
    }


    private void OnMouseDown()
    {
        if (Game.Instance.BlockInput)
            return;

        if (!_selected)
        {
            SelectSelf();
            _audioSource.PlayOneShot(_selectSound);
            // TODO: CubeManager.AddCube(int xPos, yPos, zPos)
            CubeManager.Instance.AddLeftPos(new Vector2Int(ZIndex, YIndex));
            CubeManager.Instance.AddRightPos(new Vector2Int(XIndex, YIndex));
            CubeManager.Instance.UsedNum += 1;
        }
        else
        {
            UnselectSelf();
            _audioSource.PlayOneShot(_unselectSound);
            CubeManager.Instance.RemoveLeftPos(new Vector2Int(ZIndex, YIndex));
            CubeManager.Instance.RemoveRightPos(new Vector2Int(XIndex, YIndex));
            CubeManager.Instance.UsedNum -= 1;
        }

        UI.Instance.SetUsedNum();
    }

    private void SelectSelf()
    {
        _selected = true;
        
        GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void UnselectSelf()
    {
        _selected = false;
        
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        gameObject.GetComponent<Renderer>().material.color = _defaultColor;
    }
}
