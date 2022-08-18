using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitsSpawnUI : MonoBehaviour
{
    [SerializeField] private List<UnitCard> _units;
    [SerializeField] private List<UnitCard> _heroes;
    [SerializeField] private SpriteStorage _storage;

    public event Action<Unit> spawnUnit;
    public event Action allCardsUsed;

    public void Init()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            var active = UnitService.deck.units.Count > i;
            if (active)
            {
                _units[i].Init(UnitService.deck.units[i]);
                _units[i].gameObject.GetComponentInChildren<Image>().sprite = _storage.GetCardSprite(UnitService.deck.units[i].unitClass);
                _units[i].onUnitCardClick += OnCardClick;
            }
            _units[i].gameObject.SetActive(active);
        }

        for(int i = 0; i < _heroes.Count; i++)
        {
            var active = UnitService.deck.heroes.Count > i;
            if (active)
            {
                _heroes[i].Init(UnitService.deck.heroes[i]);
                _heroes[i].gameObject.GetComponentInChildren<Image>().sprite = _storage.GetCardSprite(UnitService.deck.heroes[i].unitClass);
                _heroes[i].onUnitCardClick += OnCardClick;
            }
            _heroes[i].gameObject.SetActive(active);
        }
    }

    private void OnCardClick(UnitCard unitCard)
    {
        spawnUnit?.Invoke(unitCard.Unit);
        unitCard.onUnitCardClick -= OnCardClick;
        // unitCard.gameObject.SetActive(false); // no need to hide GO, animator will do it

        Unit unit = UnitService.round.GetUnit(); // get random unit for now
        UnitService.round.Spawn(unit);

        if (CheckAvailable() == false)
        {
            allCardsUsed?.Invoke();
        }
    }

    private bool CheckAvailable()
    {
        return !UnitService.round.isEmpty;
        // return _units.Any(unit => unit.isActiveAndEnabled) || _heroes.Any(hero => hero.isActiveAndEnabled);
    }
}
