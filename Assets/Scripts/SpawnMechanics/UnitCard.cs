using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    private Unit _unit;

    public Unit Unit => _unit;

    public event Action<UnitCard> onUnitCardClick;

    public void Init(Unit unit)
    {
        _unit = unit;
    }


    public void OnCardClick()
    {
        onUnitCardClick?.Invoke(this);
    }
}
