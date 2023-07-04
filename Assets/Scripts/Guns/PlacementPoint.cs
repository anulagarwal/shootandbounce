using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

public class PlacementPoint : MonoBehaviour
{
    public bool isOccupied = false; // Whether the point is currently occupied by a gun
    public bool isUpgradable = false; // Whether the point can be upgraded
    public int upgradeCost = 100; // Cost to upgrade the point
    public Transform mergeText;
    public bool isLocked = false;
    public TextMeshPro levelText;
    public GameObject lockObject;
    public TextMeshPro lockCost;
    public Color unlockColor;
    public Color lockColor;

    public void Start()
    {
        if (isLocked)
        {
            isOccupied = true;
            lockObject.SetActive(true);
            lockCost.text = "$" + GameManager.Instance.FormatNumber(upgradeCost);
        }
        else
        {
            //isOccupied = false;
            lockObject.SetActive(false);
        }
        UpdateUnlockText(CoinManager.Instance.currentCoins);
    }

    public void EnableMergeText()
    {
        if (mergeText != null)
        {
            mergeText.gameObject.SetActive(true);
        }
      
    }
    public void DisableMergeText()
    {
        if (mergeText != null)
        {
            mergeText.gameObject.SetActive(false);
        }
    }

    IEnumerator UnlockObject()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(1.75f);
        lockObject.SetActive(false);
        isOccupied = false;
        isLocked = false;

        // Code to execute after the delay goes here...
    }

    public void UpdateUnlockText(int c)
    {
       if (c >= upgradeCost)
        {
            lockCost.color = unlockColor;
        }
        else
        {
            lockCost.color = lockColor;
        }
    }
    public void UnlockQuick()
    {
        lockObject.SetActive(false);
        isOccupied = false;
        isLocked = false;
    }
    public void Unlock()
    {
        if (isLocked)
        {
            if (CoinManager.Instance.SubtractCoins(upgradeCost, transform.position))
            {
                StartCoroutine(UnlockObject());
            }

        }
    }

    private void OnMouseDown()
    {
        if (isLocked)
        {
            Unlock();
        }
    }
    // Function to upgrade the point
    public void Upgrade()
    {
        if (!isUpgradable)
        {
            Debug.Log("This point is not upgradable.");
            return;
        }



        // Subtract the cost from the player's coins here

        // Upgrade the point (e.g., increase its size, change its appearance, etc.)
        // This will be specific to your game's mechanics
    }
}
