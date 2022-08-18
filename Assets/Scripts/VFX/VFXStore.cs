using System.Collections;
using UnityEngine;

public class VFXStore : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private float ttl = 1;
    public string debugKey;

    public void Run()
    {
        // need to think
        //if (_vfx != null)
        //{
        //    Destroy(_vfx);
        //}

        GameObject _vfx = Instantiate(vfxPrefab, transform);
        _vfx.transform.position = transform.position;

        StartCoroutine(AutoDestroy(_vfx));
    }

    private IEnumerator AutoDestroy(GameObject vfx)
    {
        yield return new WaitForSeconds(ttl);
        Destroy(vfx);
    } 
}
