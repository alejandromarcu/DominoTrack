using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDomino : MonoBehaviour {

	public GameObject firstPiece;

	private bool fired = false;

	void Start() {
		/*
		for (int i = 1; i < 50; i++) {
		//	GameObject newPiece = Instantiate (firstPiece, firstPiece.transform.position + new Vector3 (0.1f, 0, 0) * i, Quaternion.identity, firstPiece);	
		//	newPiece.transform.parent = firstPiece.transform.parent;
			GameObject newPiece = Instantiate (firstPiece, firstPiece.transform.parent);	
			newPiece.transform.position += newPiece.transform.forward * 0.05f * i;
		}*/
		/*
		new DominoPlacer (firstPiece)
			.fwd (10)
			.right (6)
			.fwd (10)
			.left (6)
			.fwd (20)
			.left (12)
			.fwd (5)
			.right (6)
			.fwd (10)
			.right (6)
			.fwd (10)
			.right (6)
			.fwd (20);
//			.right (6)
//			.fwd (40);
		*/
		/*
		new DominoPlacer (firstPiece)
			.fwd (50)
			.right (12)
			.fwd (50)
			.left (12)
			.fwd (50)
			.right (12)
			.fwd(50);*/

	}

	// Update is called once per frame
	void Update () {
		/*

		if (Input.GetButtonDown("Fire1") && !fired) {
			fired = true;
			firstPiece.GetComponent<Rigidbody> ().isKinematic = false;
			firstPiece.GetComponent<Rigidbody>().AddForce (firstPiece.transform.forward * 0.02f, ForceMode.Impulse);
		}
		if (Input.GetButtonDown("Fire2") && fired) {
			fired = false;
			SceneManager.LoadScene ("Domino1");
		}
		*/
	}
}
