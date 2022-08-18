using UnityEngine;

public class UISfxStore : MonoBehaviour
{
    [SerializeField] private AudioSource chestOpen;

    private void Awake()
    {
        AudioService.SetUIStore(this);
    }

    public void ChestOpen() => chestOpen.Play();
}
