using UnityEngine;
using DG.Tweening;
public class DragAndDrop : MonoBehaviour
{
    public LayerMask placementLayer;
    public LayerMask binLayer; // Layer to detect if the gun is being placed on a valid point
    public float returnSpeed = 1f; // speed at which the gun returns to the original position

    private Vector3 originalPosition;
    private bool dragging = false;
    private bool placedSuccessfully = false;
    public LayerMask gunLayer;  // assign in Inspector
    public LayerMask gridLayer;
    public Gun lastTouchedGun = null;
    [SerializeField] private GameObject rangeIndicatorPrefab;
    private GameObject rangeIndicatorInstance;

    private void Start()
    {
        originalPosition = transform.position;
    }
    private void OnMouseDown()
    {
        // Start of a new drag operation
        dragging = true;
        placedSuccessfully = false;  // Reset the placement flag
        rangeIndicatorInstance = Instantiate(rangeIndicatorPrefab, transform);
        rangeIndicatorInstance.transform.localScale = new Vector3(GetComponent<Gun>().detectionRadius, GetComponent<Gun>().detectionRadius, 1);

        GunSelectionGridManager.Instance.BinActive(true);
        // Record the original position
       if(!GetComponent<Gun>().isInGrid && GetComponent<Gun>().isPlaced)
        {
            originalPosition = GetComponent<Gun>().currentPlacementPoint.transform.position;
        }
        GetComponent<Gun>().isPlaced = false;  // Add this line
        if (GetComponent<Gun>().currentPlacementPoint != null)
            GetComponent<Gun>().currentPlacementPoint.levelText.gameObject.SetActive(false);
    }


    private void OnMouseDrag()
    {
        if (dragging)
        {
            // Update the position of the gun to follow the cursor
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
            rangeIndicatorInstance.transform.position = transform.position;
            rangeIndicatorInstance.transform.localScale = new Vector3(GetComponent<Gun>().detectionRadius, GetComponent<Gun>().detectionRadius, 1);

            Collider2D[] overlappingGuns = Physics2D.OverlapCircleAll(transform.position, 0.1f, gunLayer);
            foreach (Collider2D collider in overlappingGuns)
            {
                Gun otherGun = collider.GetComponent<Gun>();
                if (otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level && (otherGun.isPlaced) && lastTouchedGun == null && !otherGun.isInGrid && otherGun.level < GetComponent<Gun>().gunModels.Length)
                {
                    lastTouchedGun = otherGun;
                    lastTouchedGun.currentPlacementPoint.EnableMergeText();
                    break;
                }

                if (otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level  && lastTouchedGun == null && otherGun.isInGrid && otherGun.level < GetComponent<Gun>().gunModels.Length)
                {
                    lastTouchedGun = otherGun;
                    break;
                }

                if(otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level && lastTouchedGun == null && (otherGun.isInGrid || (otherGun.isPlaced && !otherGun.isInGrid)) && otherGun.level >= GetComponent<Gun>().gunModels.Length)
                {
                    lastTouchedGun = null;
                }
                if (otherGun == null && lastTouchedGun != null && lastTouchedGun.isInGrid)
                {
                    lastTouchedGun = null;
                } 
                if (otherGun == null && lastTouchedGun != null)
                {
                    lastTouchedGun.currentPlacementPoint.DisableMergeText();
                    lastTouchedGun = null;
                }
            }

            if (overlappingGuns.Length == 1 && overlappingGuns[0].GetComponent<Gun>() == GetComponent<Gun>() && lastTouchedGun != null && !lastTouchedGun.isInGrid)
            {
                lastTouchedGun.currentPlacementPoint.DisableMergeText();
                lastTouchedGun = null;
            }
            if (overlappingGuns.Length == 1 && overlappingGuns[0].GetComponent<Gun>() == GetComponent<Gun>() && lastTouchedGun != null && lastTouchedGun.isInGrid)
            {
                lastTouchedGun = null;
            }
        }
    }

