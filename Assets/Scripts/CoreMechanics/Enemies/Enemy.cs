using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static DirectionExtensions;

public class Enemy : GameBehavior, IDamageable
{
    [SerializeField]
    private Transform _model;

    private TowerPoint _target;
    private DestinationPoint _dest;
    [SerializeField, Range(1f, 10.5f)]
    protected float _targetingRange = 1f;
    public EnemyFactory OriginFactory { get; set; }
    private GameTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress, _progressFactor;

    private Direction _direction;
    private DirectionChange _directionChange;
    private float _directionAngleFrom, _directionAngleTo;
    private float _pathOffset;
    private float _speed;
    private bool _isDead;

    // attack
    private float _attackRate;
    private float _attackProgress;

    private IEnemyAnimation enemyAnimation;

    private IVFXController enemyVFX;

    public float Scale { get; private set; }
    public float Health { get; private set; }
    public float Damage { get; private set; }

    public event Action onDestinationReached;

    public void Awake()
    {
        enemyAnimation = this.GetComponentInChildren<IEnemyAnimation>();

        enemyVFX = GetComponent<IVFXController>();

        //transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    public void Initialize(float scale, float pathOffset, float speed, float health, float damage, float attackRate, float attackRange)
    {
        //_originalSpeed = speed;
        _model.localScale = new Vector3(scale, scale, scale);
        _pathOffset = pathOffset;
        _speed = speed;
        Scale = scale;
        Health = health;
        Damage = damage;
        _attackRate = attackRate;
        _targetingRange = attackRange;
    }

    private void Attack()
    {
        _attackProgress += Time.deltaTime * _attackRate;

        if(_attackProgress >= 1f)
        {
            _target.Tower.TakeDamage(Damage);
            _attackProgress -= 1f;
        }
        else
        {
            enemyAnimation.PlayWalkAnimation();
        }
        enemyAnimation.PlayAttackAnimation();

    }

    private void AttackDestination()
    {
        _attackProgress += Time.deltaTime * _attackRate;

        if (_attackProgress >= 1f)
        {
            if (Navigation.IsActiveScene(Navigation.quickGameScene))
                QuickGame.EnemyReachedDestination();
            else
                NormalGame.EnemyReachedDestination();
            _attackProgress -= 1f;
        }
        else
        {
            enemyAnimation.PlayWalkAnimation();
        }
        enemyAnimation.PlayAttackAnimation();
        onDestinationReached?.Invoke();

    }

    protected private bool TowerFound(out TowerPoint target)
    {
        if (TowerPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            target = TowerPoint.GetBuffered(0);
            return target != null;
        }
        target = null;
        return false;
    }

    protected private bool DestinationFound(out DestinationPoint target)
    {
        if (DestinationPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            target = DestinationPoint.GetBuffered(0);
            return target != null;
        }
        target = null;
        return false;
    }

    protected private bool IsTargetTrackedTower(ref TowerPoint target)
    {
        if (target == null)
        {
            return false;
        }

        Vector3 myPos = transform.localPosition;
        Vector3 targetPos = target.Position;
        if (Vector3.Distance(myPos, targetPos) > _targetingRange + target.ColliderSize + target.Tower.Scale)
        {
            target = null;
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected() //вызывается на каждый кадр пока мы в окне сцены и пока выбран этот объект
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.localPosition;
        position.y += 0.1f;
        Gizmos.DrawWireSphere(position, _targetingRange);

    }

    public void SpawnOn(GameTile tile)
    {
        transform.localPosition = tile.transform.localPosition;
        _tileFrom = tile;
        _tileTo = tile.NextTileOnPath;
        _progress = 0;
        PrepareIntro();
    }

    private void PrepareIntro()
    {
        _positionFrom = _tileFrom.transform.localPosition;
        _positionTo = _tileFrom.ExitPoint;
        _direction = _tileFrom.PathDirection;
        _directionChange = DirectionChange.None;
        _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f);
        _progressFactor = 2 * _speed;

        // hack for correct sprite display
        // TODO: add solution to change sorting order by coordinate change
        SortingGroup group = GetComponent<SortingGroup>();
        if (group == null)
        {
            return;
        }

        if (_direction == Direction.North || _direction == Direction.East)
        {
            group.sortingOrder = 50;
        }
    }

    private void PrepareOutro()
    {
        _positionTo = _tileFrom.transform.localPosition;
        _directionChange = DirectionChange.None;
        _directionAngleTo = _direction.GetAngle();
        _model.localPosition = new Vector3(_pathOffset, 0f);
        _progressFactor = 2f * _speed;
    }

    public override bool GameUpdate()
    {
        if (_isDead)
        {
            return false;
        }

        if (Health <= 0f)
        {
            StartCoroutine(Death());
            return false;
        }
        if (DestinationFound(out _dest))
        {
            AttackDestination();
            Debug.Log("Обнажурен дом!!!"); // :-D

        }
        else if (TowerFound(out _target))
        {
            Attack();
        }
        else
        {
            _progress += Time.deltaTime * _progressFactor;
            while (_progress >= 1)
            {
                if (_tileTo == null)
                {
                    if(Navigation.IsActiveScene(Navigation.quickGameScene))
                        QuickGame.EnemyReachedDestination();
                    else
                        NormalGame.EnemyReachedDestination();

                    Recycle();
                    return false;
                }
                _progress = (_progress - 1f) / _progressFactor;

                PrepareNextState();
                _progress *= _progressFactor;
            }

            if (_directionChange == DirectionChange.None)
            {
                transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
            }
            //else
            //{
            //    float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
            //    transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            //}
            //Debug.Log("Пусто");
        }
        return true;
    }

    public void TakeDamage(float damage)
    {
        //DamageUI.sharedInstance.AddText((int)damage, _model.position);
        Health -= damage;
        enemyAnimation.SetHealth((int)Health);

        if (Health <= 0 && !_isDead)
        {
            // enemyVFX?.Death();
        }
    }

    private void PrepareNextState()
    {
        _tileFrom = _tileTo;
        _tileTo = _tileTo.NextTileOnPath;
        _positionFrom = _positionTo;
        if (_tileTo == null)
        {
            PrepareOutro();
        }
        _positionTo = _tileFrom.ExitPoint;
        _directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
        _direction = _tileFrom.PathDirection;
        _directionAngleFrom = _directionAngleTo;

        switch (_directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;

        }

        enemyAnimation.SetDirection(_direction);
    }

    private void PrepareForward()
    {
        //_directionAngleTo = _direction.GetAngle();
        //transform.localRotation = _direction.GetRotation();
        _model.localPosition = new Vector3(_pathOffset, 0f);
        _progressFactor = _speed;
    }

    private void PrepareTurnRight()
    {
        //_directionAngleTo = _directionAngleFrom + 90f;
        _model.localPosition = new Vector3(_pathOffset - 0.5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffset));
    }

    private void PrepareTurnLeft()
    {
        //_directionAngleTo = _directionAngleFrom - 90f;
        _model.localPosition = new Vector3(_pathOffset + 0.5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffset));

    }

    private void PrepareTurnAround()
    {
        //_directionAngleTo = _directionAngleFrom + (_pathOffset < 0f ? 180f : -180f);
        _model.localPosition = new Vector3(_pathOffset, 0f);
        transform.localPosition = _positionFrom;
        _progressFactor = _speed / (Mathf.PI * Mathf.Max(Mathf.Abs(_pathOffset), 0.2f));
    }

    private IEnumerator Death()
    {
        _isDead = true;
        enemyVFX?.Death();

        yield return new WaitForSeconds(2.5f);
        Recycle();
    }

    public override void Recycle()
    {
        OriginFactory.Reclaim(this);
    }
}

