using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

	private GameObject currentCollision = null;


	void OnTriggerEnter(Collider other) {
		if (currentCollision != null)
			return;
		
		if (other.CompareTag("domino")) {
			// TODO fix parent.parent
			transform.parent.parent.GetComponent<CursorController> ().EditMode(other.transform.parent.gameObject);
			currentCollision = other.gameObject	;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject == currentCollision) {
			// TODO fix parent.parent
			transform.parent.parent.GetComponent<CursorController> ().PlaceMode ();
			currentCollision = null;
		
		}
	}
}
