using System.Collections;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    public float health = 100f; // The initial health of the bomb
    public float explosionRadius = 5f; // The radius within which balls will be destroyed when the bomb explodes
    public LayerMask ballLayer; // Set this to the layer where your balls are.
    public float explosionForce = 5f; // The force of the explosion
    public TextMeshPro healthText; // Assign this in Inspector
    Rigidbody2D rb;
    public float minInitialForce = 3.0f; // minimum initial force
    public float maxInitialForce = 9.0f; // maximum initial force
    private void Start()
    {
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

        UpdateHealthText();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthText();

        if (health <= 0)
        {
            Explode();
        }
    }

    private void UpdateHealthText()
    {
        healthText.text = health.ToString();
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, ballLayer);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = rb.transform.position - transform.position;
                direction.Normalize();

                // Apply a force in the direction away from the explosion center.
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }

        StartCoroutine(DestroyBallsAfterDelay(colliders));
    }


    private IEnumerator DestroyBallsAfterDelay(Collider2D[] colliders)
    {
        yield return new WaitForSeconds(1f);

        foreach (Collider2D collider in colliders)
        {
            Ball ball = collider.GetComponent<Ball>();
            if (ball != null)
            {
                ball.KillBall();
            }
        }

        // Optionally, destroy the bomb itself
        Destroy(gameObject);
    }
}
