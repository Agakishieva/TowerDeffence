using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private Transform _ground;
    [SerializeField]
    private GameTile _tilePrefab;

    private Vector2Int _size;

    private GameTile[] _tiles;

    private readonly Queue<GameTile> _searchFrontier = new Queue<GameTile>();
    private GameTileContentFactory _contentFactory;

    //private readonly List<GameTile> _spawnPoints = new List<GameTile>();
    //private readonly List<GameTileContent> _contentToUpdate = new List<GameTileContent>();

    private BoardData _boardData;
    private byte X => _boardData.X;
    private byte Y => _boardData.Y;

    private List<GameTile> _spawnPoints = new List<GameTile>();
    public IEnumerable<GameTile> spawnPoints => _spawnPoints;

    private List<GameTileContent> _contentToUpdate = new List<GameTileContent>();
    public int SpawnPointCount => _spawnPoints.Count;
    private int[] _points = new int[] { 3, 14, 25, 35, 34, 55, 56, 57, 48, 59, 70, 49, 50, 40, 28, 16, 7, 19, 20, 31, 80, 68, 113, 102, 104, 120, 118, 107, 108, 86, 83, 84, 74, 64, 82 };


    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        _size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2(((size.x - 1) * 0.5f), ((size.y - 1) * 0.5f));
        _tiles = new GameTile[size.x * size.y];
        _contentFactory = contentFactory;
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }

                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative == false;
                }
            }
        }
        //ToggleDestination(_tiles[_tiles.Length / 2]);
        //ToggleSpawnPoint(_tiles[0]);
        Clear();
       // StartSpawn();

        //FindPaths();

    }

    #region EditorSpecific

    public void Initialize(BoardData boardData, GameTileContentFactory contentFactory)
    {
        _boardData = boardData;
        var offset = new Vector2((X - 1) * 0.5f, (Y - 1) * 0.5f);

        _tiles = new GameTile[X * Y];
        _contentFactory = contentFactory;
        for (int i = 0, y = 0; y < Y; y++)
        {
            for (int x = 0; x < X; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }

                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - X]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = tile.IsAlternative == false;
                }
            }
        }

        ClearEditor();
    }

    public void ClearEditor()
    {
        _spawnPoints.Clear();
        _contentToUpdate.Clear();

        for (var i = 0; i < _boardData.Content.Length; i++)
        {
            ForceBuild(_tiles[i], _contentFactory.Get(_boardData.Content[i]));
        }

        FindPaths();
    }

    public void ForceBuild(GameTile tile, GameTileContent content)
    {
        tile.Content = content;
        _contentToUpdate.Add(content);
        content.onRecycle += RemoveContent;

        if(content.Type == GameTileContentType.MortarTower || content.Type == GameTileContentType.LaserTower
            || content.Type == GameTileContentType.GunnarTower)
        {
            content.gameObject.GetComponent<Tower>().AnimationInit(tile);
        }

        if (content.Type == GameTileContentType.SpawnPoint)
            _spawnPoints.Add(tile);
    }

    public bool TryBuild(GameTile tile, GameTileContent content)
    {
        if (tile.Content.Type != GameTileContentType.Empty)
            return false;

        tile.Content = content;
        if (FindPaths() == false)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            return false;
        }

        _contentToUpdate.Add(content);
        content.onRecycle += RemoveContent;

        if (content.Type == GameTileContentType.SpawnPoint)
            _spawnPoints.Add(tile);

        return true;
    }

    #endregion

    private void StartSpawn()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            ToggleWall(_tiles[_points[i]]);
        }
        ToggleDestination(_tiles[116]);
        ToggleSpawnPoint(_tiles[110]);
        ToggleSpawnPoint(_tiles[120]);
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _contentToUpdate.Count; i++)
        {
            _contentToUpdate[i].GameUpdate();
        }
    }
    private bool FindPaths()
    {
        foreach (var t in _tiles)
        {
            if (t.Content.Type == GameTileContentType.Destination)
            {
                t.BecomeDestination();
                _searchFrontier.Enqueue(t);
            }
            else
            {
                t.ClearPath();
            }
        }
        if (_searchFrontier.Count == 0)
        {
            return false;
        }

        while (_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }

                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (var t in _tiles)
        {
            if (!t.HasPath)
            {
                return false;
            }
        }

        foreach (var t in _tiles)
        {
            t.ShowPath();
        }

        return true;

    }
    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                FindPaths();
            }

        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }
    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if (tile.Content = _contentFactory.Get(GameTileContentType.Empty))
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if (!FindPaths())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
    }


    public void ToggleTower(GameTile tile, GameTileContentType towerType)
    {
        if (tile.Content.Type == GameTileContentType.LaserTower || tile.Content.Type == GameTileContentType.MortarTower)
        {
            _contentToUpdate.Remove(tile.Content);
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if (tile.Content = _contentFactory.Get(GameTileContentType.Empty))
        {
            //tile.Content = _contentFactory.Get(GameTileContentType.Tower);
            tile.Content = _contentFactory.Get(towerType);

            if (FindPaths())
            {
                _contentToUpdate.Add(tile.Content);
                tile.Content.onRecycle += RemoveContent;
            }
            else
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            //tile.Content = _contentFactory.Get(GameTileContentType.Tower);
            tile.Content = _contentFactory.Get(towerType);
            _contentToUpdate.Add(tile.Content);
            tile.Content.onRecycle += RemoveContent;
        }
    }

    public void DestroyTile(GameTile tile)
    {
        if (tile.Content.Type <= GameTileContentType.Empty)
            return;

        _contentToUpdate.Remove(tile.Content);

        if (tile.Content.Type == GameTileContentType.SpawnPoint)
            _spawnPoints.Remove(tile);

        tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        FindPaths();
    }

    public GameTile GetTileFromBuilder(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1024))
        {
            var x = (int)(hit.point.x + X * 0.5f);
            var y = (int)(hit.point.z + Y * 0.5f);
            if (x >= 0 && x < X && y >= 0 && y < Y)
            {
                return _tiles[x + y * X];
            }
        }
        return null;
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            if (_spawnPoints.Count > 1)
            {
                _spawnPoints.Remove(tile);
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
            _spawnPoints.Add(tile);
        }
    }

    public GameTile GetTile(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1))
        {
            var x = (int)(hit.point.x + _size.x * 0.5f);
            var y = (int)(hit.point.z + _size.y * 0.5f);
            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                Debug.Log("Point = " + (x + y * _size.x));
                return _tiles[x + y * _size.x];

            }
        }
        return null;
    }
    public GameTile GetRandomSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }
    public GameTile GetSpawnPoint(int index)
    {
        // Debug.Log("Point = " + _spawnPoints[index]);

        return _spawnPoints[index];
    }
    public void Clear()
    {
        foreach (var tile in _tiles)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            tile.Content.onRecycle -= RemoveContent;
        }
        _spawnPoints.Clear();
        _contentToUpdate.Clear();
        ToggleDestination(_tiles[_tiles.Length / 2]);
        ToggleSpawnPoint(_tiles[6]);
        ToggleSpawnPoint(_tiles[78]);
        ToggleSpawnPoint(_tiles[90]);
        ToggleSpawnPoint(_tiles[162]);
    }

    private void RemoveContent(GameTileContent content)
    {
        _contentToUpdate.Remove(content);
    }

    public GameTileContentType[] GetAllContent => _tiles.Select(t => t.Content.Type).ToArray();
}
