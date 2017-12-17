using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class DominoController : MonoBehaviour {

	private AudioSource dominoSound;

	// Use this for initialization
	void Start () {
		dominoSound = GetComponent<AudioSource> ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision c) {
        if (c.rigidbody != null && c.rigidbody.gameObject.CompareTag("domino"))
        {
			dominoSound.Play();
		}
	}
}
