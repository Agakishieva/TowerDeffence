using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DirectionExtensions;

public class EnemyAnimation3rd : MonoBehaviour, IEnemyAnimation
{
    private Animator animator;
    private Direction direction;
    private int health;

    public void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetDirection(Direction _direction)
    {
        direction = _direction;
        PlayWalkAnimation();
    }

    public void SetHealth(int _health)
    {
        health = _health;
        PlayDeathAnimation();
    }
    public void PlayWalkAnimation()
    {
        animator.SetBool("walk", true);
        animator.SetBool("attack", false);
    }

    public void PlayAttackAnimation()
    {
        animator.SetBool("attack", true);
        animator.SetBool("walk", false);
    }

    public void PlayDeathAnimation()
    {
        if (health > 0)
        {
            return;
        }
        animator.SetBool("walk", false);
        animator.SetBool("attack", false);
        animator.SetTrigger("death");
    }
}
