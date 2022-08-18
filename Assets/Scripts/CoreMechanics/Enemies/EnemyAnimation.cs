using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DirectionExtensions;

public class EnemyAnimation : MonoBehaviour, IEnemyAnimation
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
        if (direction == Direction.South)
        {
            animator.SetTrigger("NW");
            animator.SetBool("attackNW", false);
            animator.SetBool("walkNW", true);
        }
        else if (direction == Direction.West)
        {
            animator.SetTrigger("NE");
            animator.SetBool("attackNE", false);
            animator.SetBool("walkNE", true);
        }
        else if (direction == Direction.North)
        {
            animator.SetTrigger("SE");
            animator.SetBool("attackSE", false);
            animator.SetBool("walkSE", true);
        }
        else if (direction == Direction.East)
        {
            animator.SetTrigger("SW");
            animator.SetBool("attackSW", false);
            animator.SetBool("walkSW", true);
        }
    }
    public void PlayAttackAnimation()
    {
        if (direction == Direction.South)
        {
            animator.SetTrigger("NW");
            animator.SetBool("walkNW", false);
            animator.SetBool("attackNW", true);
        }
        else if (direction == Direction.West)
        {
            animator.SetTrigger("NE");
            animator.SetBool("walkNE", false);
            animator.SetBool("attackNE", true);
        }
        else if (direction == Direction.North)
        {
            animator.SetTrigger("SE");
            animator.SetBool("walkSE", false);
            animator.SetBool("attackSE", true);
        }
        else if (direction == Direction.East)
        {
            animator.SetTrigger("SW");
            animator.SetBool("walkSW", false);
            animator.SetBool("attackSW", true);
        }

        AudioService.unit.MeeleAttack();
    }

    public void PlayDeathAnimation()
    {

        if(health > 0)
        {
            return;
        }

        if (direction == Direction.South)
        {
            animator.SetTrigger("NW");
            animator.SetBool("walkNW", false);
            animator.SetBool("attackNW", false);
            animator.SetBool("deathNW", true);
        }

        else if (direction == Direction.East)
        {
            animator.SetTrigger("SW");
            animator.SetBool("walkSW", false);
            animator.SetBool("attackSW", false);
            animator.SetBool("deathSW", true);
        }

        else if (direction == Direction.North)
        {
            animator.SetTrigger("SE");
            animator.SetBool("walkSE", false);
            animator.SetBool("attackSE", false);
            animator.SetBool("deathSE", true);
        }

        else if (direction == Direction.West)
        {
            animator.SetTrigger("NE");
            animator.SetBool("walkNE", false);
            animator.SetBool("attackNE", false);
            animator.SetBool("deathNE", true);
        }
        AudioService.unit.Death();
    }

}
