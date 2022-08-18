using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool _activePoint = false;

    private Material _material;
    private Color _baseColor;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        _baseColor = _material.color;
    }

    public void ActivateSpawn()
    {
        _material.color = Color.red;
    }

    public void DeactivateSpawn()
    {
        _material.color = _baseColor;
    }
}
