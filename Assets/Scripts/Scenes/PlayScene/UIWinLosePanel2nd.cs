using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLosePanel2nd : UIWinLosePanel
{
    [SerializeField] private Button menu;
    [SerializeField] private Button tryAgain;

    public override void SetResult(bool win)
    {
        base.SetResult(win);

        if (win == true)
        {
            menu.gameObject.SetActive(true);
        }
        else
        {
            tryAgain.gameObject.SetActive(true);
        }
    }
}
