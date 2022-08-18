using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnim3rdSetting : MonoBehaviour, ITowerAnim
{  
    [SerializeField] private Animator _animator;

    public void Init(GameTile tile)
    {
        return; 
    }

    private IEnumerator RunAttack()
    {
        _animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.9f);
        _animator.SetBool("attack", false);
        _animator.SetTrigger("finishing");
    }

    public void Attack()
    {
        StartCoroutine(RunAttack());       
    }

    public void Death()
    {
        _animator.SetBool("attack", false);
        _animator.SetBool("damage", true);
        _animator.SetTrigger("death");
    }

    public void Idle()
    {   
        _animator.SetBool("attack", false);
    }
}
