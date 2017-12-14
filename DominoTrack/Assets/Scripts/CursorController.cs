using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO it would be nice if the edit cursor didn't appear when I just place a piece
public class CursorController : MonoBehaviour {

	public GameObject placeCursor;
	public GameObject editCursor;

	public GameObject model;
	public GameObject startArrowModel;

	public GameObject avatar;

	private Renderer placeCursorRenderer;
	private Renderer editCursorRenderer;

	private bool editMode = false;
	private GameObject editPiece = null;

	// This should go on the root class
	private bool fired = false;

	void Start () {
		placeCursorRenderer = placeCursor.GetComponentsInChildren<Renderer> ()[0];	
		editCursorRenderer = editCursor.GetComponentsInChildren<Renderer> ()[0];
	}


	public void EditMode(GameObject obj) {
		editCursorRenderer.enabled = true;
		placeCursorRenderer.enabled = false;
		editCursor.transform.position = obj.transform.position;
		editMode = true;
		editPiece = obj;
	}

	public void PlaceMode() {
		editCursorRenderer.enabled = false;
		placeCursorRenderer.enabled = true;
		editMode = false;
		editPiece = null;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Fire1") && !fired && !editMode) {
			fired = true;
			Game.track.KickOff ();
		}

		if (Input.GetButtonDown("Fire2")) {
			Game.track.Reset ();
			fired = false;
		}


		if (Input.GetButtonDown("Fire3")) {
			SceneManager.LoadScene ("Domino1");
		}


		var handPosition = avatar.transform.TransformPoint(OVRInput.GetLocalControllerPosition (OVRInput.Controller.RTouch));

		// TODO, I want to calibrate this but the magic board stops working!
		float handOffset = 0.1f; // TODO constant

		Ray ray = new Ray (handPosition + Vector3.down * handOffset, Vector3.down);
		RaycastHit hit;
		if (!Physics.Raycast (ray, out hit, 0.05f)) { // TODO constant
			editCursorRenderer.enabled = false;
			placeCursorRenderer.enabled = false;
			editMode = false;
			return;
		}


		// We need to reposition the place cursor even in edit mode, so that the collider of the cursor may exit
		// and signal the end of edit mode.
		var dominoPosition = handPosition + Vector3.down * (hit.distance + handOffset);
		Quaternion handRotation = OVRInput.GetLocalControllerRotation (OVRInput.Controller.RTouch);
		Quaternion dominoRotation = Quaternion.Euler (0f, handRotation.eulerAngles.y, 0f);

		placeCursor.transform.SetPositionAndRotation (dominoPosition, dominoRotation);

		if (editMode) {
			HandleEditMode ();
			return;
		}

		placeCursorRenderer.enabled = true;


		if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) {
			var domino = new Domino(placeCursor.transform.position, handRotation.eulerAngles.y, model, startArrowModel);
			Game.track.Place (domino);
		//	lastPiecePlacedWithJoystick = null;
		}
	}

	private void HandleEditMode() {
		if (OVRInput.GetDown (OVRInput.RawButton.RIndexTrigger)) {
			Game.track.Remove (Game.track.Find(editPiece));
			PlaceMode ();
		}

		if (Input.GetButtonDown ("Fire1")) {
			var domino = Game.track.Find (editPiece);
			domino.ToggleStart ();

		}
	}
	/*
	private double earliestTimeToPlaceNextJoystickPiece = 0;
	private Domino lastPiecePlacedWithJoystick = null;

	private void HandleJoystickPlacing() {
		if (Time.time < earliestTimeToPlaceNextJoystickPiece) {
			return;
		}
		// TODO prevent from removing pieces that were not placed with joystick.  I think I just need to place a flag in domino

		Vector2 control = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
	


		if (control.y > 0.2) {
			lastPiecePlacedWithJoystick = Game.track.PlaceNext (0);
		} else if (control.x > 0.2) {
			lastPiecePlacedWithJoystick = Game.track.PlaceNext (15); // todo constant
		} else if (control.x < -0.2) { 
			lastPiecePlacedWithJoystick = Game.track.PlaceNext (-15); // todo constant
		} else if (control.y < -0.2) {
			if (Equals(lastPiecePlacedWithJoystick, Game.track.last)) {
				Game.track.Remove (Game.track.last);
			}
			lastPiecePlacedWithJoystick = Game.track.last;
		}


	
	//	earliestTimeToPlaceNextJoystickPiece = Time.time + 0.1;
		earliestTimeToPlaceNextJoystickPiece = Time.time +  0.15 / (control.magnitude + 0.5);

	}
	*/
}
