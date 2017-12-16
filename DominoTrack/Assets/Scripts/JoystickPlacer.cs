using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlacer : MonoBehaviour {

	public float threshold = 0.2f;

	private enum Direction { None, Up, Down, Left, Right};

	private double earliestTimeToPlaceNextJoystickPiece = 0;
	private Domino lastPiecePlacedWithJoystick = null;

	// TODO check for edit mode
	void Update() {
        if (!Game.isBuilding) return;

		if (Time.time < earliestTimeToPlaceNextJoystickPiece) {
			return;
		}

   
		// TODO prevent from removing pieces that were not placed with joystick.  I think I just need to place a flag in domino

		Vector2 control = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

		float max = threshold;
		Direction dir = Direction.None;

		if (control.y > max) {
			dir = Direction.Up;
			max = control.y;
		}
		if (control.x > max) {
			dir = Direction.Right;
			max = control.x;
		}
		if (-control.x > max) {
			dir = Direction.Left;
			max = -control.x;
		}
		if (-control.y > max) {
			dir = Direction.Down;
			max = -control.y;
		}

		switch (dir) {
		case Direction.Up: 
			lastPiecePlacedWithJoystick = Game.track.PlaceNext (0);
			break;
		case Direction.Left: 
			lastPiecePlacedWithJoystick = Game.track.PlaceNext (-15);
			break;
		case Direction.Right: 
			lastPiecePlacedWithJoystick = Game.track.PlaceNext (15);
			break;
		case Direction.Down: 
			if (Equals(lastPiecePlacedWithJoystick, Game.track.last)) {
				Game.track.Remove (Game.track.last);
			}
			lastPiecePlacedWithJoystick = Game.track.last;			
			break;
		case Direction.None:
			return;
		}

		earliestTimeToPlaceNextJoystickPiece = Time.time +  0.1 / (control.magnitude + 0.5);

	}


}
