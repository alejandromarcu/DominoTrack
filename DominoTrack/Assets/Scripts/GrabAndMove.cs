using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndMove : MonoBehaviour {
	public GameObject avatar;

	private bool isMoving = false;
	private Vector3 handStartPos;
	private Vector3 objectStartPos;

    private void Start()
    {
        Game.OnModeChanged += OnGameModeChanged;
    }

    private void OnGameModeChanged(Game.GameMode mode)
    {
        if (mode == Game.GameMode.Run)
        {
            if (isMoving) Release();
        }
    }

    void Update () {
        if (!Game.isBuilding) return;

        if (OVRInput.GetDown (OVRInput.RawButton.RHandTrigger)) {
			Grab ();
		}
		if (OVRInput.GetUp (OVRInput.RawButton.RHandTrigger)) {
			Release ();
		}
		if (isMoving) {
			Move ();
		}
	}

	void Grab () {
		Game.track.Freeze ();
		isMoving = true;
		handStartPos = avatar.transform.TransformPoint(OVRInput.GetLocalControllerPosition (OVRInput.Controller.RTouch));
		objectStartPos = transform.position;
	}

	void Move () {
		var handCurrentPos = avatar.transform.TransformPoint(OVRInput.GetLocalControllerPosition (OVRInput.Controller.RTouch));
		transform.position = (handCurrentPos - handStartPos) + objectStartPos;
	}

	void Release () {
		isMoving = false;
		Game.track.Unfreeze ();
	}
}
