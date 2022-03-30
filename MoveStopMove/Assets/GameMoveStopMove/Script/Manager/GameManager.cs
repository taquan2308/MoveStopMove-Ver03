using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>, IInitializeVariables
{
    //GameManager-LevelManager-OpenLevel-TryGame-OverGame-Score-level-ListEnemy - tagetFrame;
    public event Action<int> OnGameStarted = delegate { };
    public event Action OnLevelCompleted = delegate { };
    public event Action OnGameLose = delegate { };
    public event Action OnGameVictory = delegate { };
    public static int sceneIndex = 0;
    private PlayerMain playerMain;
    private int curentLevel;
    private bool gameStarted;
    private bool levelCompleted;
    private bool gameLose;
    private bool gameVictory;
    private int currentLevel;
    [SerializeField] private List<Character> listCharacter;
    [SerializeField] private List<Transform> listTransformCharacter;
    [SerializeField] private List<GameObject> listObstacle;
    public List<Character> ListCharacter { get => listCharacter; set => listCharacter = value; }
    public List<GameObject> ListObstacle { get => listObstacle; set => listObstacle = value; }
    public List<Transform> ListTransformCharacter { get => listTransformCharacter; set => listTransformCharacter = value; }
    [SerializeField] private Transform NearestEnemyFromPlayerTrans;
    //Canvas Game Over
    [SerializeField] private GameObject footTarget;
    private bool isPlay;
    private int killedCount;
    private int enemyAlive;
    [SerializeField] private SpawnEnemyManager spawnEnemyManager;
    private bool isFirtCountEnemy;
    private string[] arrayName;
    private int indexName;
    private string nameKillPlayer;
    public bool IsPlay { get => isPlay; set => isPlay = value; }
    public GameObject FootTarget { get => footTarget; set => footTarget = value; }
    public bool GameStarted { get => gameStarted; set => gameStarted = value; }
    public bool LevelCompleted { get => levelCompleted; set => levelCompleted = value; }
    public bool GameLose { get => gameLose; set => gameLose = value; }
    public int KilledCount { get => killedCount; set => killedCount = value; }
    public int EnemyAlive { get => enemyAlive; set => enemyAlive = value; }
    public string[] ArrayName { get => arrayName; set => arrayName = value; }
    public int IndexName { get => indexName; set => indexName = value; }
    public string NameKillPlayer { get => nameKillPlayer; set => nameKillPlayer = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public bool GameVictory { get => gameVictory; set => gameVictory = value; }
    
    private void Awake()
    {
        listCharacter = new List<Character>();
        ListTransformCharacter = new List<Transform>();
    }
    void Start()
    {
        InitializeVariables();//
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFirtCountEnemy)
        {
            isFirtCountEnemy = true;
            enemyAlive = spawnEnemyManager.ArrEnemyPrefabs.Length + ListCharacter.Count - 1;//false
            UIManager.Instance.UpdateAlives();
        }
        GameState();
    }
    private void GameState()
    {
        if (gameStarted)
        {
            gameStarted = false;
            OnGameStarted?.Invoke(currentLevel);
        }

        if (levelCompleted || listCharacter.Count == 1 && playerMain != null)
        {
            levelCompleted = false;
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings -1)
            {
                if (playerMain != null)
                {
                    if (playerMain.IsPlay == true)
                    {
                        OnGameVictory?.Invoke();
                        UIManager.Instance.OpenUI(UIName.Victory);
                        UIManager.Instance.CloseUI(UIName.Joystick);
                        UIManager.Instance.CloseUI(UIName.Live);
                        UIManager.Instance.CloseUI(UIName.Setting);
                        playerMain.IsPlay = false;
                        playerMain.PlayerAnimationGetterSetter.PlayWinAnim();
                    }
                }
            }
            else
            {
                if (playerMain != null)
                {
                    if (playerMain.IsPlay == true)
                    {
                        OnLevelCompleted?.Invoke();
                        playerMain.IsPlay = false;
                        playerMain.PlayerAnimationGetterSetter.PlayWinAnim();
                    }
                }
            }
        }

        if (gameLose && listCharacter.Count > 0)
        {
            playerMain.Gold += killedCount * 10;
            PlayerPrefs.SetInt("GoldPlayer", playerMain.Gold);
            gameLose = false;
            footTarget.SetActive(false);
            OnGameLose?.Invoke();
        }
    }
    public void NextLevel()
    {
        playerMain.Gold += killedCount * 10;
        PlayerPrefs.SetInt("GoldPlayer", playerMain.Gold);
        sceneIndex++;
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            SceneManager.LoadScene(Mathf.Clamp(SceneManager.GetActiveScene().buildIndex + 1, 0, SceneManager.sceneCountInBuildSettings - 1));
        }
    }
    public void InitializeVariables()
    {
        //name of enemy
        arrayName = new string[] { "Jorja", "Clifford", "Howells", "Callen", "Spooner", "Miranda", "Orozco", "Melisa", "Mcmillan", "Rea", "Trevino", "Kyron", "Welch", "Dean", "Abid", "Barr", "Adkins", "Esmae ", "Novak", "Tyrese", "Kinney", "Porter", "Serenity", "Evans", "Margot", "Tanisha", "Coles", "Stewart", "Vang", "Freya", "Beard", "Calista", "Currie", "Hettie", "Merrill", "Harmony", "William", "Pike", "Rita", "Weronika", "Bateman", "Caine", "Hicks", "Finnian", "Buck", "Jai", "Terrell", "Wong", "Caelan", "Whittle", "Ebrahim", "Kearns", "Quinn", "Witt", "Hettie", "Paloma", "Barnard", "Cunningham", "York" };
        indexName = 0;
        Application.targetFrameRate = 60;
        playerMain = PlayerMain.Instance;
        NearestEnemyFromPlayerTrans = playerMain.NearestEnemyFromPlayerTrans;//
        // canvas
        isPlay = false;
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        gameStarted = false;
        levelCompleted = false;
        gameLose = false;
        gameVictory = false;
        killedCount = 0;
        isFirtCountEnemy = false;
    }
}
