using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Tower : GameTileContent, IDamageable
{
    public bool IsDead = false;
    public ITowerAnim towerAnim;
    public IVFXController towerVFX;
    [SerializeField, Range(1.5f, 10.5f)]
    protected float _targetingRange = 1.5f;

    [SerializeField]
    private float _health = 100f;

    public float Health => _health;
    public float Scale { get; private set; }

    public virtual void Awake()
    {
        towerAnim = this.GetComponent<ITowerAnim>();

        towerVFX = GetComponentInChildren<IVFXController>();
    }
    
    protected private bool IsAcquirateTarget(out TargetPoint target)
    {
        if (TargetPoint.FillBuffer(transform.localPosition, _targetingRange))
        {
            
            target = TargetPoint.GetBuffered(0);     
            return true;
        }
        target = null;
        return false;
    }

    protected private bool IsTargetTracked(ref TargetPoint target)
    {
        if(target == null)
        {
            return false;
        }

        Vector3 myPos = transform.localPosition;
        Vector3 targetPos = target.Position;
        if (Vector3.Distance(myPos, targetPos) > _targetingRange + target.ColliderSize + target.Enemy.Scale)
        {
            target = null;
            return false;
        }
        return true;
    }

    private IEnumerator instDeath()
    {
        yield return new WaitForSeconds(3f);
        Reclaim(this); 
    }

    public override void GameUpdate()
    {
        if (IsDead)
        {
            return;
        }

        if (!(IsAcquirateTarget(out TargetPoint target) && Health > 0f))
        {
            if (towerAnim != null)
            {
                towerAnim.Idle();
            }
        }

        if(Health <= 0f )
        {
            IsDead = true;
            towerAnim.Death();

            if (towerVFX != null)
            {
                towerVFX.Death();
            }

            StartCoroutine(instDeath());
            return;
        }                    
    }

    public void AnimationInit(GameTile tile)
    {
        towerAnim.Init(tile);
        towerVFX?.Init(tile);
    }

    public void TakeDamage(float damage)
    {
        DamageUI.sharedInstance.AddText((int)damage, transform.position);
        _health -= damage;
    }

    public void Reclaim(Tower tower)
    {
        Recycle();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.1f;
        Gizmos.DrawWireSphere(position, _targetingRange);
    }
}