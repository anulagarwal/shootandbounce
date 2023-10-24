using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    #region Singleton
    public static UpgradeManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of UpgradeManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    public List<int> dropValueCosts;
    public List<int> dropSpeedCosts;
    public List<int> fireRateCosts;

    public List<int> dropValues;
    public List<float> dropSpeeds;
    public List<float> fireRates;

    public Button dropValueUpgradeButton;
    public Button dropSpeedUpgradeButton;
    public Button fireRateUpgradeButton;

    public GameObject valueAdIcon;
    public GameObject speedAdIcon;
    public GameObject fireRateAdIcon;

    public TMP_Text upgradeDropValueCostText;
    public TMP_Text upgradeDropSpeedCostText;
    public TMP_Text upgradeFireRateCostText;

    [SerializeField] private TMP_Text dropSpeedLevel = null;
    [SerializeField] private TMP_Text dropValueLevel = null;
    [SerializeField] private TMP_Text fireFasterLevel = null;

    public int currentDropValueLevel = 0;
    public int currentDropSpeedLevel = 0;
    public int currentFireRateLevel = 0;



    public BallSpawner bs;

    public void RestoreUpgrades(List<UpgradeData> ud)
    {
        foreach(UpgradeData u in ud)
        {
            if (u.upgradeName == "FireRate")
            {
                if (u.level > 0)
                {
                    for (int i = 0; i < u.level; i++)
                    {
                        UpgradeGunFireRateFree();
                    }
                }
            }

            if(u.upgradeName == "DropSpeed")
            {
                if (u.level > 0)
                {
                    for (int i = 0; i < u.level; i++)
                    {
                        UpgradeBallDropSpeedFree();
                    }
                }
            }

            if(u.upgradeName == "DropValue")
            {
                if (u.level > 0)
                {
                    for (int i = 0; i < u.level; i++)
                    {
                        UpgradeBallDropValueFree();
                    }
                }
            }
            if(u.upgradeName == "BuyGun")
            {
                if (u.level > 0)
                {
                    GunSelectionGridManager.Instance.currentGunIndex = u.level;
                }
            }    
        }
    }

    public void UpgradeBallDropValueAd()
    {
        GunSelectionGridManager.Instance.WatchedRewardedAd(6);
    }

    public void BallValueFunction()
    {
        currentDropValueLevel++;
        upgradeDropValueCostText.text = currentDropValueLevel < dropValueCosts.Count ? "DROP VALUE \n" + "$" + dropValueCosts[currentDropValueLevel].ToString() : "MAX";
        dropValueLevel.text = "LVL " + (currentDropValueLevel + 1) + "";
        SaveManager.Instance.DoSaveGame();
    }
    public void UpgradeBallDropValue()
    {
       if (currentDropValueLevel >= dropValueCosts.Count)
        {
            return;
        }
        if (currentDropValueLevel%3 != 0)
        {
            valueAdIcon.SetActive(false);          
        }
        
        if (currentDropValueLevel < dropValueCosts.Count && CoinManager.Instance.SubtractCoins(dropValueCosts[currentDropValueLevel], dropValueUpgradeButton.transform.position, false))
        {
            BallValueFunction();
        }

        if (currentDropValueLevel >= dropValueCosts.Count)
        {
            valueAdIcon.SetActive(false);

            return;
        }
        if (currentDropValueLevel%3 == 0)
        {
            valueAdIcon.SetActive(true);
        }
        else
        {
            valueAdIcon.SetActive(false);
        }
       

    }
    public void UpgradeBallDropValueFree()
    {
        if (currentDropValueLevel >= dropValueCosts.Count)
        {
            valueAdIcon.SetActive(false);
            return;
        }
        if (currentDropValueLevel < dropValueCosts.Count)
        {
            BallValueFunction();
        }
        if (currentDropValueLevel >= dropValueCosts.Count)
        {
            valueAdIcon.SetActive(false);
            return;
        }
        if (currentDropValueLevel%3 == 0)
        {
            valueAdIcon.SetActive(true);
        }
        else
        {
            valueAdIcon.SetActive(false);
        }


    }

    public void UpgradeBallDropSpeedAd()
    {
        GunSelectionGridManager.Instance.WatchedRewardedAd(5);
    }
    public void BallSpeedFunction()
    {
        currentDropSpeedLevel++;
        upgradeDropSpeedCostText.text = currentDropSpeedLevel < dropSpeedCosts.Count ? "DROP SPEED \n" + "$" + dropSpeedCosts[currentDropSpeedLevel].ToString() : "MAX";
        bs.spawnRate = GetDropSpeed();
        dropSpeedLevel.text = "LVL " + (currentDropSpeedLevel + 1) + "";
        SaveManager.Instance.DoSaveGame();

    }

    public void UpgradeBallDropSpeed()
    {
        if (currentDropSpeedLevel >= dropSpeedCosts.Count)
        {
            speedAdIcon.SetActive(false);
            return;
        }
        if (dropSpeedCosts[currentDropSpeedLevel] != 0)
        {
            speedAdIcon.SetActive(false);
            
        }

        if (currentDropSpeedLevel < dropSpeedCosts.Count && CoinManager.Instance.SubtractCoins(dropSpeedCosts[currentDropSpeedLevel], dropSpeedUpgradeButton.transform.position, false))
        {
            BallSpeedFunction();
        }
        if (currentDropSpeedLevel >= dropSpeedCosts.Count)
        {
            speedAdIcon.SetActive(false);
            return;
        }
        if (currentDropSpeedLevel%3 == 0)
        {
            speedAdIcon.SetActive(true);
        }
        else
        {
            speedAdIcon.SetActive(false);
        }



    }
    public void UpgradeBallDropSpeedFree()
    {
        if (currentDropSpeedLevel < dropSpeedCosts.Count)
        {
            BallSpeedFunction();

        }
        else
        {
            speedAdIcon.SetActive(false);
            return;
        }
        if (currentDropSpeedLevel%3 == 0)
        {
            speedAdIcon.SetActive(true);
        }
        else
        {
            speedAdIcon.SetActive(false);
        }

    }


    void Start()
    {
        upgradeDropValueCostText.text = "DROP VALUE \n" + "$" + dropValueCosts[currentDropValueLevel].ToString();
        upgradeDropSpeedCostText.text = "DROP SPEED \n" + "$" + dropSpeedCosts[currentDropSpeedLevel].ToString();
        upgradeFireRateCostText.text = "FIRE RATE \n" + "$" + fireRateCosts[currentFireRateLevel].ToString();

        if (fireRateCosts[currentFireRateLevel] == 0)
        {
            upgradeFireRateCostText.text = "FIRE RATE \n" + "FREE";
            fireRateAdIcon.SetActive(true);
        }
        if (dropSpeedCosts[currentDropSpeedLevel] == 0)
        {
            upgradeDropSpeedCostText.text = "DROP SPEED \n" + "FREE";
            speedAdIcon.SetActive(true);
        }
        if (dropValueCosts[currentDropValueLevel] == 0)
        {
            upgradeDropValueCostText.text = "DROP VALUE \n" + "FREE";
            valueAdIcon.SetActive(true);
        }
        bs.spawnRate = GetDropSpeed();

        fireRateAdIcon.SetActive(false);
        speedAdIcon.SetActive(false);
        valueAdIcon.SetActive(false);

    }

    public void UpgradeGunFireRateAd()
    {
        GunSelectionGridManager.Instance.WatchedRewardedAd(7);
    }

    public void FireRateFunction()
    {
        currentFireRateLevel++;
        upgradeFireRateCostText.text = currentFireRateLevel < fireRateCosts.Count ? "FIRE RATE \n" + "$" + fireRateCosts[currentFireRateLevel].ToString() : "MAX";
        fireFasterLevel.text = "LVL " + (currentFireRateLevel + 1) + "";
        SaveManager.Instance.DoSaveGame();

    }
    public void UpgradeGunFireRate()
    {
        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            fireRateAdIcon.SetActive(false);

            return;
        }
        if (currentFireRateLevel%3 != 0)
        {
            fireRateAdIcon.SetActive(false);           
        }

        if (currentFireRateLevel < fireRateCosts.Count && CoinManager.Instance.SubtractCoins(fireRateCosts[currentFireRateLevel], fireRateUpgradeButton.transform.position, false))
        {
            FireRateFunction();
        }

        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            fireRateAdIcon.SetActive(false);

            return;
        }

        if (currentFireRateLevel%3 == 0)
        {
            fireRateAdIcon.SetActive(true);
        }
        else
        {
            fireRateAdIcon.SetActive(false);
        }



    }
    public void UpgradeGunFireRateFree()
    {
        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            fireRateAdIcon.SetActive(false);

            return;
        }
        if (currentFireRateLevel < fireRateCosts.Count)
        {
            FireRateFunction();

        }
        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            fireRateAdIcon.SetActive(false);

            return;
        }
        if (currentFireRateLevel%3 == 0)
        {
            fireRateAdIcon.SetActive(true);
        }
        else
        {
            fireRateAdIcon.SetActive(false);
        }
       


    }

    void Update()
    {
        if (currentDropValueLevel < dropValueCosts.Count)
            dropValueUpgradeButton.interactable = CoinManager.Instance.currentCoins >= dropValueCosts[currentDropValueLevel];
        if (currentDropSpeedLevel < dropSpeedCosts.Count)
            dropSpeedUpgradeButton.interactable = CoinManager.Instance.currentCoins >= dropSpeedCosts[currentDropSpeedLevel];

        if (currentFireRateLevel < fireRateCosts.Count)
            fireRateUpgradeButton.interactable = CoinManager.Instance.currentCoins >= fireRateCosts[currentFireRateLevel];
    }

    public int GetDropValue()
    {
        return currentDropValueLevel < dropValues.Count ? dropValues[currentDropValueLevel] : dropValues[dropValues.Count - 1];
    }

    public float GetDropSpeed()
    {
        return currentDropSpeedLevel < dropSpeeds.Count ? dropSpeeds[currentDropSpeedLevel] : dropSpeeds[dropSpeeds.Count - 1];
    }

    public float GetFireRate()
    {
        return currentFireRateLevel < fireRates.Count ? fireRates[currentFireRateLevel] : fireRates[fireRates.Count - 1];
    }
}
