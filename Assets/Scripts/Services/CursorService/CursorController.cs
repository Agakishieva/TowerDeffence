using System.Collections;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] public int diffX;
    [SerializeField] public int diffY;

    private float spawnTime;
    private float minTTL = 0.5f;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        gameObject.transform.position = new Vector3(
            Input.mousePosition.x - diffX,
            Input.mousePosition.y - diffY,
            Input.mousePosition.z
        );
    }

    public void CursorDestroy()
    {
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitUntil(() => Time.time > spawnTime + minTTL);
        Destroy(gameObject);
    }
}
