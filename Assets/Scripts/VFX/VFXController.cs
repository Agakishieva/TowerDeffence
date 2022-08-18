using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour, IVFXController
{
    [SerializeField] private VFXStore deathVFX;
    [SerializeField] private VFXStore attackVFX;

    public void Init(GameTile tile)
    {
        // do nothing
    }

    public void Attack()
    {
        attackVFX?.Run();
    }

    public void Death()
    {
        deathVFX?.Run();
    }
}
