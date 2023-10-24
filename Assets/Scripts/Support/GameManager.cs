using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Momo;
//using CrazyGames;
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    _instance = singleton.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Properties
    [Header("Component Reference")]
    [SerializeField] private GameObject confetti;
    [SerializeField] private List<MonoBehaviour> objectsToDisable;

    [Header("Game Attributes")]
    [SerializeField] private int currentScore;
    [SerializeField] private int currentLevel;
    [SerializeField] private GameState currentState;
    [SerializeField] private int numberOfMoves;
    [SerializeField] private float levelLength;
    [SerializeField] public int nextLevelRequirement;


    public int CurrentScore => currentScore;
    public int CurrentLevel => currentLevel;
    public GameState CurrentState => currentState;

    private float levelStartTime;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;
        Application.targetFrameRate = 100;
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 1);
        UIManager.Instance.UpdateLevel(currentLevel);
        currentState = GameState.Main;
      //  CrazyEvents.Instance.GameplayStart();
        StartLevel();
    }
    #endregion

    #region Level Management
    public void StartLevel()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.Gameplay);
        currentState = GameState.InGame;
        Analytics.Instance.StartLevel(currentLevel);
        levelStartTime = Time.time;

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
       // CrazyEvents.Instance.GameplayStop();
    }

    public void UnPause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;

        }

        //CrazyEvents.Instance.GameplayStart();
    }

    public void AddMove(int v)
    {
        numberOfMoves += v;
    }

    public void WinLevel()
    {
        if (currentState == GameState.InGame)
        {
            SaveManager.Instance.DeleteSaveFile();

            //confetti.SetActive(true);
            Invoke("ChangeLevel", 1.4f);
            CoinManager.Instance.SubtractCoins(nextLevelRequirement);
            currentState = GameState.Win;
            PlayerPrefs.SetInt("level", currentLevel + 1);
            currentLevel++;

            foreach (MonoBehaviour m in objectsToDisable)
            {
                m.enabled = false;
            }
            levelLength = Time.time - levelStartTime;
            PlayerLevelData pld = new PlayerLevelData();
            pld.Init(currentLevel, 0, true, numberOfMoves, levelLength);

            if (currentLevel == 5)
            {
                if (PlayerPrefs.GetInt("review", 0) == 0)
                {
                    GetComponent<ReviewsManager>().Request();
                    PlayerPrefs.SetInt("review", 1);
                }
            }
            PlayerManager.Instance.AddLevelData(pld);
            //Send Data
            Analytics.Instance.WinLevel();


        }
    }

    public void GoHome()
    {
        UnPause();
        SceneManager.LoadScene("Core");
    }

    public void LoseLevel()
    {
        if (currentState == GameState.InGame)
        {
            Invoke("ShowLoseUI", 2f);
            currentState = GameState.Lose;
            foreach (MonoBehaviour m in objectsToDisable)
            {
                m.enabled = false;
            }
            levelLength = Time.time - levelStartTime;
            PlayerLevelData pld = new PlayerLevelData();
            pld.Init(currentLevel, 1, false, numberOfMoves, levelLength);
            PlayerManager.Instance.AddLevelData(pld);
            //Send Data
            Analytics.Instance.LoseLevel();
        }
    }

    public void ChangeLevel()
    {
        //SceneManager.LoadScene("Core");
        SaveManager.Instance.DeleteSaveFile();

        if (currentLevel > 2)
        {
            int newId = currentLevel % 2;
            if (newId == 0)
            {
                newId = 2;
            }
            SceneManager.LoadScene("Level " + (newId));
        }
        else
        {
            SceneManager.LoadScene("Level " + currentLevel);
        }
    }
    #endregion

    #region Public Core Functions
    public void AddScore(int value)
    {
        currentScore += value;
        UIManager.Instance.UpdateScore(currentScore);
    }

    public string FormatNumber(int num)
    {
        if (num >= 1000000000)
            return (num / 1000000000d).ToString("0.#") + "B";
        else if (num >= 1000000)
            return (num / 1000000d).ToString("0.#") + "M";
        else if (num >= 1000)
            return (num / 1000d).ToString("0.#") + "K";
        else
            return num.ToString();
    }

    #endregion

    #region Invoke Functions
    private void ShowWinUI()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.GameWin);
    }

    private void ShowLoseUI()
    {
        UIManager.Instance.SwitchUIPanel(UIPanelState.GameLose);
    }
    #endregion
}


