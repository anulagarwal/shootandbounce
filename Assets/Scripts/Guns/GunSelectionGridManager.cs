using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // required for using TextMeshPro
using CrazyGames;
using System.Linq;

public class GunSelectionGridManager : MonoBehaviour
{
    public GameObject gunPrefab; // assign in Inspector
    public Transform[] gridPoints; // assign in Inspector
    public Button spawnButton; // assign in Inspector
    public int gunCost = 10; // assign in Inspector
    private CoinManager currencyManager; // assign in Awake
    public List<int> gunCosts; // List of gun costs
    private int currentGunIndex = 0; // The index of the current gun
    public TextMeshProUGUI gunCostText;
    public List<Gun> guns;
    public bool isPopupOpen;
    public Gun currentHighestLevelGun;
    public int highestLevelUnlocked=1;

    int rewardedAdID;
    #region Singleton
    private static GunSelectionGridManager _instance;
    public static GunSelectionGridManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GunSelectionGridManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GunManager");
                    _instance = singleton.AddComponent<GunSelectionGridManager>();
                }
            }
            return _instance;
        }
    }
    #endregion


    void Awake()
    {
        currencyManager = CoinManager.Instance;
        UpdateButtonState();
    }

    void Update()
    {
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        spawnButton.interactable = currencyManager.currentCoins >= gunCosts[currentGunIndex] && HasFreeGridPoint();
        gunCostText.text = "BUY GUN \n $" + gunCosts[currentGunIndex].ToString();

    }

    bool HasFreeGridPoint()
    {
        foreach (Transform point in gridPoints)
        {
            if (point.childCount == 0)
            {
                return true;
            }
        }

        return false;
    }

    Transform GetFreeGridPoint()
    {
        foreach (Transform point in gridPoints)
        {
            if (point.childCount == 0)
            {
                return point;
            }
        }

        return null;
    }
    public void SpawnGun()
    {

    }

    public void ActiveButton(bool active)
    {
        spawnButton.interactable = active;
    }

    public void OnSpawnButtonClicked()
    {
        if (TutorialManager.Instance != null)
        {
            if (TutorialManager.Instance.isEnabled)
            {
                TutorialManager.Instance.NextStep();                
            }
        }
        if (currentGunIndex >= gunCosts.Count)
        {
            Debug.Log("All guns have been purchased.");
            return;
        }

        if (gunCosts[currentGunIndex] == 0)
        {
            //Watch Ad
            WatchedRewardedAd(4);
            return;
        }

        if (currencyManager.currentCoins >= gunCosts[currentGunIndex])
        {
            Transform freePoint = GetFreeGridPoint();
            if (freePoint != null)
            {
                SpawnGun(freePoint);
                currencyManager.SubtractCoins(gunCosts[currentGunIndex]);
                currentGunIndex++; // Go to the next gun

                // Update gun cost text
                if (currentGunIndex < gunCosts.Count)
                {
                    gunCostText.text = "BUY GUN \n $" + gunCosts[currentGunIndex].ToString();
                }
                else
                {
                    currentGunIndex = gunCosts.Count - 1;
                }
            }
        }
        else if (WatchAdForPurchase())
        {
            // Prompt the player to watch an ad
        }
    }

    bool WatchAdForPurchase()
    {
        // Implement the logic to determine when the player should be prompted to watch an ad
        // For example, return true every 5th purchase:
        // return currentGunIndex % 5 == 0;
        return false;
    }

    void SpawnGun(Transform point)
    {
        int x = GetGunSpawnLevel();
        Vector3 v = point.position;
        v.z = 0;
        GameObject newGun = Instantiate(gunPrefab, v, Quaternion.identity);
        newGun.transform.SetParent(point);
        Gun gunComponent = newGun.GetComponent<Gun>();
        gunComponent.EnableGunLevel(x);
        if (gunComponent != null)
        {
            gunComponent.isPlaced = true;
            gunComponent.isInGrid = true; // add this line
            guns.Add(gunComponent);
        }
        else
        {
            Debug.LogError("Spawned gun does not have a Gun component");
        }
    }

    public int GetGunSpawnLevel()
    {
        int i = GetHighestLevel();
       
       
            i = Random.Range(GetHighestLevel()-4, GetHighestLevel()-2);
        float x = Random.Range(0, 1.0f);
            
        if (x < 0.12f)
        {
            i = GetHighestLevel() - 2;
        }
        if (x < 0.05f)
        {
            i = GetHighestLevel() - 1;
        }
        if (i < 1)
        {
            i = 1;
        }
        return i;
    }


    public void CheckForHigherLevel(int lev, Gun g)
    {
        if(guns.Find(x=>x.level == lev) == null && lev > highestLevelUnlocked)
        {
            CrazyEvents.Instance.HappyTime();
            UIManager.Instance.EnableNewGun(lev-1);
            currentHighestLevelGun = g;
            highestLevelUnlocked = lev;
        }
    }

    //Gun Manager
    //Crit value - apply to every bullet hit for gun
    //Clone value - apply to every bullet hit for gun
    //Damage upgrade - apply to gun

    public void CallBackRewardedAd()
    {
        switch (rewardedAdID)
        {
            //Crit Upgrade
            case 1:
                currentHighestLevelGun.UpdateCritChance(0.5f);
                break;

            //Clone Upgrade
            case 2:
                currentHighestLevelGun.UpdateCloneChance(0.5f);
                break;

            //Damage Upgrade
            case 3:
                currentHighestLevelGun.UpdateDamage(currentHighestLevelGun.damage);
                UIManager.Instance.OnClickNewGunClose();
                break;

            //Purchase Gun
            case 4:
                Transform freePoint = GetFreeGridPoint();
                if (freePoint != null)
                {
                    SpawnGun(freePoint);
                    currentGunIndex++; // Go to the next gun

                    // Update gun cost text
                    if (currentGunIndex < gunCosts.Count)
                    {
                        gunCostText.text = "BUY GUN \n $" + gunCosts[currentGunIndex].ToString();
                    }
                    else
                    {
                        currentGunIndex = gunCosts.Count - 1;
                    }
                }
                break;

            //Drop Speed Upgrade
            case 5:
                UpgradeManager.instance.UpgradeBallDropSpeedFree();
                break;

            //Drop Value Upgrade
            case 6:
                UpgradeManager.instance.UpgradeBallDropValueFree();
                break;

            //Fire Rate Upgrade
            case 7:
                UpgradeManager.instance.UpgradeGunFireRateFree();
                break;
        }
    }

    public void WatchedRewardedAd(int val)
    {
        rewardedAdID = val;
        switch (val)
        {
            //Crit Upgrade
            case 1:
                break;

            //Clone Upgrade
            case 2:

                break;

            //Damage Upgrade
            case 3:
                AdManager.Instance.WatchRewardedAdForDamageUpgrade();
                break;

            //Purchase Gun
            case 4:
                AdManager.Instance.WatchRewardedAdForDamageUpgrade();
                break;

            //Drop Speed Upgrade
            case 5:
                AdManager.Instance.WatchRewardedAdForDamageUpgrade();
                break;

            //Drop Value Upgrade
            case 6:
                AdManager.Instance.WatchRewardedAdForDamageUpgrade();
                break;

            //Fire Rate Upgrade
            case 7:
                AdManager.Instance.WatchRewardedAdForDamageUpgrade();
                break;
        }

    }
    
    

    // C#
    static int SortByScore(Gun p1, Gun p2)
    {
        return p1.level.CompareTo(p2.level);
    }

    public int GetHighestLevel()
    {
        if (guns.Count == 0)
        {
            return 1;
        }
        else
        {
             guns.Sort(SortByScore);

            return guns[guns.Count - 1].level;
        }
    }

    public void PopupClosed()
    {
        isPopupOpen = false;
    }


}
