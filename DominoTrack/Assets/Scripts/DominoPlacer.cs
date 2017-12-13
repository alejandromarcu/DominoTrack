using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoPlacer {
	private GameObject piece;  // TODO any way to specify that this needs to be a domino?
	private float distance = 0.03f; // TODO in constructor?
	private float angle = 15; // TODO constr?

	public DominoPlacer(GameObject piece) {
		this.piece = piece;
	}

	public DominoPlacer fwd(int n) {
		return place(n, Vector3.zero);
	}

	public DominoPlacer right(int n) {
		return place(n, Vector3.up * angle);
	}

	public DominoPlacer left(int n) {
		return place(n, Vector3.down * angle);
	}


	private DominoPlacer place(int n, Vector3 angle) {
		for (int i = 0; i < n; i++) {
			Vector3 newPos = piece.transform.position;
			newPos += piece.transform.forward * distance;
			Ray ray = new Ray (new Vector3 (newPos.x, 10, newPos.z), Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 11)) {
				Debug.Log ("Hit! " + hit.distance);
				//piece.transform.position.Set (piece.transform.position.x, hit.point.y, piece.transform.position.z);
				//	newPos.Set(newPos.x, hit.point.y  + i / 10f, newPos.z);	
				newPos.y = hit.point.y;
			} else {
				Debug.Log ("WTF, not hit");
			}
			piece = MonoBehaviour.Instantiate (piece, piece.transform.parent);	
			piece.transform.position = newPos;
			piece.transform.Rotate (angle);

			/*

			piece = MonoBehaviour.Instantiate (piece, piece.transform.parent);	
			piece.transform.position += piece.transform.forward * distance;
			Ray ray = new Ray (new Vector3 (piece.transform.position.x, 10, piece.transform.position.z), Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 11)) {
				Debug.Log ("Hit! " + hit.distance);
				piece.transform.position.Set (piece.transform.position.x, hit.point.y, piece.transform.position.z);
			} else {
				Debug.Log ("WTF, not hit");
			}
			piece.transform.Rotate (angle);
			*/
		}
		return this;
	}
}
