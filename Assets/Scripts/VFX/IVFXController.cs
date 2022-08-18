using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVFXController
{
    public void Init(GameTile tile);

    public void Attack();

    public void Death();
}
