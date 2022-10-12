using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SDD.Events;
using static Tools;
using UnityEditor;


public class GameManager : Manager<GameManager>, IEventHandler
{
    [Tooltip("Unsigned Int")]
    [SerializeField] private GameScene m_CurrentScene = GameScene.MENUSCENE;

    [SerializeField] private bool m_IsDebugMode = false;

    private IEnumerator m_GameManagerCoroutine;


    /// <summary>
    /// Game Score
    /// </summary>
    private static int m_Score;

    /// <summary>
    /// Current Score on the game
    /// </summary>
    private int m_CurrentScore;

    #region GameState Properties

    /**
     * <summary>The game state</summary>
     */
    private static GameState m_GameState;

    public static bool IsMenu { get => GameManager.m_GameState.Equals(GameState.MENU); }
    public static bool IsPlaying { get => GameManager.m_GameState.Equals(GameState.PLAY); }
    public static bool IsPausing { get => GameManager.m_GameState.Equals(GameState.PAUSE); }
    public static bool IsWinning { get => GameManager.m_GameState.Equals(GameState.WIN); }
    public static bool IsGameOver { get => GameManager.m_GameState.Equals(GameState.GAMEOVER); }
    public static bool IsOnPlaying { get => GameManager.IsPlaying || GameManager.IsPausing; }

    #endregion

    #region GameScene Properties
    public bool IsMenuScene { get => this.m_CurrentScene.Equals(GameScene.MENUSCENE); }
    public bool IsFirstLevelScene { get => this.m_CurrentScene.Equals(GameScene.FIRSTLEVELSCENE); }
    public bool IsHelpScene { get => this.m_CurrentScene.Equals(GameScene.HELPSCENE); }
    public bool IsCreditScene { get => this.m_CurrentScene.Equals(GameScene.CREDITSCENE); }
    #endregion

    #region Event Listeners Methods

    /**
    * <summary>Handle the NewGameButtonClickedEvent</summary>
    * <remarks>Call the new game methods <see cref="NewGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnNewGameButtonClickedEvent(NewGameButtonClickedEvent e)
    {
        this.NewGame();
    }

    /**
    * <summary>Handle the MainMenuButtonClickedEvent</summary>
    * <remarks>Call the Menu methods <see cref="Menu"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnMainMenuButtonClickedEvent(MainMenuButtonClickedEvent e)
    {
        this.Menu();
    }

    /**
    * <summary>Handle the HelpButtonClickedEvent</summary>
    * <remarks>Call the help methods <see cref="ExitGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnHelpButtonClickedEvent(HelpButtonClickedEvent e)
    {
        this.Help();
    }

    /**
    * <summary>Handle the CreditButtonClickedEvent</summary>
    * <remarks>Call the credit game methods <see cref="ExitGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnCreditButtonClickedEvent(CreditButtonClickedEvent e)
    {
        this.CreditGame();
    }

    /**
    * <summary>Handle the ExitButtonClickedEvent</summary>
    * <remarks>Call the exit game methods <see cref="ExitGame"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnExitButtonClickedEvent(ExitButtonClickedEvent e)
    {
        this.ExitGame();
    }

    /**
    * <summary>Handle the LevelGameOverEvent</summary>
    * <remarks>Call the game over methods <see cref="GameOver"/></remarks>
    * <param name="e">The event</param> 
    */
    private void OnLevelGameOverEvent(LevelGameOverEvent e)
    {
        this.GameOver();
    }

    /**
    * <summary>Handle the ObjectWillGainScoreEvent</summary>
    * <remarks>If the collided GO is the ThrowableObject so we earn time</remarks>
    * <param name="e">The event</param> 
    */
    private void OnObjectWillGainScoreEvent(ObjectWillGainScoreEvent e)
    {
        bool isPlayer = e.eOtherGO.CompareTag("Player");
        if (isPlayer && GameManager.IsPlaying)
        {
            this.EarnScore(e.eThisGameObject);

            if (isPlayer)
            {
                e.eOtherGO.SetActive(false); // Desativate the ThrowableObject when hit ObjectWillGainScore
            } 
        }
    }

