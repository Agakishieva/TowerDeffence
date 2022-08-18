using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WarEntity
{
    private Vector3 _launchPoint, _targetPoint, _launchVelocity;

    private float _age;
    private float _blastRadius, _damage;
    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, float blastRadius, float damage)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;
        _blastRadius = blastRadius;
        _damage = damage;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        Vector3 p = _launchPoint + _launchVelocity * _age;
        p.y -= 0.01f * 9.81f * _age * _age;
    
        if (p.y <= 0.7f)
        {
            if (Navigation.IsPlayScene())
                NormalGame.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
            else
                QuickGame.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);

            OriginFactory.Reclaim(this);
            return false;
        }

        if (p.magnitude > 0)
        {
            transform.localPosition = p;
            Vector3 d = _launchVelocity;
            //d.y -= 9.81f * _age;
            transform.localRotation = Quaternion.LookRotation(d);

            if (Navigation.IsPlayScene())
                NormalGame.SpawnExplosion().Initialize(p, 0.1f);
            else
                QuickGame.SpawnExplosion().Initialize(p, 0.1f);
        }

        return true;
    }
}
