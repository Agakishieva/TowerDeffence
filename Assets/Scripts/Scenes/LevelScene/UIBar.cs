using UnityEngine;

public class UIBar : MonoBehaviour
{
    public enum BarValue
    {
        zero, one, two, three
    }

    [SerializeField] private GameObject bar1;
    [SerializeField] private GameObject bar2;
    [SerializeField] private GameObject bar3;

    public void SetValue(BarValue value)
    {
        bar1.SetActive((int)value > 0);
        bar2.SetActive((int)value > 1);
        bar3.SetActive((int)value > 2);
    }
}
