using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public enum UnitType
    {
        unit,
        hero,
    }

    public enum UnitClass
    {
        Daughter,
        Fermer,
        FermerR,
        Villager,
        VillagerR,
        Sheriff
    }

    public string name { get; private set; }
    public UnitType type { get; private set; }
    public UnitClass unitClass { get; private set; }
    public bool selected { get; private set; }

    private Stat _damage;
    private Stat _atkSpeed;
    private Stat _atkRange;
    private Stat _health;
    private Stat _defence;
    private Stat _speed;

    public int damage => _damage.value;
    public int atkSpeed => _atkSpeed.value;
    public int atkRange => _atkRange.value;
    public int health => _health.value;
    public int defence => _defence.value;
    public int speed => _speed.value;

    public Unit
    (
        string name,
        UnitType type,
        UnitClass unitClass,
        int damage,
        int atkSpeed,
        int atkRange,
        int health,
        int defence,
        int speed
    )
    {
        this.name = name;
        this.type = type;
        this.unitClass = unitClass;

        _damage = new Stat(damage);
        _atkSpeed = new Stat(atkSpeed);
        _atkRange = new Stat(atkRange);
        _health = new Stat(health);
        _defence = new Stat(defence);
        _speed = new Stat(speed);

        this.selected = true; // for demo all unit selected always
    }

    public void Upgrade()
    {
        // set all stats max level for demo
        _damage.LevelUp(true);
        // _atkSpeed.LevelUp(true);
        // _atkRange.LevelUp(true);
        _health.LevelUp(true);
        _defence.LevelUp(true);
        // _speed.LevelUp(true);
    }
}
