using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class QuickGame : MonoBehaviour//, ICleanUp
{
    [SerializeField]
    private Vector2Int _boardSize;

    [SerializeField]
    private GameBoard _board;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private WarFactory _warFactory;

    [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField]
    private GameScenario _scenario;

    [SerializeField, Range(0, 100)]
    private int _startingPlayerHealth = 100;

    private bool _scenarioInProcess;
    private GameScenario.State _activeScenario;
    private CancellationTokenSource _prepareCancellation;

    [SerializeField]
    private bool _fromSave = false;
    [SerializeField]
    private string _fileName;

    //private SceneInstance _environment; //

    private int _playerHealth = 100;

    private int _currentPlayerHealth;

    //[SerializeField]
    //private GameScenario.State _activeScenario;

    private readonly GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private readonly GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private readonly BoardSerializer _serializer = new BoardSerializer();
    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private GameTileContentType _currentTowerType;

    private static QuickGame _instance;
    private bool _isPaused;
    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = Mathf.Max(0, value);
            //_defenderHud.UpdatePlayerHealth(_playerHealth, _startingPlayerHealth);
        }
    }

    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]{_contentFactory,
        _warFactory, _enemyFactory};

    //public string SceneName => Constants.Scenes.QUICK_GAME;


    private void OnEnable()
    {
        _instance = this;
    }
    /*
    public void Init(SceneInstance environment)
    {
        _environment = environment;
        //_defenderHud.PauseClicked += OnPauseClicked;
        //_defenderHud.QuitGame += GoToMainMenu;
        var initialData = GenerateInitialData();
        _board.Initialize(initialData, _contentFactory);
       // _tilesBuilder.Initialize(_contentFactory, _camera, _board, false);
    }
    
    private BoardData GenerateInitialData()
    {
        var result = new BoardData
        {
            X = (byte)_boardSize.x,
            Y = (byte)_boardSize.y,
            Content = new GameTileContentType[_boardSize.x * _boardSize.y]
        };
        result.Content[0] = GameTileContentType.SpawnPoint;
        result.Content[result.Content.Length - 1] = GameTileContentType.Destination;
        return result;
    }*/
    private void OnPauseClicked(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }

        if (_scenarioInProcess)
        {
            var waves = _activeScenario.GetWaves();
            _defenderHud.UpdateScenarioWaves(waves.currentWave, waves.wavesCount);
            if (PlayerHealth <= 0)
            {
                _scenarioInProcess = false;
                _gameResultWindow.Show(GameResultType.Defeat, BeginNewGame, GoToMainMenu);
            }
            if (_activeScenario.Progress() == false && _enemies.IsEmpty)
            {
                _scenarioInProcess = false;
                _gameResultWindow.Show(GameResultType.Victory, BeginNewGame, GoToMainMenu);
            }
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }*/


    private void Start()
    {
        if (_fromSave)
        {
            var initialData = GenerateInitialData();
            _board.Initialize(initialData, _contentFactory);
        }
        else
        {
            _board.Initialize(_boardSize, _contentFactory);
        }

        BeginNewGame();
    }

    private BoardData GenerateInitialData()
    {
        var result = _serializer.Load(_fileName);
        if (result == null || _fileName == "")
        {
            result = new BoardData
            {
                X = (byte)_boardSize.x,
                Y = (byte)_boardSize.y,
                Content = new GameTileContentType[_boardSize.x * _boardSize.y]
            };
            result.Content[0] = GameTileContentType.SpawnPoint;
            result.Content[result.Content.Length - 1] = GameTileContentType.Destination;
        }
        return result;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = GameTileContentType.LaserTower;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = GameTileContentType.MortarTower;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentTowerType = GameTileContentType.GunnarTower;
        }
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }
        if (_currentPlayerHealth <= 0)
        {
            Debug.Log("Defeated");
            BeginNewGame();
        }
        //Debug.Log (_currentPlayerHealth);
        if (!_activeScenario.Progress() && _enemies.IsEmpty)
        {
            Debug.Log("Victory");
            //BeginNewGame();
            _activeScenario.Progress();
        }
        //_activeScenario.Progress();

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        GameTile spawnPoint = _instance._board.GetSpawnPoint(Random.Range(0, _instance._board.SpawnPointCount));
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnPoint);
        _instance._enemies.Add(enemy);
    }/*
    public static void SpawnEnemy(EnemyFactory factory, EnemyType enemyType)
    {
        var spawnPoint = _instance._board.GetRandomSpawnPoint();
        var enemy = factory.Get(enemyType);
        enemy.SpawnOn(spawnPoint);
        _instance._enemies.Add(enemy);
    }*/

    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _currentTowerType);
            }
            else
            {
                _board.ToggleWall(tile); //?????
            }
        }
    }
    private void HandleAlternativeTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile); //Destination
            }
            else
            {
                _board.ToggleSpawnPoint(tile); //SPAWN POINT
            }
            //tile.Content = _contentFactory.Get(GameTileContentType.Destination);
        }
    }

    public static Shell SpawnShell()
    {
        Shell shell = _instance._warFactory.Shell;
        _instance._nonEnemies.Add(shell);
        return shell;
    }
    public static Bullet SpawnBullet()
    {

        Bullet bullet = _instance._warFactory.Bullet;
        _instance._nonEnemies.Add(bullet);
        return bullet;
    }
    public static Arrow SpawnArrow()
    {

        Arrow arrow = _instance._warFactory.Arrow;
        _instance._nonEnemies.Add(arrow);
        return arrow;
    }
    public static Explosion SpawnExplosion()
    {
        var shell = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(shell);
        return shell;
    }
    public static void EnemyReachedDestination()
    {
        //Debug.Log("House_hp = " + _instance.PlayerHealth);
        _instance.PlayerHealth--;
    }
    /*
    public async void BeginNewGame()
    {
        Cleanup();
        _tilesBuilder.Enable();
        PlayerHealth = _startingPlayerHealth;

        try
        {
            _prepareCancellation?.Dispose();
            _prepareCancellation = new CancellationTokenSource();
            if (await _prepareGamePanel.Prepare(_prepareTime, _prepareCancellation.Token))
            {
                _activeScenario = _scenario.Begin();
                _scenarioInProcess = true;
            }
        }
        catch (TaskCanceledException _) { }
    }
    */
    public void Cleanup()
    {
        //_tilesBuilder.Disable();
        _scenarioInProcess = false;
        _prepareCancellation?.Cancel();
        _prepareCancellation?.Dispose();
        _enemies.Clear();
        _nonEnemies.Clear();

        if (_fromSave)
            _board.ClearEditor();
        else
            _board.Clear();

    }
    /*
    private void GoToMainMenu()
    {
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(new ClearGameOperation(this));
        ProjectContext.Instance.AssetProvider.UnloadAdditiveScene(_environment);
        ProjectContext.Instance.LoadingScreenProvider.LoadAndDestroy(operations);
    }
    
    
    public static Explosion SpawnExplosion()
    {
        Explosion shell = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(shell);
        return shell;
    }*/

    private void BeginNewGame()
    {
        _enemies.Clear();
        _nonEnemies.Clear();
        _currentPlayerHealth = _startingPlayerHealth;
        _activeScenario = _scenario.Begin();

        if (_fromSave)
            _board.ClearEditor();
        else
            _board.Clear();
    }

}
