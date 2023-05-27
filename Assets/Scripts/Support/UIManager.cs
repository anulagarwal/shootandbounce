using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("UIManager");
                    _instance = singleton.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region Properties
    [Header("Components Reference")]
    [SerializeField] private GameObject PointText;
    [SerializeField] private GameObject AwesomeText;
    [SerializeField] private GameObject JoyStick;

    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuUIPanel = null;
    [SerializeField] private GameObject gameplayUIPanel = null;
    [SerializeField] private GameObject gameOverWinUIPanel = null;
    [SerializeField] private GameObject gameOverLoseUIPanel = null;
    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject newGunPanel = null;
    [SerializeField] private List<GameObject> newGuns = null;



    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Text mainLevelText = null;
    [SerializeField] private Text inGameLevelText = null;
    [SerializeField] private Text winLevelText = null;
    [SerializeField] private Text loseLevelText = null;
    [SerializeField] private Text debugText = null;

    [Header("Settings")]
    [SerializeField] private GameObject settingsBox;
    [SerializeField] private Sprite enabledVibration;
    [SerializeField] private Sprite disabledVibration;
    [SerializeField] private Sprite disabledSFX;
    [SerializeField] private Sprite enabledSFX;
    [SerializeField] private Button SFX;
    [SerializeField] private Button vibration;

    [Header("Reward/Coins")]
    [SerializeField] List<Text> allCurrentCoins = null;
    [SerializeField] List<Transform> coins = null;

    [Header("Post Level")]
    [SerializeField] Button multiplyReward;
    [SerializeField] Text multiplyText;
    [SerializeField] Text levelReward;

    [Header("Daily")]
    [SerializeField] Button dailyReward;
    [SerializeField] Text dailyText;

    Transform coinBarPos;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("vibrate", 1) == 0)
        {
            vibration.image.sprite = disabledVibration;
        }
        else
        {
            vibration.image.sprite = enabledVibration;
        }

        if (PlayerPrefs.GetInt("sound", 1) == 0)
        {
            SFX.image.sprite = disabledSFX;
        }
        else
        {
            SFX.image.sprite = enabledSFX;
        }
    }
    #endregion

    #region UI Panel Management
    public void SwitchUIPanel(UIPanelState state)
    {
        switch (state)
        {
            case UIPanelState.MainMenu:
                mainMenuUIPanel.SetActive(true);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.Gameplay:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(true);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.GameWin:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(true);
                gameOverLoseUIPanel.SetActive(false);
                break;
            case UIPanelState.GameLose:
                mainMenuUIPanel.SetActive(false);
                gameplayUIPanel.SetActive(false);
                gameOverWinUIPanel.SetActive(false);
                gameOverLoseUIPanel.SetActive(true);
                break;
        }
    }
    #endregion
    #region Update UI Elements
    public void UpdateScore(int value)
    {
        scoreText.text = "" + value;
    }

    public void UpdateDebugText(string s)
    {
        debugText.text = s;
    }

    public void UpdateLevel(int level)
    {
        mainLevelText.text = "LEVEL " + level;
//        inGameLevelText.text = "LEVEL " + level;
        winLevelText.text = "LEVEL " + level;
        loseLevelText.text = "LEVEL " + level;
    }

    public void UpdateCurrentCoins(int v)
    {
        foreach (Text t in allCurrentCoins)
        {
            t.text = GameManager.Instance.FormatNumber(v) + "";
        }
    }

    public void UpdateLevelReward(int v)
    {
        levelReward.text = "+" + v + "";
    }
    #endregion

    #region OnClick UI Buttons
    public void OnClickPlayButton()
    {
        GameManager.Instance.StartLevel();
    }

    public void OnClickChangeButton()
    {
        GameManager.Instance.ChangeLevel();
    }

    public void OnClickMove()
    {
        GameManager.Instance.AddMove(1);
    }

    public void OnClickWin()
    {
        GameManager.Instance.WinLevel();
    }

    public void OnClickSFXButton()
    {
        if (PlayerPrefs.GetInt("sound", 1) == 1)
        {
            PlayerPrefs.SetInt("sound", 0);
            SFX.image.sprite = disabledSFX;
        }
        else
        {
            PlayerPrefs.SetInt("sound", 1);
            SFX.image.sprite = enabledSFX;
        }
    }

    public async void SendPoolTo(bool add, Vector3 worldPos)
    {
        // Existing implementation
    }

    public void OnClickVibrateButton()
    {
        // Existing implementation
    }

    public void OnClickNewGunClose()
    {
        GameManager.Instance.UnPause();
        newGunPanel.SetActive(false);
             foreach (GameObject g in newGuns)
        {
            g.SetActive(false);
        }
    }


    public void OnClickPauseMenu()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public void OnClickUnPause()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.UnPause();
       

    }

    public void OnClickSettingsButton()
    {
        settingsBox.SetActive(!settingsBox.activeSelf);
    }
    #endregion

    public void EnableNewGun(int level)
    {
        GameManager.Instance.PauseGame();
        newGunPanel.SetActive(true);
        foreach (GameObject g in newGuns)
        {
            g.SetActive(false);
        }
        newGuns[level - 1].SetActive(true);
    }

    #region Spawn Texts
    public void SpawnPointText(Vector3 point)
    {
        Instantiate(PointText, point, Quaternion.identity);
    }

    public void SpawnAwesomeText(Vector3 point, string s)
    {
        GameObject g = Instantiate(AwesomeText, new Vector3(point.x, 2, point.z), Quaternion.identity);
        g.GetComponentInChildren<TextMeshPro>().text = s;
    }
    #endregion

}