    private void OnContinueGameEvent(ContinueGameEvent e)
    {
        this.PlayGame();
    }
    #endregion

    #region  GameMangers Utils Methods
    /// <summary>
    /// Play the game
    /// </summary>
    private void PlayGame()
    {
        Time.timeScale = 1;
        SetGameState(Tools.GameState.PLAY);
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0;
        SetGameState(Tools.GameState.PAUSE);
    }

    /**
     * <summary>Go to the victory scene</summary> 
     */
    private void VictoryGame()
    {
        if (!GameManager.IsWinning && !GameManager.IsGameOver) return;

        this.LoadALevel(GameScene.VICTORYSCENE, false);
    }

    /**
     * <summary>Switch to GAMEOVER and go to the victory scene</summary> 
     */
    private void GameOver()
    {
        this.SetGameState(GameState.GAMEOVER);
        this.VictoryGame();
    }

    /**
     * <summary>Start a new Game</summary> 
     * <remarks>Save the game, reset all games variables and load the first LEVEL</remarks>
     */
    private void NewGame()
    {
        this.LoadALevel(GameScene.FIRSTLEVELSCENE);
    }

    /**
     * <summary>Earn score on game</summary>
     * <param name="gameObject">The game object with Iscore interface</param>
     */
    private void EarnScore(GameObject gameObject)
    {
        if (!gameObject) return;

        int totalNewlyGainedScore = this.m_CurrentScore;
        IScore[] scores = gameObject.GetComponentsInChildren<IScore>();

        for (int i = 0; i < scores.Length; i++)
        {
            totalNewlyGainedScore += scores[i].Score;
        }

        SetScore(totalNewlyGainedScore);
    }

    /**
     * <summary>Reset the game</summary> 
     */
    private void ResetGame()
    {
        this.ResetGameVar();
        this.LoadALevel(this.m_CurrentScene);
    }

    /**
     * <summary>Reset the game variable</summary> 
     */
    private void ResetGameVar()
    {
        this.SetGameState(GameState.PLAY);
    }

    /**
     * <summary>Load a LVL</summary>
     * <remarks>Start a couroutine to wait a time for event can play</remarks>
     * <param name="gameScene">The gameScene to load</param>
     */
    private void LoadALevel(GameScene gameScene)
    {
        this.LoadALevel(gameScene, true);
    }

    /// <summary>
    /// Load a LVL
    /// </summary>
    /// <param name="gameScene">The gameScene to load</param>
    /// <param name="waitToLoad">If we wait before loading</param>
    private void LoadALevel(GameScene gameScene, bool waitToLoad)
    {
        if (waitToLoad)
        {
            this.m_GameManagerCoroutine = Tools.MyWaitCoroutine(1, null, () => this.SetGameScene(gameScene));
            StartCoroutine(this.m_GameManagerCoroutine);
        }
        else
        {
            this.SetGameScene(gameScene);
        }
    }

    /**
     * <summary>Load the Menu</summary>
     */
    private void Menu()
    {
        this.PlayGame();
        this.LoadALevel(GameScene.MENUSCENE, false);
        this.SetGameState(Tools.GameState.MENU);
    }

    /**
    * <summary>Load the HelpScene</summary>
    */
    private void Help()
    {
        this.LoadALevel(GameScene.HELPSCENE);
    }

    /**
    * <summary>Load the CreditScene</summary>
    */
    private void CreditGame()
    {
        this.LoadALevel(GameScene.CREDITSCENE);
    }

    /**
    * <summary>Exit the game</summary>
    */
    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    #endregion

    #region Setters
    /**
     * <summary>Set the game state</summary> 
     * <remarks>Send an event for each gamestate</remarks>
     * <param name="newGameState">The new game state</param>
     */
    private void SetGameState(GameState newGameState)
    {
        GameManager.m_GameState = newGameState;
        switch (GameManager.m_GameState)
        {
            case GameState.MENU:
                EventManager.Instance.Raise(new GameMenuEvent());
                break;
            case GameState.PLAY:
                EventManager.Instance.Raise(new GamePlayEvent());
                break;
            case GameState.PAUSE:
                EventManager.Instance.Raise(new GamePauseEvent());
                break;
            case GameState.WIN:
                EventManager.Instance.Raise(new GameVictoryEvent());
                break;
            case GameState.GAMEOVER:
                EventManager.Instance.Raise(new GameOverEvent());
                break;
            case GameState.ENDLVL:
                EventManager.Instance.Raise(new GameEndLVLEvent());
                break;
            default:
                break;
        }
    }

