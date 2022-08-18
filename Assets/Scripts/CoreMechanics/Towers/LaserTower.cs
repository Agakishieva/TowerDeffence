using System.Collections;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField, Range(1f, 100f)]
    private float _damagePerSeconds = 10f;

    public GameObject _bullet;
    public float Speed;
    [SerializeField]
    private Transform _turret;

    [SerializeField]
    private Transform _laserBeam;

    private Vector3 _laserBeamScale;
    private float _launchProgress;
    [SerializeField, Range(0.5f, 2f)]
    private float _shootsPerSeconds = 1f;
    private TargetPoint _target;

    //public override GameTileContentType Type => GameTileContentType.LaserTower;
    //public override TowerType Type => TowerType.Laser;

    private void Awake()
    {
        _laserBeamScale = _laserBeam.localScale;
    }

    public override void GameUpdate()
    {
        if (IsTargetTracked(ref _target) || IsAcquirateTarget(out _target))
        {
            Shoot();
        }
        else
        {
            _laserBeam.localScale = Vector3.zero;
        }

    }
        private void Shoot() 
    {

        var point = _target.Position;
        _turret.LookAt(point);
        _laserBeam.localRotation = _turret.localRotation;

        var distance = Vector3.Distance(_turret.position, point);
        _laserBeamScale.z = distance;
        _laserBeam.localScale = _laserBeamScale;
        _laserBeam.localPosition = _turret.localPosition + 0.5f * distance * _laserBeam.forward;

        _target.Enemy.TakeDamage(_damagePerSeconds * Time.deltaTime);
    }
}