    void OnMouseUp()
    {
        if (dragging)
        {
            Destroy(rangeIndicatorInstance);

            Collider2D hitCollider = Physics2D.OverlapPoint(transform.position, placementLayer);
            Collider2D deleteCollider = Physics2D.OverlapPoint(transform.position, binLayer);
            if(deleteCollider != null)
            {
                if (GetComponent<Gun>().currentPlacementPoint != null)
                    GetComponent<Gun>().currentPlacementPoint.isOccupied = false;

                GunSelectionGridManager.Instance.RemoveGun(GetComponent<Gun>());
                GunSelectionGridManager.Instance.BinActive(false);

                return;
            }
            GunSelectionGridManager.Instance.BinActive(false);


            if (hitCollider != null)
            {
                PlacementPoint placementPoint = hitCollider.GetComponent<PlacementPoint>();
                if (!placementPoint.isOccupied)
                {
                    // Check for a potential merge


                    placedSuccessfully = true;
                    transform.position = hitCollider.transform.position;

                    // Record the original position
                   if (!GetComponent<Gun>().isInGrid && GetComponent<Gun>().isPlaced)
                    {
                        originalPosition = GetComponent<Gun>().currentPlacementPoint.transform.position;
                    }
                    placementPoint.isOccupied = true;

                    if (GetComponent<Gun>().currentPlacementPoint != null)
                        GetComponent<Gun>().currentPlacementPoint.isOccupied = false;

                    GetComponent<Gun>().currentPlacementPoint = placementPoint;
                    GetComponent<Gun>().isPlaced = true;
                    GetComponent<Gun>().currentPlacementPoint.levelText.text = GetComponent<Gun>().level + "";
                    GetComponent<Gun>().currentPlacementPoint.levelText.gameObject.SetActive(true);
                    GetComponent<Gun>().ScaleGunToNormal();
                    SaveManager.Instance.DoSaveGame();
                    if (GetComponent<Gun>().isInGrid)
                    {
                        transform.SetParent(null);
                        GetComponent<Gun>().isInGrid = false;
                    }


                    if (TutorialManager.Instance != null)
                    {
                        if (TutorialManager.Instance.isEnabled)
                        {
                            TutorialManager.Instance.NextStep();
                        }
                    }
                }
               
            }

            if (lastTouchedGun != null)
            {
                Gun otherGun = lastTouchedGun;
                if (otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level && (otherGun.isPlaced))
                {
                    // Merge the guns


                    if (GetComponent<Gun>().currentPlacementPoint != null)
                    {
                        GetComponent<Gun>().currentPlacementPoint.isOccupied = false;

                        GetComponent<Gun>().currentPlacementPoint.levelText.gameObject.SetActive(false);
                    }
                    if (otherGun.currentPlacementPoint != null)
                    {
                        otherGun.currentPlacementPoint.levelText.gameObject.SetActive(true);
                        otherGun.currentPlacementPoint.levelText.text = otherGun.level + "";
                        otherGun.currentPlacementPoint.DisableMergeText();
                    }
                   
                    otherGun.MergeWith(GetComponent<Gun>());

                    placedSuccessfully = true;                    
                }
                else if (otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level && (otherGun.isInGrid))
                {
                    // Merge the guns                       
                    otherGun.MergeWith(GetComponent<Gun>());
                    placedSuccessfully = true;
                   
                }
            }

            if (!placedSuccessfully && !GetComponent<Gun>().isInGrid)
            {
                transform.DOMove(originalPosition, returnSpeed);
                GetComponent<Gun>().currentPlacementPoint.levelText.text = GetComponent<Gun>().level + "";
                GetComponent<Gun>().currentPlacementPoint.levelText.gameObject.SetActive(true);
                GetComponent<Gun>().isPlaced = true;
            }
            else if (!placedSuccessfully && GetComponent<Gun>().isInGrid)
            {
                transform.DOMove(originalPosition, returnSpeed);
            }

            dragging = false;
        }
    }






}