using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlacer : MonoBehaviour {

	public float threshold = 0.2f;

	private enum Direction { None, Up, Down, Left, Right};

	private double earliestTimeToPlaceNextJoystickPiece = 0;

    private bool justDeleted = false;

	// TODO check for edit mode
	void Update() {
        if (!Game.isBuilding) return;

		if (Time.time < earliestTimeToPlaceNextJoystickPiece) {
			return;
		}

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

        switch (dir)
        {
            case Direction.Up:
                Game.track.PlaceNext(0);
                break;
            case Direction.Left:
                Game.track.PlaceNext(-15);
                break;
            case Direction.Right:
                Game.track.PlaceNext(15);
                break;
            case Direction.Down:
                // The throttiling when deleting is one piece each time you move down
                if (justDeleted || Game.track.isEmpty) return;
                Game.track.Remove(Game.track.last);
                justDeleted = true;
                return;
            case Direction.None:
                justDeleted = false;
                return;
        }
        earliestTimeToPlaceNextJoystickPiece = Time.time +  0.1 / (control.magnitude + 0.5);
	}


}
