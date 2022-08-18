using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILevelScene : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button equipButton;

    [SerializeField] private UIBar damageBar;
    [SerializeField] private UIBar atkSpeedBar;
    [SerializeField] private UIBar atkRangeBar;
    [SerializeField] private UIBar healthBar;
    [SerializeField] private UIBar defenceBar;

    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;
    [SerializeField] private GameObject slot4;

    // buttons
    public void OnMenuPress()
    {
        Navigation.NavigateMain();
    }

    public void OnEquipPress()
    {
        StartCoroutine(RunEquip());
    }

    private IEnumerator RunEquip()
    {
        UnitService.deck.DemoUpgrade();

        equipButton.interactable = false;

        slot1.SetActive(true);
        slot2.SetActive(true);
        slot3.SetActive(true);
        slot4.SetActive(true);

        damageBar.SetValue(UIBar.BarValue.two);
        yield return new WaitForSeconds(0.2f);
        damageBar.SetValue(UIBar.BarValue.three);
        yield return new WaitForSeconds(0.2f);

        atkSpeedBar.SetValue(UIBar.BarValue.two);
        yield return new WaitForSeconds(0.2f);
        atkSpeedBar.SetValue(UIBar.BarValue.three);
        yield return new WaitForSeconds(0.2f);

        atkRangeBar.SetValue(UIBar.BarValue.two);
        yield return new WaitForSeconds(0.2f);
        atkRangeBar.SetValue(UIBar.BarValue.three);
        yield return new WaitForSeconds(0.2f);

        healthBar.SetValue(UIBar.BarValue.two);
        yield return new WaitForSeconds(0.2f);
        healthBar.SetValue(UIBar.BarValue.three);
        yield return new WaitForSeconds(0.2f);

        defenceBar.SetValue(UIBar.BarValue.two);
        yield return new WaitForSeconds(0.2f);
        defenceBar.SetValue(UIBar.BarValue.three);
        yield return new WaitForSeconds(0.2f);
    }
}