    /**
     * <summary>Set the score</summary>
     * <param name="score">The score</param>
     */
    private void SetScore(int score)
    {
        this.m_CurrentScore = score;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = this.m_CurrentScore });
    }

    /**
     * <summary>Set the GameScene</summary>
     * <remarks>Load immediatly the new game scene, pls use <see cref="LoadALevel(GameScene)"/> to wait before load</remarks>
     * <param name="gameScene">The gamescene</param>
     */
    private void SetGameScene(GameScene gameScene)
    {
        this.m_CurrentScene = gameScene;
        switch (gameScene)
        {
            case GameScene.MENUSCENE:
                SceneManager.LoadScene("MenuScene");
                break;
            case GameScene.FIRSTLEVELSCENE:
                SceneManager.LoadScene("FirstLevelScene");
                break;
            case GameScene.HELPSCENE:
                SceneManager.LoadScene("HelpScene");
                break;
            case GameScene.CREDITSCENE:
                SceneManager.LoadScene("CreditScene");
                break;
            case GameScene.VICTORYSCENE:
                SceneManager.LoadScene("VictoryScene");
                break;
            default:
                break;
        }
    }
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.AddListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.AddListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.AddListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.AddListener<LevelGameOverEvent>(OnLevelGameOverEvent);
        EventManager.Instance.AddListener<ObjectWillGainScoreEvent>(OnObjectWillGainScoreEvent);
        EventManager.Instance.AddListener<ContinueGameEvent>(OnContinueGameEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<NewGameButtonClickedEvent>(OnNewGameButtonClickedEvent);
        EventManager.Instance.RemoveListener<HelpButtonClickedEvent>(OnHelpButtonClickedEvent);
        EventManager.Instance.RemoveListener<CreditButtonClickedEvent>(OnCreditButtonClickedEvent);
        EventManager.Instance.RemoveListener<ExitButtonClickedEvent>(OnExitButtonClickedEvent);
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(OnMainMenuButtonClickedEvent);
        EventManager.Instance.RemoveListener<LevelGameOverEvent>(OnLevelGameOverEvent);
        EventManager.Instance.RemoveListener<ObjectWillGainScoreEvent>(OnObjectWillGainScoreEvent);
    }
    #endregion

    #region GameManagers OWN UPDATE METHODS

    /**
     * <summary>Update the Game</summary>
     * <remarks>It run only if the game state is PLAY</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    private void UpdateGame()
    {
        if (GameManager.IsPlaying && Input.GetButton("ResetGame"))
        {
            this.ResetGame();
        }else if (Input.GetButton("PauseGame"))
        {
            if (GameManager.IsPlaying)
            {
                this.PauseGame();
            }else
            {
                this.PlayGame();
            }
        }

        this.UpdateGameState();
    }

    /**
     * <summary>Update the GameState</summary>
     * <remarks>If the timer is finish the game state is set to LOOSE. Also we call everytime UpdateGameTime and CheckGameState</remarks>
     * 
     * <param name="timerUtils">The TimerUtils object</param>
     */
    private void UpdateGameState()
    {
        if (GameManager.IsPlaying)
        {
            //this.GameOver();
        }
    }
    #endregion

    #region MonoBehaviour METHODS
    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }

    private void Awake()
    {
        base.InitManager();
    }

    private void Start()
    {
        Tools.GameState gameState = this.m_IsDebugMode ? Tools.GameState.PLAY : GameManager.m_GameState;
        this.SetGameState(gameState);
    }

    private void FixedUpdate()
    {
        this.UpdateGame();
    }

    private void OnDestroy()
    {
        if (this.m_GameManagerCoroutine != null)
        {
            StopCoroutine(this.m_GameManagerCoroutine);
        }
    }
    #endregion
}
