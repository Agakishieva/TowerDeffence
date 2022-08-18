using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private Button playBuyton;

    // buttons
    public void OnPlayClick()
    {
        Navigation.NavigatePlay();
    }

    public void OnPlayClick2()
    {
        Navigation.NavigatePlay2();
    }

    public void OnPlayClick3()
    {
        Navigation.NavigatePlay3();
    }
}
