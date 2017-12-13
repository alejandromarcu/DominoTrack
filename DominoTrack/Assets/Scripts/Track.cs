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
		var newPiece = new Domino (pos + last.forward * 0.025f, rotation, last.model, last.startArrowModel); // TODO constant
		Place (newPiece);
		return newPiece;
	}

	public void Reset() {
		foreach (var domino in track) {
			domino.ResetGameObject ();
		}
		Freeze ();
	}

	public void Freeze() {
		/*
		//track.ForEach (domino => domino.Freeze ());
		foreach (var domino in track) {
			domino.Freeze ();
		}
		*/
	}

	public void Unfreeze() {
		/*
		//track.ForEach (domino => domino.Unfreeze ());
		foreach (var domino in track) {
			domino.Unfreeze ();
		}
		*/
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
