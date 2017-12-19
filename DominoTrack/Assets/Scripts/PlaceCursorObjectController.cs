using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Switch between edit and place modes depending on wheteher there's a collision between the cursor and other pieces
 */
public class PlaceCursorObjectController : MonoBehaviour
{

    private GameObject currentCollision = null;
    public GameObject cursor;
    private CursorController cursorController;

    void Awake()
    {
        cursorController = cursor.GetComponent<CursorController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentCollision != null)
            return;

        if (other.CompareTag("domino"))
        {
            cursorController.StartEditing(other.transform.parent.gameObject);
            currentCollision = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentCollision)
        {
            cursorController.StopEditing(other.transform.parent.gameObject);
            currentCollision = null;
        }
    }
}
