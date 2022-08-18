using UnityEngine;
using UnityEngine.UI;

public class UIWinLosePanel : MonoBehaviour
{
    [SerializeField] private Text result;
    [SerializeField] private Button endRoundBtn;
    [SerializeField] private Text endRoundBtnText;

    protected bool state;

    public virtual void SetResult(bool win)
    {
        if (win == true)
        {
            state = true;
            result.text = "WIN";
            if (endRoundBtnText != null)
            {
                endRoundBtnText.text = "Main menu";
            }
        }
        else
        {
            state = false;
            result.text = "LOSE";
            if (endRoundBtnText != null)
            {
                endRoundBtnText.text = "Try again";
            }
        }
    }

    public void OnPress()
    {
        if (state)
        {
            Navigation.NavigateMain();
        } else
        {
            Navigation.NavigateChests();
        }
    }
}
