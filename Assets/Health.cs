using UnityEngine;
using TMPro; // Required for manipulating UI elements

public class Health : MonoBehaviour
{
    public int health; // The health value
    public TextMeshPro healthDisplay; // UI Text component to display the health

    // Call this method to increase the health
    public void IncreaseHealth(int amount)
    {
        health += amount;
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        // Update the text field with the new health value
        healthDisplay.text = "" + health;
    }

    void Start()
    {
        // Initialize the health display
        UpdateHealthDisplay();
    }
}
