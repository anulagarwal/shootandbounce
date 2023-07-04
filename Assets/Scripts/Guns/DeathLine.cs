using UnityEngine;
using DG.Tweening;
public class DeathLine : MonoBehaviour
{
    public GameObject coinPrefab; // assign in Inspector
    public int coinsPerBall = 10; // adjust as needed
    public Transform saw; // adjust as needed
    public Transform moneyPoint; // adjust as needed

    public float speed;
    public GameObject awesomeText;
    private void Start()
    {
        saw.DORotate(new Vector3(0f, 0f, 1)* speed, 1f, RotateMode.FastBeyond360)
                 .SetLoops(-1, LoopType.Incremental)
                 .SetEase(Ease.Linear);
    }

    public void Rotate()
    {
        saw.DORotate(new Vector3(0f, 0f, 1) * speed, 1f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            Vector3 v = new Vector3(ball.transform.position.x, ball.transform.position.y, 0.25f);

           

            // Spawn coins
            for (int i = 0; i < coinsPerBall; i++)
            {
               Destroy(Instantiate(coinPrefab, moneyPoint.position, Quaternion.identity),10f);
            }
            
           
                // Add coins equal to the ball's health
                CoinManager.Instance.AddCoins(Mathf.RoundToInt(ball.health), ball.transform.position);
            v.z = 0.4f;
            GameObject g = Instantiate(awesomeText, v, Quaternion.identity);
            g.GetComponent<AwesomeText>().SetText(ball.health);

            // Destroy the ball
            Destroy(ball.gameObject);
        }
    }
}
