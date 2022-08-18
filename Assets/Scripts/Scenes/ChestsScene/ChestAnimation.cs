using System.Collections;
using UnityEngine;

public class ChestAnimation : MonoBehaviour/*, IPointerClickHandler*/
{
    private Animator animator;
    [SerializeField] private GameObject gacha;
    [SerializeField] private GameObject VFXopening;
    [SerializeField] private GameObject VFXburn;
    [SerializeField] private GameObject VFXgacha;

    private const float timeout1 = 2f;
    private const float timeout2 = 2f;
    private const float timeout3 = 3;
    private const float timeout4 = 1;

    public static float totalTimeout => timeout1 + timeout2 + timeout3 + timeout4;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator ChestOpening()
    {
        VFXburn.SetActive(true);
        yield return new WaitForSeconds(timeout1);
        animator.SetTrigger("chest_opening");
        yield return new WaitForSeconds(timeout2);
        VFXburn.SetActive(false);
        VFXopening.SetActive(true);
        yield return new WaitForSeconds(timeout3);
        this.gameObject.SetActive(false);
        gacha.SetActive(true);
        VFXgacha.SetActive(true);
    }
}
