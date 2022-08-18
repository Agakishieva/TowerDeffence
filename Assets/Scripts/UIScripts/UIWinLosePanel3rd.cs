using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLosePanel3rd : UIWinLosePanel
{
    [SerializeField] private Image _win;
    [SerializeField] private Image _lose;
    [SerializeField] private Button menu;
    [SerializeField] private Button tryAgain;

    public override void SetResult(bool win)
    {
        if (win == true)
        {
            state = true;
            _win.gameObject.SetActive(true);
            menu.gameObject.SetActive(true);
        }
        else
        {
            state = false;
            _lose.gameObject.SetActive(true);
            tryAgain.gameObject.SetActive(true);
        }
    }

}
