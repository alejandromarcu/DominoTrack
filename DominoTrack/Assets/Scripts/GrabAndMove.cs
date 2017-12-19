using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabAndMove : MonoBehaviour {
	public GameObject avatar;

	private bool isMoving = false;
	private Vector3 handStartPos;
	private Vector3 objectStartPos;
    private float lowestPoint;

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
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        lowestPoint = renderers.Min(r => r.bounds.min.y);
	}

	void Move () {
		var handCurrentPos = avatar.transform.TransformPoint(OVRInput.GetLocalControllerPosition (OVRInput.Controller.RTouch));
        // If trying to go below the floor, just move the hand start position so that it doesn't happen
        if (handStartPos.y - handCurrentPos.y > lowestPoint)
        {
            handStartPos.y = handCurrentPos.y + lowestPoint;
        }
        transform.position = handCurrentPos - handStartPos + objectStartPos;
	}

	void Release () {
		isMoving = false;
		Game.track.Unfreeze ();
	}
}
