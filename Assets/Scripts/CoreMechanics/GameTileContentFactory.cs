using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] private GameTileContent _destinationPrefab;
    [SerializeField] private GameTileContent _emptyPrefab;
    [SerializeField] private GameTileContent _wallPrefab;
    [SerializeField] private GameTileContent _spawnPrefab;
    [SerializeField] private GameTileContent _bush;
    [SerializeField] private GameTileContent _fence;
    [SerializeField] private GameTileContent _fence1;
    [SerializeField] private GameTileContent _fence2;
    [SerializeField] private GameTileContent _fence3;
    [SerializeField] private GameTileContent _fence4;
    [SerializeField] private GameTileContent _fence5;
    [SerializeField] private GameTileContent _fence6;
    [SerializeField] private GameTileContent _waterTower;
    [SerializeField] private GameTileContent _car;
    

    [SerializeField] private Tower _laserTower;
    [SerializeField] private Tower _GunnarTower;
    [SerializeField] private Tower _mortarTower;

    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }
    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination:
                return Get(_destinationPrefab);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(_spawnPrefab);
            case GameTileContentType.LaserTower:
                return Get(_laserTower);
            case GameTileContentType.MortarTower:
                return Get(_mortarTower);
            case GameTileContentType.Bush:
                return Get(_bush);
            case GameTileContentType.Fence:
                return Get(_fence);
            case GameTileContentType.Fence1:
                return Get(_fence1);
            case GameTileContentType.Fence2:
                return Get(_fence2);
            case GameTileContentType.Fence3:
                return Get(_fence3);
            case GameTileContentType.Fence4:
                return Get(_fence4);
            case GameTileContentType.Fence5:
                return Get(_fence5);
            case GameTileContentType.Fence6:
                return Get(_fence6);
            case GameTileContentType.WaterTower:
                return Get(_waterTower);
            case GameTileContentType.Car:
                return Get(_car);
            case GameTileContentType.GunnarTower:
                return Get(_GunnarTower);
        }
        return null;
    }

    public T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }

}
