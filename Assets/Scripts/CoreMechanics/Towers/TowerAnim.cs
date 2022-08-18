using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnim : MonoBehaviour, ITowerAnim
{
    [SerializeField] private Animator[] _possiblePositions;
    
    [SerializeField] private Animator _animator;

    public void Init(GameTile tile)
    {
        _animator.gameObject.SetActive(false);
        switch (tile.PathDirection)
        {
            case DirectionExtensions.Direction.North:
                _animator = _possiblePositions[0];
                break;
            case DirectionExtensions.Direction.East:
                _animator = _possiblePositions[1];
                break;
            case DirectionExtensions.Direction.South:
                _animator = _possiblePositions[2];
                break;
            case DirectionExtensions.Direction.West:
                _animator = _possiblePositions[3];
                break;
            default:
                break;
        }

        _animator.gameObject.SetActive(true);
    }

    public void Attack()
    {
        _animator.SetBool("attack", true);
    }

    public void Death()
    {
        _animator.SetBool("attack", false);
        _animator.SetTrigger("death");
        _animator.SetBool("death", true);
    }

    public void Idle()
    {   
        _animator.SetBool("attack", false);
    }
}
