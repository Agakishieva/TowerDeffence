using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class WarFactory : GameObjectFactory
{
    [SerializeField]
    private Shell _shellPrefab;

    [SerializeField]
    private Bullet _bulletPrefab;

    [SerializeField]
    private Arrow _arrowPrefab;

    [SerializeField]
    private Explosion _explosionPrefab;
    public Explosion Explosion => Get(_explosionPrefab);

    public Shell Shell => Get(_shellPrefab);
    public Bullet Bullet => Get(_bulletPrefab);
    public Arrow Arrow => Get(_arrowPrefab);

    private T Get<T>(T prefab) where T : WarEntity
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

    public void Reclaim(WarEntity entity)
    {
        Destroy(entity.gameObject);
    }
}
