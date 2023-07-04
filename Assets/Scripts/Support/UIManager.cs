using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

[System.Serializable]
public class GunUpgrade
{
    public GameObject g;
    public Image gunImage;
    public Text dmg;
    public Button critUpgrade;
    public Button dmgUpgrade;
    public Button clomeUpgrade;
}

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
    [SerializeField] private List<GunUpgrade> gunUpgrades = null;
    [SerializeField] private Slider vol = null;
    [SerializeField] private EnableDisableGameObject claimButton = null;






    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private Text mainLevelText = null;
    [SerializeField] private Text inGameLevelText = null;
    [SerializeField] private Text winLevelText = null;
    [SerializeField] private Text loseLevelText = null;
    [SerializeField] private Text debugText = null;
    [SerializeField] private Text dropSpeedLevel = null;
    [SerializeField] private Text dropValueLevel = null;
    [SerializeField] private Text fireFasterLevel = null;




    [Header("Settings")]
    [SerializeField] private GameObject settingsBox;
    [SerializeField] private Sprite enabledVibration;
    [SerializeField] private Sprite disabledVibration;
    [SerializeField] private Sprite disabledSFX;
    [SerializeField] private Sprite enabledSFX;
    [SerializeField] private Button SFX;
    [SerializeField] private Button vibration;
    [SerializeField] private Button nextLevelButton;


    [Header("Reward/Coins")]
    [SerializeField] List<Text> allCurrentCoins = null;
    [SerializeField] List<Transform> coins = null;
    private int currentCoin = 0;

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
        UpdateSoundSlider();
        UpdateSoundVolume();
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

   
    public void ActiveNextLevel(bool active)
    {
        nextLevelButton.interactable = active;
    }

    public void UpdateLevelReward(int v)
    {
        levelReward.text = "+" + v + "";
    }
    public void ResetClaimButton()
    {
        GetComponent<EnableDisableGameObject>().ResetAndDisableGameObject();
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
    public void OnClickClaimExtra()
    {
        //show ad
        //Pause
        //on ad complete give coins
        GunSelectionGridManager.Instance.WatchedRewardedAd(8);
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
    public void OnClick_BackToHomeButton()
    {
        GameManager.Instance.GoHome();
    }
    public void OnClick_ClaimSpeed()
    {
        GunSelectionGridManager.Instance.WatchedRewardedAd(9);
    }


    public void SendPoolTo(bool add, Vector3 worldPos)
    {
        // Create a new DOTween sequence
        DOTween.KillAll();
        Sequence s = DOTween.Sequence();

        foreach (DeathLine dl in FindObjectsOfType<DeathLine>())
        {
            dl.Rotate();
        }
        //s.Kill();
        foreach(Transform c in coins)
        {
            c.transform.localPosition = Vector3.zero;
        }
        Vector3 screenEndPoint = Vector3.zero;
        // Move each coin
        for (int i = 0; i < 7; i++)
        {
            s.AppendCallback(() =>
            {
                // Get the next coin GameObject from the list and activate it
                GameObject coin = coins[currentCoin].gameObject;
                coin.SetActive(true);

                if (add)
                {
                    // Convert the end point's world position to a screen position
                     screenEndPoint = Camera.main.WorldToScreenPoint(worldPos);
                }
                else
                {
                     screenEndPoint = worldPos;
                }

                // Convert the screen position to a position relative to the canvas
                Vector2 canvasEndPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)coin.transform.parent, screenEndPoint, null, out canvasEndPoint);

                // Move the coin to the end point
                coin.transform.DOLocalMove(canvasEndPoint, 0.5f).OnComplete(() =>
                {
                    // Deactivate the coin when it reaches the end point
                    coin.SetActive(false);
                });

                // Update the index of the current coin
                currentCoin = (currentCoin + 1) % coins.Count;
            });

            // Add a delay before moving the next coin
            s.AppendInterval(0.1f);
        }

        // Start the sequence
        s.Play();
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

        GunSelectionGridManager.Instance.PopupClosed();
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

    public void ActiveNewGunCritUpgrade(bool active)
    {

    }
    public void ActiveNewGunDamageUpgrade(bool active)
    {
        GunSelectionGridManager.Instance.WatchedRewardedAd(3);
    }
    public void ActiveNewGunCloneUpgrade(bool active)
    {

    }
    public void UpdateSoundSlider()
    {
        vol.value = PlayerPrefs.GetFloat("soundvalue", 1f);
        UpdateSoundVolume();
    }
    public void UpdateSoundVolume()
    {
        SoundManager.Instance.UpdateSound(vol.value);
        PlayerPrefs.SetFloat("soundvalue", vol.value);

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
