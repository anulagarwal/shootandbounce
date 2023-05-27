using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;
    private Vector2 currentSwipe;

    private float moveSpeed = 5.0f;

    private void Update()
    {
        Swipe();
    }

    private void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Capture the start position of the touch
            startTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Capture the end position of the touch
            endTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Create a swipe vector
            currentSwipe = new Vector2(endTouchPosition.x - startTouchPosition.x, endTouchPosition.y - startTouchPosition.y);

            // Normalize the swipe
            currentSwipe.Normalize();

            // Move and rotate the character based on the swipe
            if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
            {
                MoveCharacter(new Vector3(currentSwipe.x, 0, 0));
                RotateCharacter(currentSwipe.x > 0 ? 1 : -1);
            }
        }
    }

    private void MoveCharacter(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * moveSpeed;
    }

    private void RotateCharacter(int direction)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }
}
