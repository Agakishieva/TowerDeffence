using UnityEngine;
using System;
[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    
    [Serializable]
    class EnemyConfig
    {
        public Enemy Prefab;
        [SerializeField, FloatRangeSlider(0.5f, 2f)]
        //public FloatRange Scale = new FloatRange(0f);
        //[FloatRangeSlider(-0.2f, 0.2f)]
        public FloatRange PathOffset = new FloatRange(0f);
        [FloatRangeSlider(0.2f, 5f)]
        public FloatRange Speed = new FloatRange(1f);
        [FloatRangeSlider(10f, 1000f)]
        public FloatRange Health = new FloatRange(100f);
        [FloatRangeSlider(10f, 1000f)]
        public FloatRange Damage = new FloatRange(30f);
        [FloatRangeSlider(1f, 5f)]
        public FloatRange AttackRate = new FloatRange(2f);
    }

    [Serializable]
    class EnemyConfigNew
    {
        public Enemy Prefab;
    }

    [SerializeField]
    private EnemyConfig _small, _medium, _large;

    [SerializeField]
    private EnemyConfigNew _daughter, _fermer, _fermerR, _villager, _villagerR, _sheriff;


    public Enemy Get(Unit unitConfig)
    {
        var config = GetUnitPrefab(unitConfig);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.OriginFactory = this;
        instance.Initialize(1f, 0f,
            unitConfig.speed, unitConfig.health,
            unitConfig.damage, unitConfig.atkSpeed, unitConfig.atkRange);
        return instance;
    }

    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.OriginFactory = this;
        instance.Initialize(1f, config.PathOffset.RandomValueInRange,
            config.Speed.RandomValueInRange, config.Health.RandomValueInRange, 
            config.Damage.RandomValueInRange, config.AttackRate.RandomValueInRange, 1f);
        return instance;
    }

    private EnemyConfigNew GetUnitPrefab(Unit unitConfig)
    {
        var unitClass = unitConfig.unitClass;
        switch (unitClass)
        {
            case Unit.UnitClass.Daughter:
                return _daughter;
            case Unit.UnitClass.Fermer:
                return _fermer;
            case Unit.UnitClass.FermerR:
                return _fermerR;
            case Unit.UnitClass.Villager:
                return _villager;
            case Unit.UnitClass.VillagerR:
                return _villagerR;
            case Unit.UnitClass.Sheriff:
                return _sheriff;
        }
        Debug.LogError($"No config for {unitClass}");
        return _fermer;
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Large:
                return _large;
            case EnemyType.Medium:
                return _medium;
            case EnemyType.Small:
                return _small;
        }
        Debug.LogError($"No config for {type}");
        return _medium;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
 