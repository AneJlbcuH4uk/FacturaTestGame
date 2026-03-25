using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PathManager pathManager;
    [SerializeField] private Transform TilesHolder;
    [SerializeField] private WorldGridBuilder gridBuilder;
    [SerializeField] private FlowFieldManager flowField;
    [SerializeField] private float gridSize;
    [SerializeField] private EnemyManager enemyManager;

    private List<Tile> tiles = new();
    private WorldGrid grid;
    private Vector2Int curCarTile = Vector2Int.zero;
    private Transform carTransform;

    [SerializeField] private GameObject resultUI;
    [SerializeField] private TMP_Text resultTextUI;
    [SerializeField] private Button restartButton;

    private bool gameEnded = false;

    [SerializeField] private Car car;
    private CarController carController;

    private bool gameStarted = false;

    [SerializeField] private GameObject GameInfoUI;
    [SerializeField] private UIHealthBar healthBar;
    [SerializeField] private UIHealthBar progressBar;

    private void Awake()
    {

        for (int i = 0; i < TilesHolder.childCount; i++) 
        {
            var tileGameObject = TilesHolder.GetChild(i);
            if(tileGameObject.GetComponent<Tile>() is Tile tile)
                tiles.Add(tile);
        }
    }

    void Start()
    {
        grid = gridBuilder.BuildWorldGrid(tiles, gridSize);
        flowField.Init(grid);
        pathManager.GeneratePath(tiles);

        carController = car.GetComponent<CarController>();
        carController.Init(pathManager.PathPoints);
        carTransform = car.gameObject.transform;

        car.OnCarDestroyed += () => LoseGame();
        carController.OnPathComplete += () => WinGame();
        restartButton.onClick.AddListener(RestartGame);
        car.OnHealthChanged += healthBar.SetValue;
        carController.OnProgressChanged += progressBar.SetValue;

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(gameStarted)
            TrackChanges();
    }

    private void TrackChanges() 
    {
        Vector2Int currentTile = grid.WorldToGrid(carTransform.position);
        if (curCarTile != currentTile)
        {
            curCarTile = currentTile;
            flowField.UpdateField(carTransform.position, carTransform.forward);
            enemyManager.MoveSpawnWindow(carTransform.position.z);
        }
        carController.MoveAlongPath();
    }


    public void StartGame() 
    {
        if (gameStarted) return;

        gameStarted = true;
        Time.timeScale = 1f;

        GameInfoUI.SetActive(true);
        enemyManager.StartSpawning(pathManager.GetLastPoint());
    }
    

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        enemyManager.StopSpawning();
        Time.timeScale = 0f;

        GameInfoUI.SetActive(false);
        resultUI.SetActive(true);

        if (resultTextUI != null)
        {
            resultTextUI.text = "You Win";
        }
    }

    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        enemyManager.StopSpawning();
        Time.timeScale = 0f;

        GameInfoUI.SetActive(false);
        resultUI.SetActive(true);

        if (resultTextUI != null)
        {
            resultTextUI.text = "You Lose";
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
