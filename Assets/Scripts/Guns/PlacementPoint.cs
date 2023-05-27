using UnityEngine;
using DG.Tweening;
public class PlacementPoint : MonoBehaviour
{
    public bool isOccupied = false; // Whether the point is currently occupied by a gun
    public bool isUpgradable = false; // Whether the point can be upgraded
    public int upgradeCost = 100; // Cost to upgrade the point
    public Transform mergeText;


    public void EnableMergeText()
    {
        mergeText.gameObject.SetActive(true);
      
    }
    public void DisableMergeText()
    {
        mergeText.gameObject.SetActive(false);
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
