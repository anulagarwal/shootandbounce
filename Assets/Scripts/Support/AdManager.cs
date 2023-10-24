using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CrazyGames;
//using AppodealAds.Unity.Api;
//using AppodealAds.Unity.Common;

public class AdManager : MonoBehaviour
{

    #region Singleton
    private static AdManager _instance;
    public static AdManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("AdManager");
                    _instance = singleton.AddComponent<AdManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    [Header("Attributes")]
    [SerializeField] bool isBannerOn;
    [SerializeField] bool isInterstitialOn;
    [SerializeField] bool isRewardedOn;
    [SerializeField] AdType adNetwork;
    [SerializeField] string key;
    [SerializeField] RewardType rewardState;

    bool isRewardedGD;

    private void Start()
    {
        //Appodeal.initialize("8c2fb21772d1170cfe9feaef793420cb9c446d1806692480", Appodeal.REWARDED_VIDEO, this);
        //Appodeal.cache(Appodeal.REWARDED_VIDEO);
        /* Appodeal.setRewardedVideoCallbacks(this);
         Appodeal.initialize("8c2fb21772d1170cfe9feaef793420cb9c446d1806692480", Appodeal.REWARDED_VIDEO, this);
         Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);
         Appodeal.cache(Appodeal.REWARDED_VIDEO);*/
        GameDistribution.Instance.PreloadRewardedAd();

        GameDistribution.OnResumeGame += OnResumeGame;
       // GameDistribution.OnPauseGame += OnPauseGame;
       // GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
        GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
        GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
        GameDistribution.OnRewardGame += OnRewardGame;
    }


    public void ShowRewarded()
    {
        //if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        {
          //  Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
    }

   
    void AdBreakChangeLevelSuccess()
    {
        GetComponent<SceneHandler>().ChangeTheLevel();
    }

    public void WatchRewardedAdForDamageUpgrade()
    {
        GameManager.Instance.PauseGame();
        //CrazyAds.Instance.beginAdBreakRewarded(DamageUpgradeBack);

        GameDistribution.Instance.ShowRewardedAd();

        /*if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        {
            /*Appodeal.show(Appodeal.REWARDED_VIDEO);
            Appodeal.cache(Appodeal.REWARDED_VIDEO);
        }
        else
        {
        }*/
       // DamageUpgradeBack();

        //remove
    }

    public void WatchAdForLevel()
    {
        GameDistribution.Instance.ShowAd();
    }

    void DamageUpgradeBack()
    {
        GameManager.Instance.UnPause();
        GunSelectionGridManager.Instance.CallBackRewardedAd();
    }

    public void OnResumeGame()
    {
        AdBreakChangeLevelSuccess();
    }

    void ChangeLevel()
    {
        GameManager.Instance.WinLevel();
    }

    #region Rewarded Video callback handlers

    //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache).
    public void onRewardedVideoLoaded(bool isPrecache)
    {
        Debug.Log("Video loaded");
    }

    // Called when rewarded video failed to load
    public void onRewardedVideoFailedToLoad()
    {
        Debug.Log("Video failed");
        GameManager.Instance.UnPause();

    }

    // Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, or incorrect creative)
    public void OnRewardedVideoFailure()
    {
        Debug.Log("Video show failed");
        GameManager.Instance.UnPause();

    }

    // Called when rewarded video is shown
    public void onRewardedVideoShown()
    {
        Debug.Log("Video shown");
    }

    // Called when reward video is clicked
    public void onRewardedVideoClicked()
    {
        Debug.Log("Video clicked");
    }

    // Called when rewarded video is closed
    public void onRewardedVideoClosed(bool finished)
    {
        Debug.Log("Video closed");
    

       // Appodeal.cache(Appodeal.REWARDED_VIDEO);
    }

    // Called when rewarded video is viewed until the end
    public void OnRewardedVideoSuccess()
    {

       
      //  Appodeal.cache(Appodeal.REWARDED_VIDEO);
        //  Appodeal.cache(Appodeal.REWARDED_VIDEO);

    }

    public void OnRewardGame()
    {
        GunSelectionGridManager.Instance.CallBackRewardedAd();
        GameManager.Instance.UnPause();

        GameDistribution.Instance.PreloadRewardedAd();


    }
    //Called when rewarded video is expired and can not be shown
    public void onRewardedVideoExpired()
    {
        Debug.Log("Video expired");
    }

    #endregion
}
