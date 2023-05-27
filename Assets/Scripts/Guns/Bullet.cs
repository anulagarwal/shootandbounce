using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // The damage dealt by the bullet
    public float speed = 10f; // The speed of the bullet
    private Vector2 targetPos; // The direction towards the target
    public Transform target;
    public GameObject collisionVFX;
    public float collisionDestroyDelay;

    public void SetTarget(Vector2 targetPosition, Transform targ)
    {
        target = targ;

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void FixedUpdate()
    {
        // Move the bullet towards the target
        //GetComponent<Rigidbody2D>().velocity = targetDirection * speed;
        if (target != null)
        {
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit a ball
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            // We hit a ball! Deal damage
            ball.TakeDamage(damage);

            // Destroy the bullet
        }
        Destroy(Instantiate(collisionVFX, collision.contacts[0].point, Quaternion.identity),collisionDestroyDelay);
        Destroy(gameObject);

    }
}
