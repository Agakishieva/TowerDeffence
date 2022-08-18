using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerAnim
{
    public void Attack();
    public void Death();
    public void Idle();
    public void Init(GameTile tile);
}
