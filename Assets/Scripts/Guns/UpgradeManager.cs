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

    private int currentDropValueLevel = 0;
    private int currentDropSpeedLevel = 0;
    private int currentFireRateLevel = 0;



    public BallSpawner bs;

    public void UpgradeBallDropValue()
    {
        if (currentDropValueLevel >= dropValueCosts.Count)
        {
            return;
        }
        if (dropValueCosts[currentDropValueLevel] == 0)
        {
            //Watch Ad
            GunSelectionGridManager.Instance.WatchedRewardedAd(6);
            return;
        }
        else
        {
            valueAdIcon.SetActive(false);
        }

        if (currentDropValueLevel < dropValueCosts.Count && CoinManager.Instance.SubtractCoins(dropValueCosts[currentDropValueLevel]))
        {
            currentDropValueLevel++;
            upgradeDropValueCostText.text = currentDropValueLevel < dropValueCosts.Count ? "DROP VALUE \n" + "$" + dropValueCosts[currentDropValueLevel].ToString() : "MAX";
            dropValueLevel.text = "LVL " + (currentDropValueLevel + 1) + "";
        }

        if(currentDropValueLevel>= dropValueCosts.Count)
        {
            return;
        }
        if (dropValueCosts[currentDropValueLevel] == 0)
        {
            upgradeDropValueCostText.text = "DROP VALUE \n" + "FREE";
            valueAdIcon.SetActive(true);
        }

    }
    public void UpgradeBallDropValueFree()
    {
        if (currentDropValueLevel >= dropValueCosts.Count)
        {
            return;
        }
        if (currentDropValueLevel < dropValueCosts.Count)
        {
            currentDropValueLevel++;
            upgradeDropValueCostText.text = currentDropValueLevel < dropValueCosts.Count ? "DROP VALUE \n" + "$" + dropValueCosts[currentDropValueLevel].ToString() : "MAX";
            dropValueLevel.text = "LVL " + (currentDropValueLevel + 1) + "";

        }
        if (dropValueCosts[currentDropValueLevel] == 0)
        {
            upgradeDropValueCostText.text = "DROP VALUE \n" + "FREE";
            valueAdIcon.SetActive(true);
        }
        else
        {
            valueAdIcon.SetActive(false);
        }       
    }

    public void UpgradeBallDropSpeed()
    {
        if(currentDropSpeedLevel>= dropSpeedCosts.Count)
        {
            return;
        }
        if (dropSpeedCosts[currentDropSpeedLevel] == 0)
        {
            //Watch Ad
            GunSelectionGridManager.Instance.WatchedRewardedAd(5);
            return;
        }
        else
        {
            speedAdIcon.SetActive(false);
        }
        if (currentDropSpeedLevel < dropSpeedCosts.Count && CoinManager.Instance.SubtractCoins(dropSpeedCosts[currentDropSpeedLevel]))
        {
            currentDropSpeedLevel++;
            upgradeDropSpeedCostText.text = currentDropSpeedLevel < dropSpeedCosts.Count ? "DROP SPEED \n" + "$" +dropSpeedCosts[currentDropSpeedLevel].ToString() : "MAX";
            bs.spawnRate = GetDropSpeed();
            dropSpeedLevel.text = "LVL " + (currentDropSpeedLevel + 1) + "";
        }

        if (currentDropSpeedLevel >= dropSpeedCosts.Count)
        {
            return;
        }
        if (dropSpeedCosts[currentDropSpeedLevel] == 0)
        {

            upgradeDropSpeedCostText.text = "DROP SPEED \n" + "FREE";
            speedAdIcon.SetActive(true);

        }

    }
    public void UpgradeBallDropSpeedFree()
    {
        if (currentDropSpeedLevel < dropSpeedCosts.Count)
        {
            currentDropSpeedLevel++;
            upgradeDropSpeedCostText.text = currentDropSpeedLevel < dropSpeedCosts.Count ? "DROP SPEED \n" + "$" + dropSpeedCosts[currentDropSpeedLevel].ToString() : "MAX";
            bs.spawnRate = GetDropSpeed();
            dropSpeedLevel.text = "LVL " + (currentDropSpeedLevel + 1) + "";
        }
        else
        {
            return;
        }
        if (dropSpeedCosts[currentDropSpeedLevel] == 0)
        {

            upgradeDropSpeedCostText.text = "DROP SPEED \n" + "FREE";
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
            upgradeFireRateCostText.text = "FIRE RATE \n"  + "FREE";
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
    }

    public void UpgradeGunFireRate()
    {
        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            return;
        }
        if (fireRateCosts[currentFireRateLevel] == 0)
        {
            //Watch Ad
            GunSelectionGridManager.Instance.WatchedRewardedAd(7);
            return;
        }
        else
        {
            fireRateAdIcon.SetActive(false);
        }
        if (currentFireRateLevel < fireRateCosts.Count && CoinManager.Instance.SubtractCoins(fireRateCosts[currentFireRateLevel]))
        {
            currentFireRateLevel++;
            upgradeFireRateCostText.text =   currentFireRateLevel < fireRateCosts.Count ? "FIRE RATE \n"+ "$" + fireRateCosts[currentFireRateLevel].ToString() : "MAX";
            fireFasterLevel.text = "LVL " + (currentFireRateLevel + 1) + "";
        }
        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            return;
        }
        if (fireRateCosts[currentFireRateLevel] == 0)
        {
            upgradeFireRateCostText.text = "FIRE RATE \n" + "FREE";
            fireRateAdIcon.SetActive(true);
        }
    }
    public void UpgradeGunFireRateFree()
    {
        if (currentFireRateLevel >= fireRateCosts.Count)
        {
            return;
        }
        if (currentFireRateLevel < fireRateCosts.Count)
        {
            currentFireRateLevel++;
            upgradeFireRateCostText.text = currentFireRateLevel < fireRateCosts.Count ? "FIRE RATE \n" + "$" + fireRateCosts[currentFireRateLevel].ToString() : "MAX";
            fireFasterLevel.text = "LVL " + (currentFireRateLevel + 1) + "";

        }
        if (fireRateCosts[currentFireRateLevel] == 0)
        {
            upgradeFireRateCostText.text = "FIRE RATE \n" + "FREE";
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
