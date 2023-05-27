using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AppodealAds.Unity.Api;
//using AppodealAds.Unity.Common;
using CrazyGames;

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
        
    }


    public void ShowRewarded()
    {
        CrazyAds.Instance.beginAdBreakRewarded();
    }

    public void ShowAdChangeLevel()
    {
        CrazyAds.Instance.beginAdBreak(AdBreakChangeLevelSuccess);
    }
    void AdBreakChangeLevelSuccess()
    {
        GetComponent<SceneHandler>().ChangeTheLevel();
    }
  
}