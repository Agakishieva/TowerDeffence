using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NormalGame : MonoBehaviour//, ICleanUp
{
    [SerializeField]
    private GameBoard _board;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private UnitController _unitController;

    [SerializeField]
    private UIPlayScene _uiPlay;

    [SerializeField]
    private GameTileContentFactory _contentFactory;

    [SerializeField]
    private WarFactory _warFactory;

    [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField]
    private GameScenario _scenario;

    [SerializeField, Range(0, 100)]
    private int _startingPlayerHealth = 2;

    private bool _scenarioInProcess;
    private GameScenario.State _activeScenario;
    private CancellationTokenSource _prepareCancellation;

    //private SpawnPoint _pickedSpawn;
    //private GameTile _pickedSpawnTile;

    [SerializeField]
    private string _fileName;

    private int _playerHealth;

    private int _currentPlayerHealth;

    private readonly GameBehaviorCollection _enemies = new GameBehaviorCollection();
    private readonly GameBehaviorCollection _nonEnemies = new GameBehaviorCollection();

    private readonly BoardSerializer _serializer = new BoardSerializer();
    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private static NormalGame _instance;
    private bool _isPaused;

    public event Action allUnitsDead;

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

    private void OnEnable()
    {
        _instance = this;

        _enemies.emptyBehaviors += InvokeAllUnitsDead;
    }

    private void OnDisable()
    {
        _enemies.emptyBehaviors -= InvokeAllUnitsDead;
    }

    private void InvokeAllUnitsDead()
    {
        allUnitsDead.Invoke();
    }

    private void OnPauseClicked(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }


    private void Start()
    {
        var initialData = GenerateInitialData();
        _board.Initialize(initialData, _contentFactory);
        _unitController.Init();


        BeginNewGame();
    }

    private BoardData GenerateInitialData()
    {
        var result = _serializer.LoadRes(_fileName);
        
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
        

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }

    public static void SpawnEnemy(Unit type)
    {
        foreach(GameTile spawn in _instance._board.spawnPoints)
        {
            Enemy enemy = _instance._enemyFactory.Get(type);
            enemy.SpawnOn(spawn);
            enemy.onDestinationReached += EnemyReachedDestination;
            _instance._enemies.Add(enemy);
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
        _instance._uiPlay.ShowWin();
        _instance.Cleanup();
    }
    
    public void Cleanup()
    {
        //_tilesBuilder.Disable();
        _scenarioInProcess = false;
        _prepareCancellation?.Cancel();
        _prepareCancellation?.Dispose();
        _enemies.Clear();
        // _nonEnemies.Clear(); // disabled for demo
        // _board.ClearEditor(); // disabled for demo
    }
    

    private void BeginNewGame()
    {
        _enemies.Clear();
        _nonEnemies.Clear();
        _currentPlayerHealth = _startingPlayerHealth;

        _board.ClearEditor();
    }

    public void OnStartScenarioPress()
    {
        _activeScenario = _scenario.Begin();
    }

}
