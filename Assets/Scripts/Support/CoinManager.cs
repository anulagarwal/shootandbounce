using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{

    #region Singleton
    private static CoinManager _instance;
    public static CoinManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CoinManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("Managers");
                    _instance = singleton.AddComponent<CoinManager>();
                }
            }
            return _instance;
        }
    }
    #endregion
    [Header("Attributes")]
    [SerializeField] int startCoins;
    [SerializeField] public int currentCoins;

    [Header("Rewards")]
    [SerializeField] int levelReward;
    // Start is called before the first frame update
    void Start()
    {
        startCoins = PlayerPrefs.GetInt("coins", startCoins);
        currentCoins = startCoins;
        UIManager.Instance.UpdateCurrentCoins(currentCoins);
        AddCoins(0, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Level Rewards

    public void AddToLevelReward(int v)
    {
        levelReward += v;
        UIManager.Instance.UpdateLevelReward(levelReward);
    }

    public void MultiplyLevelReward(int v)
    {
        levelReward *= v;
        UIManager.Instance.UpdateLevelReward(levelReward);
    }

    #endregion

    #region Coin Getter Setter
    public void AddCoins(int v, Vector3 worldPos)
    {
        currentCoins += v;
        PlayerPrefs.SetInt("coins", currentCoins);
        UIManager.Instance.UpdateCurrentCoins(currentCoins);
        if (currentCoins >= GameManager.Instance.nextLevelRequirement)
        {
            UIManager.Instance.ActiveNextLevel(true);
        }
        //  UIManager.Instance.SendPoolTo(true, worldPos);
    }


    public bool SubtractCoins(int v, Vector3 worldPos)
    {
        if (currentCoins - v > 0)
        {
            currentCoins -= v;
            PlayerPrefs.SetInt("coins", currentCoins);
            UIManager.Instance.UpdateCurrentCoins(currentCoins);
            UIManager.Instance.SendPoolTo(false, worldPos);
            if(currentCoins>= GameManager.Instance.nextLevelRequirement)
            {
                UIManager.Instance.ActiveNextLevel(true);
            }
            else
            {
                UIManager.Instance.ActiveNextLevel(false);
            }
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool SubtractCoins(int v)
    {
        if (currentCoins - v > 0)
        {
            currentCoins -= v;
            PlayerPrefs.SetInt("coins", currentCoins);
            UIManager.Instance.UpdateCurrentCoins(currentCoins);
            if (currentCoins >= GameManager.Instance.nextLevelRequirement)
            {
                UIManager.Instance.ActiveNextLevel(true);
            }
            else
            {
                UIManager.Instance.ActiveNextLevel(false);
            }
            return true;
        }
        else
        {
            return false;
        }

    }

    #endregion

}
