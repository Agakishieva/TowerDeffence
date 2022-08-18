using System.Collections.Generic;
using UnityEngine;

public class UnitService
{
    public static DeckUnits deck = new DeckUnits();
    public static RoundUnits round = new RoundUnits();

    static UnitService()
    {
        deck.Load();
    }

    public class DeckUnits
    {
        public List<Unit> all { get; protected set; } // all available units and heroes
        public List<Unit> units => all.FindAll(FindUnits);
        public List<Unit> unitsSelected => units.FindAll(FindSelected); // units selected for next round
        public List<Unit> heroes => all.FindAll(FindHeroes);
        public List<Unit> heroesSelected => heroes.FindAll(FindSelected); // units selected for next round

        public void Load()
        {
            Unit unit1 = new Unit("Small", Unit.UnitType.unit, Unit.UnitClass.Daughter, 20, 1, 1, 90, 10, 1);
            Unit unit2 = new Unit("Medium", Unit.UnitType.unit, Unit.UnitClass.Fermer, 60, 1, 1, 90, 10, 1);
            Unit unit3 = new Unit("Large", Unit.UnitType.unit, Unit.UnitClass.Villager, 90, 1, 1, 90, 10, 1);

            all = new List<Unit>() { unit1, unit2, unit3 };
        }

        public void DemoUpgrade()
        {
            Unit unit4 = new Unit("Unit4", Unit.UnitType.unit, Unit.UnitClass.FermerR, 20, 1, 1, 100, 10, 1);
            all.Add(unit4);

            Unit unit5 = new Unit("Unit5", Unit.UnitType.unit, Unit.UnitClass.VillagerR, 20, 1, 1, 100, 10, 1);
            all.Add(unit5);

            Unit hero = new Unit("Hero", Unit.UnitType.hero, Unit.UnitClass.Sheriff, 120, 1, 1, 200, 20, 1);
            all.Add(hero);

            all.ForEach(UpgradeUnit);
        }

        // service
        private bool FindSelected(Unit unit) => unit.selected;

        private bool FindUnits(Unit unit) => unit.type == Unit.UnitType.unit;

        private bool FindHeroes(Unit unit) => unit.type == Unit.UnitType.hero;

        private void UpgradeUnit(Unit unit) => unit.Upgrade();
    }

    public class RoundUnits
    {
        public List<Unit> units { get; protected set; } // units avaulable in current round

        public bool isEmpty => units.Count == 0;

        // run at round start to init units
        public void Start()
        {
            // copy selected units to current round
            units = new List<Unit>(UnitService.deck.unitsSelected);
        }

        public bool Spawn(Unit unit)
        {
            // remove unit from current round
            return units.Remove(unit);
        }

        public Unit GetUnit()
        {
            if (isEmpty)
            {
                return null;
            }

            return units.Find(unit => unit != null);
        }
    }
}
