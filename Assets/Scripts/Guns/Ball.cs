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
    public float maxVelocity = 5f;
    public List<ColorRange> colorRanges;
    [SerializeField] SpriteRenderer spriteRenderer;

    public float minInitialForce = 1.0f; // minimum initial force
    public float maxInitialForce = 3.0f; // maximum initial force


    public float defaultMass;
    Sequence sequence;
    public Transform target;
    Rigidbody2D rb ;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject AwesomeText;



    private void Start()
    {
        // Initialize the value text
        valueText.text = value.ToString();
        rb = GetComponent<Rigidbody2D>();
        // Apply an initial force
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

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
    public void SetValue(int value)
    {
        this.value = value;
        valueText.text = GameManager.Instance.FormatNumber(value);
        
        health = value;

        Vector3 v = Vector3.zero;
        v = new Vector3(transform.localScale.x + growthRate, transform.localScale.y + growthRate, transform.localScale.z + growthRate);
        if (value >= 100)
        {
            valueText.fontSize = 18;
        }
        if (value >= 1000)
        {
            valueText.fontSize = 15f;
            v = new Vector3(transform.localScale.x + (growthRate / 2), transform.localScale.y + (growthRate / 2), transform.localScale.z + (growthRate / 2));
        }
        transform.localScale = v;
    }
    private void OnMouseDown()
    {
        KillBall();
    }

    public void KillBall()
    {
        CoinManager.Instance.AddCoins(Mathf.RoundToInt(health), transform.position);
        GameObject g = Instantiate(AwesomeText, transform.position, Quaternion.identity);
        g.GetComponent<AwesomeText>().SetText(health);

        // Spawn coins
        for (int i = 0; i < 2; i++)
        {
            Destroy(Instantiate(coinPrefab, transform.position, Quaternion.identity), 10f);
        }
        Destroy(gameObject);
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
        Vector3 v = Vector3.zero;
       v = new Vector3(transform.localScale.x + growthRate, transform.localScale.y + growthRate, transform.localScale.z + growthRate);
        if (value >= 100)
        {
            valueText.fontSize = 18;
        }
        if(value >= 1000)
        {
            valueText.fontSize = 15f;
           v = new Vector3(transform.localScale.x + (growthRate/2), transform.localScale.y + (growthRate/2), transform.localScale.z + (growthRate/2));
        }
        
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
