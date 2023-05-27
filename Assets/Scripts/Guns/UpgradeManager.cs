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

    public TMP_Text upgradeDropValueCostText;
    public TMP_Text upgradeDropSpeedCostText;
    public TMP_Text upgradeFireRateCostText;

    private int currentDropValueLevel = 0;
    private int currentDropSpeedLevel = 0;
    private int currentFireRateLevel = 0;

    public List<float> gunDamages;
    public List<float> gunFireRates;
    public List<float> ranges;


    public BallSpawner bs;

    public void UpgradeBallDropValue()
    {
        if (currentDropValueLevel < dropValueCosts.Count && CoinManager.Instance.SubtractCoins(dropValueCosts[currentDropValueLevel]))
        {
            currentDropValueLevel++;
            upgradeDropValueCostText.text = currentDropValueLevel < dropValueCosts.Count ? "DROP VALUE \n" + "$" + dropValueCosts[currentDropValueLevel].ToString() : "MAX";
            AdManager.Instance.ShowRewarded();
        }
    }

    public void UpgradeBallDropSpeed()
    {
        if (currentDropSpeedLevel < dropSpeedCosts.Count && CoinManager.Instance.SubtractCoins(dropSpeedCosts[currentDropSpeedLevel]))
        {
            currentDropSpeedLevel++;
            upgradeDropSpeedCostText.text = currentDropSpeedLevel < dropSpeedCosts.Count ? "DROP SPEED \n" + "$" +dropSpeedCosts[currentDropSpeedLevel].ToString() : "MAX";
            bs.spawnRate = GetDropSpeed();

        }
    }

    public float GetGunDamage(int level)
    {
        return level < gunDamages.Count ? gunDamages[level] : gunDamages[gunDamages.Count - 1];
    }

    public float GetGunFireRate(int level)
    {
        return level < gunFireRates.Count ? gunFireRates[level] : gunFireRates[gunFireRates.Count - 1];
    }

    public float GetGunRange(int level)
    {
        return level < ranges.Count ? ranges[level] : ranges[ranges.Count - 1];
    }

    void Start()
    {
        upgradeDropValueCostText.text = "DROP VALUE \n" + "$" + dropValueCosts[currentDropValueLevel].ToString();
        upgradeDropSpeedCostText.text = "DROP SPEED \n" + "$" + dropSpeedCosts[currentDropSpeedLevel].ToString();
        upgradeFireRateCostText.text = "FIRE RATE \n" + "$" + fireRateCosts[currentFireRateLevel].ToString();
    
    bs.spawnRate = GetDropSpeed();
        

    }
    public void UpgradeGunFireRate()
    {
        if (currentFireRateLevel < fireRateCosts.Count && CoinManager.Instance.SubtractCoins(fireRateCosts[currentFireRateLevel]))
        {
            currentFireRateLevel++;
            upgradeFireRateCostText.text =   currentFireRateLevel < fireRateCosts.Count ? "FIRE RATE \n"+ "$" + fireRateCosts[currentFireRateLevel].ToString() : "MAX";
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
