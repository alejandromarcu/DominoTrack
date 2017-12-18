using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track {
	public delegate void PlaceDominoAction(Domino domino);
	public static event PlaceDominoAction OnDominoPlaced;

	private List<Domino> track = new List<Domino>();
	public Domino last {
		get {
			if (track.Count == 0) {
				return null;
			}
			return track[track.Count - 1];
		}
	}
    public bool isEmpty {  get { return track.Count == 0; } }
	public GameObject trackGameObject { private set; get; }

	public Track(GameObject track) {
		trackGameObject = track;
	}

	public void Place(Domino d) {
		track.Add(d);
		d.ResetGameObject ();
		if (track.Count == 1) {
			d.ToggleStart ();
		}
		d.Freeze ();
		OnDominoPlaced (d);
	}
		
	public Domino PlaceNext(float angle) {
		// TODO use raytracing for y, since the surface height could change
		if (last == null) {
			return null;
		}
		var pos = last.position;
		var rotation = last.rotationY + angle;
        // TODO check for collisions
        float d = FindNextAvailablePosition(pos, last.forward, angle);
        Debug.Log("FindNextAvailablePosition: " + d);

        //Debug.Log(last.forward);
       // Debug.Log(last.gameObject.transform.rotation + " QQQQ " + Quaternion.LookRotation(last.forward));
        var newPiece = new Domino (pos + last.forward * d, rotation, last.model, last.startArrowModel); 
		Place (newPiece);
		return newPiece;
	}

    private float FindNextAvailablePosition(Vector3 position, Vector3 forward, float angle)
    {
        // TODO constants
        float DEFAULT_STEP = 0.025f;
        Vector3 SIZE = new Vector3(0.028f, 0.05f, 0.004f);

        float angle_rad = angle * Mathf.PI / 180f;
        float projected_z = Mathf.Abs((Mathf.Sin(angle_rad) * SIZE.x + Mathf.Cos(angle_rad) * SIZE.z) / 2.0f);
        Debug.Log("Proj z " + projected_z);
        var center = position + Vector3.up * SIZE.y / 1.5f; // TODO 2?

        if (CheckFreeSpace(center, forward, SIZE.z / 2f + 0.002f, DEFAULT_STEP + projected_z))
        {
            Debug.Log("FULL STEP");
            return DEFAULT_STEP;
        }
        if (CheckFreeSpace(center, forward, SIZE.z / 2f + 0.002f, DEFAULT_STEP / 2f + projected_z))
        {
            Debug.Log("HALF STEP");

            return DEFAULT_STEP / 2f;
        }


        float step = DEFAULT_STEP * 1.5f;
        var delta = DEFAULT_STEP / 4f;
        while (!CheckFreeSpace(center, forward, step - delta, step + delta))
        {
            step += 0.5f * DEFAULT_STEP;
        }
        Debug.Log("Step: " + step);

        return step;
    }

 

    private bool CheckFreeSpace(Vector3 position, Vector3 forward, float min, float max)
    {
   //     Vector3 fwd = Quaternion.Euler(0f, rotationY, 0f) * Vector3.forward;
    //    Debug.Log("roty: " + rotationY + "  fwd: " + fwd);
        var center = position + forward * (min + max) / 2f;
        var half_size = new Vector3(0.03f, 0.04f, max - min) / 2f; // TODO constant with size, a bit wider but less tall to avoid collisions with the floor
        Debug.Log("Center, size: " + center + "   " + half_size*100);
        if (!Physics.CheckBox(center, half_size, last.gameObject.transform.rotation))
        {
            /*
            var q = Vector3.up * 0.2f;
            Debug.DrawLine(position + forward * min + q, position + forward * max + q, Color.red, 120f, true);
            Debug.DrawLine(center - Vector3.up * 0.02f, center + Vector3.up * 0.02f, Color.yellow, 120f, true);
            Debug.DrawLine(position + forward * min - Vector3.up * 0.02f, position + forward * min + Vector3.up * 0.02f, Color.blue, 120f, true);
            Debug.DrawLine(position + forward * max - Vector3.up * 0.02f, position + forward * max + Vector3.up * 0.02f, Color.green, 120f, true);
            */
            drawbox(center, half_size, last.gameObject.transform.rotation, Color.red);
            return true;
        }
        else
        {
            var q = Vector3.up * 0.2f;
            drawbox(center, half_size, last.gameObject.transform.rotation, Color.black);

//            Debug.DrawLine(position + forward * min + q, position + forward * max + q, Color.black, 120f, true);
            return false;
        }
        
        //            return !Physics.CheckBox(center, half_size, last.gameObject.transform.rotation);

    //    return !Physics.CheckBox(center, half_size, Quaternion.LookRotation(forward));
    }


	public void Reset() {
		foreach (var domino in track) {
			domino.ResetGameObject ();
		}
		Freeze ();
	}

	public void Freeze() {
		track.ForEach (domino => domino.Freeze ());
	}

	public void Unfreeze() {
		track.ForEach (domino => domino.Unfreeze ());
	}

	public Domino Find(GameObject obj) {
		foreach (var domino in track) {
			if (domino.MatchesGameObject(obj)) {
				return domino;
			}
		}
		return null;
	}

	public void Remove(Domino toRemove) {
		toRemove.DestroyGameObject ();
		track.Remove (toRemove);
	}

	public void KickOff() {
		Unfreeze ();
		track.ForEach(d => d.KickOffIfNeeded());
	}
}
