using System.Collections;
using UnityEngine;

public class CursorService : MonoBehaviour
{
    [SerializeField] private CursorController cursorPrefab;

    private GameObject spawnedCursor;
    private bool isDragging;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            SpawCursor();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            RemoveCursor();
        }

        if (spawnedCursor != null && !isDragging)
        {
            RemoveCursor();
        }
    }

    private void SpawCursor()
    {
        if (spawnedCursor != null)
        {
            Destroy(spawnedCursor);
        }

        StartCoroutine(RunSpawnCursor());
    }

    private void RemoveCursor()
    {
        if (spawnedCursor != null)
        {
            CursorController controller = spawnedCursor.GetComponent<CursorController>();
            if (controller != null)
            {
                controller.CursorDestroy();
            } else
            {
                Destroy(spawnedCursor);
            }
        }
    }

    private IEnumerator RunSpawnCursor()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject cursor = Instantiate(
            cursorPrefab.gameObject,
            new Vector3(
                Input.mousePosition.x - cursorPrefab.diffX,
                Input.mousePosition.y - cursorPrefab.diffY,
                Input.mousePosition.z
            ),
            Quaternion.identity
        );

        cursor.transform.SetParent(GameObject.Find("Hud").transform, false);

        spawnedCursor = cursor;
    }
}
