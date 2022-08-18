using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHub : MonoBehaviour, IVFXController
{
    [SerializeField] private VFXController[] possiblePositions;

    [SerializeField] private VFXController defaultController;

    public void Init(GameTile tile)
    {
        switch (tile.PathDirection)
        {
            case DirectionExtensions.Direction.North:
                defaultController = possiblePositions[0];
                break;
            case DirectionExtensions.Direction.East:
                defaultController = possiblePositions[1];
                break;
            case DirectionExtensions.Direction.South:
                defaultController = possiblePositions[2];
                break;
            case DirectionExtensions.Direction.West:
                defaultController = possiblePositions[3];
                break;
            default:
                break;
        }
    }

    public void Attack()
    {
        defaultController.Attack();
    }

    public void Death()
    {
        defaultController.Death();
    }
}
