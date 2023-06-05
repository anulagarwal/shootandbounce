using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Gun : MonoBehaviour
{
    public float fireRate = 1f; // bullets per second
    public GameObject ballPrefab; // assign in Inspector
    public float shootForce = 10f; // adjust as needed
    public float detectionRadius = 5f; // adjust as needed
    public LayerMask ballLayer; // assign in Inspector
    private Transform targetBall;
    private float nextFireTime = 0f;
    public bool isInGrid = false;
    public int level = 1;
    public GameObject[] gunModels;
    public bool isPlaced = false;  // Add this line

    public PlacementPoint currentPlacementPoint;
    public Transform shooterPos;


    public List<GameObject> bulletPrefabs;
    public List<float> gunDamages;
    public List<float> gunFireRates;
    public List<float> ranges;


    public float critChance;
    public float cloneChance;   
    public int damage;
   
    private void Start()
    {
        EnableGunLevel(level);
    }
    void Update()
    {
        // Check for balls within the detection radius
        if (!isInGrid)
        {
            Collider2D[] ballsInRadius = Physics2D.OverlapCircleAll(transform.position, detectionRadius, ballLayer);

            // If no balls are in range, stop
            if (ballsInRadius.Length == 0)
            {
                return;
            }

            // Find the closest ball
            Collider2D closestBall = null;
            float closestDistance = Mathf.Infinity;
            foreach (Collider2D ball in ballsInRadius)
            {
                float distance = Vector2.Distance(transform.position, ball.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBall = ball;
                    
                        targetBall = ball.transform;
                    
                }
            }

            // Point the gun towards the closest ball
            Vector2 directionToBall = closestBall.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToBall.y, directionToBall.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // If it's time to fire, fire
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + 1f / (fireRate + UpgradeManager.instance.GetFireRate());
                
            }
        }
    }

    void Fire()
    {
        // Create a new ball and apply a force to it
        GameObject bullet = Instantiate(bulletPrefabs[level-1], shooterPos.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<Bullet>().damage = damage;
        rb.AddForce(transform.right * shootForce, ForceMode2D.Impulse);
        Bullet b = bullet.GetComponent<Bullet>();
        b.SetTarget(targetBall.position, targetBall);
        SoundManager.Instance.Play(level);
    }

    public void UpdateCritChance(float f)
    {
        critChance = f;
    }

    public void UpdateCloneChance(float f)
    {
        cloneChance = f;
    }

    public void UpdateDamage(int f)
    {
        damage += f;
    }
    public void MergeWith(Gun other)
    {
       
        // Merge the guns: delete the other gun and increase the level of this one
        GunSelectionGridManager.Instance.guns.Remove(other);
        if (!other.isInGrid)
        {
            other.currentPlacementPoint.isOccupied = false;
        }
        Destroy(other.gameObject);
        GunSelectionGridManager.Instance.CheckForHigherLevel(level+1, this);
        level++;
        
        EnableGunLevel(level);
        if (TutorialManager.Instance != null)
        {
            if (TutorialManager.Instance.isEnabled)
            {
                TutorialManager.Instance.NextStep();
            }
        }
        /*  // Activate the new model
          if (level - 1 < gunModels.Length)
          {
              gunModels[level - 1].SetActive(true);
          }

          damage = Mathf.RoundToInt(UpgradeManager.instance.GetGunDamage(level));
          fireRate = UpgradeManager.instance.GetGunFireRate(level);
          detectionRadius = UpgradeManager.instance.GetGunRange(level);*/
    }

   public void EnableGunLevel(int lev)
    {
        foreach(GameObject g in gunModels)
        {
            g.SetActive(false);
        }
        if (lev - 1 < gunModels.Length)
        {
            gunModels[lev - 1].SetActive(true);
        }

        damage = Mathf.RoundToInt(gunDamages[lev-1]);
        fireRate = Mathf.RoundToInt(gunFireRates[lev - 1]);
        detectionRadius = Mathf.RoundToInt(ranges[lev - 1]);
        level = lev;
        if (currentPlacementPoint != null)
        {
            currentPlacementPoint.levelText.text = level + "";
            currentPlacementPoint.levelText.gameObject.SetActive(true);
        }
    }
    public void ScaleGunToNormal()
    {
        foreach(GameObject g in gunModels)
        {
            g.transform.DOScale(Vector3.one, 0.5f);
        }
    }
    public void ScaleGunToGrid()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Gun otherGun = collision.gameObject.GetComponent<Gun>();

        if (otherGun != null)
        {
        }
        if (otherGun != null && otherGun.level == this.level && this.isPlaced && otherGun.isPlaced)
        {
            /*// Deactivate the old model
            if (level - 1 < gunModels.Length)
            {
                gunModels[level - 1].SetActive(false);
            }

            // Merge the guns: delete the other gun and increase the level of this one
            Destroy(otherGun.gameObject);
            this.level++;

            // Activate the new model
            if (level - 1 < gunModels.Length)
            {
                gunModels[level - 1].SetActive(true);
            }*/
            MergeWith(otherGun);
        }
    }

}
