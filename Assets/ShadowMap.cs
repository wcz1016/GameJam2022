using UnityEngine;

[CreateAssetMenu(fileName = "New ShadowMap", menuName = "Shadow Map", order = 51)]
public class ShadowMap : ScriptableObject
{
    public Vector2Int[] leftPos;
    public Vector2Int[] rightPos;
}