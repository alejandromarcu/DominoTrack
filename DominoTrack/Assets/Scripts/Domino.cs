﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Domino {
	enum StartModes { NoStart, Forward, Backward};

	public Vector3 localPosition { get; private set; }
	public Vector3 position { 
		get {
			return Game.track.trackGameObject.transform.TransformPoint (localPosition);
		} 
		set {
			localPosition = Game.track.trackGameObject.transform.InverseTransformPoint (value);
		}
	}

	public float rotationY { get; private set; }
	public GameObject model  { get; private set; }
	public GameObject startArrowModel  { get; private set; }
	private StartModes startMode = StartModes.NoStart;
	private GameObject gameObject { get; set; } 
	private GameObject startArrow;
	public Vector3 forward { get { return gameObject.transform.forward; } }
    private Rigidbody rigidbody;

	public Domino(Vector3 worldPosition, float rotationY, GameObject model, GameObject startArrowModel) {
		this.position = worldPosition;
		this.rotationY = rotationY;
		this.model = model;
		this.startArrowModel = startArrowModel;
	}

	public void ResetGameObject() {
		DestroyGameObject ();
		var rotation = Quaternion.Euler (0f, rotationY, 0f);
		gameObject = MonoBehaviour.Instantiate (model, position, rotation, model.transform.parent);
		gameObject.SetActive(true);

        rigidbody = gameObject.GetComponentInChildren<Rigidbody> ();
		RefreshStartArrow ();
	}

	public void Freeze() {
		if (gameObject != null) {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	public void Unfreeze() {
		if (gameObject != null) {
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.Sleep ();
		}
	}

	public void DestroyGameObject() {
		if (gameObject != null) {
			Object.Destroy (gameObject);
		}
	}

	public void ToggleStart() {
		switch (startMode) {
		case StartModes.NoStart:
			startMode = StartModes.Forward;
			break;
		case StartModes.Forward:
			startMode = StartModes.Backward;
			break;
		case StartModes.Backward:
			startMode = StartModes.NoStart;
			break;	
		}
		RefreshStartArrow ();
	}

	private void RefreshStartArrow() {
		if (startArrow) {
			Object.Destroy (startArrow);
		}

		if (startMode != StartModes.NoStart) {
			startArrow = MonoBehaviour.Instantiate (startArrowModel, position, gameObject.transform.rotation, gameObject.transform);
			startArrow.SetActive (true);
			if (startMode == StartModes.Backward) {
				startArrow.transform.Rotate (Vector3.up * 180);
			}
		}
	}

	public void KickOffIfNeeded() {
		if (startMode == StartModes.NoStart) {
			return;
		}
		int direction = startMode == StartModes.Forward ? 1 : -1;

        rigidbody.AddForce (gameObject.transform.forward * 0.1f * direction, ForceMode.Impulse);
	}

	public bool MatchesGameObject(GameObject obj) {
		return obj == gameObject;
	}
}
