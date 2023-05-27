using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
[System.Serializable]
public struct ColorRange
{
    public int min;
    public int max;
    public Color color;
}

public class Ball : MonoBehaviour
{
    public int value = 1; // The value of the ball
    public float growthRate = 0.1f; // The rate at which the ball grows in size
    public TextMeshPro valueText; // Assign in Inspector
    public int health;
    public float maxVelocity;
    public List<ColorRange> colorRanges;
    [SerializeField] SpriteRenderer spriteRenderer;

    public float minInitialForce = 1.0f; // minimum initial force
    public float maxInitialForce = 3.0f; // maximum initial force

    public float defaultMass;
    Sequence sequence;
    public Transform target;

    private void Start()
    {
        // Initialize the value text
        valueText.text = value.ToString();
        
        // Apply an initial force
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Add some randomness to the force
            float forceX = Random.Range(minInitialForce, maxInitialForce);
            float forceY = Random.Range(minInitialForce, maxInitialForce);
            Vector2 force = new Vector2(-forceX, forceY);
            rb.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("No Rigidbody2D component found on this object.");
        }
        defaultMass = GetComponent<Rigidbody2D>().mass;
        foreach (var range in colorRanges)
        {
            if (value >= range.min && value <= range.max)
            {
                spriteRenderer.color = range.color;
                break;
            }
        }
    }
    

    public void SetValue(int value)
    {
        this.value = value;
        valueText.text = GameManager.Instance.FormatNumber(value);
        //transform.localScale = Vector3.one;
        health = value;

    }


    private void OnDestroy()
    {
        sequence.Kill();
    }

    public void TakeDamage(int damage)
    {
        // Increase the value of the ball
        value += damage;

        // Update the TextMesh Pro object with the new value
        valueText.text = value.ToString();

        // Scale up the ball over time
        Vector3 v = new Vector3(transform.localScale.x + growthRate, transform.localScale.y + growthRate, transform.localScale.z + growthRate);
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(v, 1f).SetEase(Ease.OutBounce));
        
        health = value;
        // Update the color according to the new value
        foreach (var range in colorRanges)
        {
            if (value >= range.min && value <= range.max)
            {
                spriteRenderer.color = range.color;
                break;
            }
        }
        //GetComponent<Rigidbody2D>().mass = defaultMass + (health / 10);

    }
}
