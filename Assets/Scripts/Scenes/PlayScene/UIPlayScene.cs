using UnityEngine;

public class UIPlayScene : MonoBehaviour
{
    [SerializeField] private UIWinLosePanel winLosePanel;
    [SerializeField] private UnitsSpawnUI _spawnUI;
    [SerializeField] private NormalGame _normalGame;

    private bool allCardsUsed;

    private void OnEnable()
    {
        allCardsUsed = false;

        _spawnUI.allCardsUsed += SetAllCardsUsed;
        _normalGame.allUnitsDead += CheckEndRound;
    }

    private void OnDisable()
    {
        _spawnUI.allCardsUsed -= SetAllCardsUsed;
        _normalGame.allUnitsDead -= CheckEndRound;
    }

    private void SetAllCardsUsed()
    {
        allCardsUsed = true;
    }

    private void CheckEndRound()
    {
        if (allCardsUsed)
        {
            ShowLose();
        }
    }

    // Win-Lose
    public void ShowWin()
    {
        winLosePanel.gameObject.SetActive(true);
        winLosePanel.SetResult(true);
    }

    public void ShowLose()
    {
        winLosePanel.gameObject.SetActive(true);
        winLosePanel.SetResult(false);
    }
}
