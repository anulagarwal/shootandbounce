using UnityEngine;
using DG.Tweening;

public class DragAndDrop : MonoBehaviour
{
    public LayerMask placementLayer; // Layer to detect if the gun is being placed on a valid point
    public float returnSpeed = 1f; // speed at which the gun returns to the original position

    private Vector3 originalPosition;
    private bool dragging = false;
    private bool placedSuccessfully = false;
    public LayerMask gunLayer;  // assign in Inspector
    public LayerMask gridLayer;
    public Gun lastTouchedGun = null;
    private void OnMouseDown()
    {
        // Start of a new drag operation
        dragging = true;
        placedSuccessfully = false;  // Reset the placement flag
        

                // Record the original position
                originalPosition = transform.position;
        GetComponent<Gun>().isPlaced = false;  // Add this line
    }


    private void OnMouseDrag()
    {
        if (dragging)
        {
            // Update the position of the gun to follow the cursor
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
            Collider2D[] overlappingGuns = Physics2D.OverlapCircleAll(transform.position, 0.1f, gunLayer);
            foreach (Collider2D collider in overlappingGuns)
            {
                Gun otherGun = collider.GetComponent<Gun>();
                if (otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level && (otherGun.isPlaced) && lastTouchedGun == null)
                {
                    lastTouchedGun = otherGun;
                    lastTouchedGun.currentPlacementPoint.EnableMergeText();
                    break;
                }
                if(otherGun==null && lastTouchedGun != null)
                {
                    lastTouchedGun.currentPlacementPoint.DisableMergeText();
                    lastTouchedGun = null;
                }
            }
            if(overlappingGuns.Length == 1 && overlappingGuns[0].GetComponent<Gun>() == GetComponent<Gun>() && lastTouchedGun !=null)
            {
                lastTouchedGun.currentPlacementPoint.DisableMergeText();
                lastTouchedGun = null;                
            }
        }
    }

    void OnMouseUp()
    {
        if (dragging)
        {
            Collider2D hitCollider = Physics2D.OverlapPoint(transform.position, placementLayer);
            if (hitCollider != null)
            {
                PlacementPoint placementPoint = hitCollider.GetComponent<PlacementPoint>();
                if (!placementPoint.isOccupied)
                {
                    // Check for a potential merge


                    placedSuccessfully = true;
                    transform.position = hitCollider.transform.position;
                    originalPosition = transform.position;
                    placementPoint.isOccupied = true;

                    if(GetComponent<Gun>().currentPlacementPoint!=null)
                    GetComponent<Gun>().currentPlacementPoint.isOccupied = false;

                    GetComponent<Gun>().currentPlacementPoint = placementPoint;
                    GetComponent<Gun>().isPlaced = true;

                    if (GetComponent<Gun>().isInGrid)
                    {
                        transform.SetParent(null);
                        GetComponent<Gun>().isInGrid = false;
                    }
                }
                else
                {
                    Collider2D[] overlappingGuns = Physics2D.OverlapCircleAll(transform.position, 0.1f, gunLayer);
                    foreach (Collider2D collider in overlappingGuns)
                    {
                        Gun otherGun = collider.GetComponent<Gun>();
                        if (otherGun != null && otherGun != GetComponent<Gun>() && otherGun.level == GetComponent<Gun>().level && (otherGun.isPlaced))
                        {
                            // Merge the guns
                            if (GetComponent<Gun>().currentPlacementPoint!=null)
                            GetComponent<Gun>().currentPlacementPoint.isOccupied = false;
                            otherGun.currentPlacementPoint.DisableMergeText();
                            otherGun.MergeWith(GetComponent<Gun>());

                            break;
                        }
                    }
                }
            }



            if (!placedSuccessfully)
            {
                transform.DOMove(originalPosition, returnSpeed);
            }

            dragging = false;
        }
    }

   




}
