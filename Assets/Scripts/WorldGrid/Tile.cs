using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class Tile : MonoBehaviour
{
    public Transform Entry;
    private Renderer rend;
    public Bounds Bounds => rend.bounds;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }
}
