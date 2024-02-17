using UnityEngine;

public class Position : MonoBehaviour
{
    public Element element;
    public Vector3 position;
    public Vector3 size;
    public int arrayIndex;

    [HideInInspector]
    public int row;
    [HideInInspector]
    public int col;

    private void Awake()
    {
        position = transform.position;
        size = transform.localScale;
    }
}