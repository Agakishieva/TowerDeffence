using System.Collections;
using UnityEngine;

 public class GunnerTower : Tower
{
    [SerializeField, Range(0.5f, 2f)]
    private float _shootsPerSeconds = 1f;
    [SerializeField, Range(0.5f, 3f)]
    private float _shellBlastRadius = 1f;
    [SerializeField, Range(1f, 100f)]
    private float _damage;
    [SerializeField]
    private Transform _mortar;

    private float _launchSpeed;
    private float _launchProgress;


    public override void Awake()
    {
        base.Awake();
        OnValidate();
    } 

    private void OnValidate()
    {
        float x = _targetingRange + 0.251f;
        float y = -_mortar.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }

    public override void GameUpdate()
    {
        if (IsDead)
        {
            return;
        }
        base.GameUpdate();
        _launchProgress += Time.deltaTime * _shootsPerSeconds;
        while (_launchProgress >= 1f)
        {
            if (IsAcquirateTarget(out TargetPoint target))
            {
                if (towerAnim != null)
                {
                    towerAnim.Attack();
                }
                if (towerVFX != null)
                {
                    towerVFX.Attack();
                }
                Launch(target);
                _launchProgress -= 1f;
            }
            else
            {
                _launchProgress = 0.999f;
            }
        }
    }

    private void Launch(TargetPoint target)
    {
        Vector3 launchPoint = _mortar.position;
        Vector3 targetPoint = target.Position;
        targetPoint.y = 0f;
        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.z - launchPoint.z;
         

        float x = dir.magnitude; //длина вектора dir
        float y = -launchPoint.y;
        dir /= x;

        float g = 9.81f;
        float s = 3;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        //_mortar.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));

        if (Navigation.IsPlayScene())
            NormalGame.SpawnBullet().Initialize(launchPoint, targetPoint,
                //new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y), _shellBlastRadius, _damage);
                new Vector3(s * dir.x, 0, s * dir.y), _shellBlastRadius, _damage);
        else
            QuickGame.SpawnBullet().Initialize(launchPoint, targetPoint,
                new Vector3(s * dir.x, 0, s * dir.y), _shellBlastRadius, _damage);
    }
} 