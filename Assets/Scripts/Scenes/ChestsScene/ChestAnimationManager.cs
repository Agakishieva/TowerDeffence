using System.Collections;
using UnityEngine;

public class ChestAnimationManager : MonoBehaviour
{
    [SerializeField] private ChestAnimation[] chests;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ChestAnimation chest in chests)
        {
            StartCoroutine(chest.ChestOpening());
        }

        StartCoroutine(NavigateWithDelay());
    }

    private IEnumerator NavigateWithDelay()
    {
        yield return new WaitForSeconds(ChestAnimation.totalTimeout);

        // Navigation.NavigateUpgrade();
    }
}
