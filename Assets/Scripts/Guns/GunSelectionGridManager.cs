using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // required for using TextMeshPro
using CrazyGames;

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

    public void OnSpawnButtonClicked()
    {
        if (currentGunIndex >= gunCosts.Count)
        {
            Debug.Log("All guns have been purchased.");
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
        GameObject newGun = Instantiate(gunPrefab, point.position, Quaternion.identity, point);
        Gun gunComponent = newGun.GetComponent<Gun>();
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

    public void CheckForHigherLevel(int lev)
    {
        if(guns.Find(x=>x.level == lev) == null)
        {
            CrazyEvents.Instance.HappyTime();
            UIManager.Instance.EnableNewGun(lev-1);

        }
    }


}
