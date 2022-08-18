using System;
using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    private GameTileContentType _type;

    public GameTileContentType Type => _type;
    public GameTileContentFactory OriginFactory { get; set; }

    public event Action<GameTileContent> onRecycle;

    //public bool IsBlockingPath => Type > GameTileContentType.BeforeBlockers;
    public bool IsBlockingPath => Type != GameTileContentType.Destination
        && Type != GameTileContentType.Empty
        && Type != GameTileContentType.SpawnPoint
        && Type != GameTileContentType.LaserTower
        && Type != GameTileContentType.MortarTower
        && Type != GameTileContentType.GunnarTower;
    public void Recycle()
    {
        onRecycle?.Invoke(this);
        OriginFactory.Reclaim(this);
    }

    public virtual void GameUpdate()
    {

    }

}

public enum GameTileContentType
{
    Empty,
    Destination,
    Wall,
    SpawnPoint,
    LaserTower,
    MortarTower,
    Bush,
    Fence,
    Fence1,
    Fence2,
    Fence3,
    Fence4,
    Fence5,
    Fence6,
    WaterTower,
    Car,
    GunnarTower
}
public enum TowerType
{
    Laser,
    Mortar,
    Gunner
}
