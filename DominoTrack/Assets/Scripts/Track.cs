using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track
{
    public delegate void PlaceDominoAction(Domino domino);
    public static event PlaceDominoAction OnDominoPlaced;

    private List<Domino> track = new List<Domino>();
    public Domino last
    {
        get
        {
            if (track.Count == 0)
            {
                return null;
            }
            return track[track.Count - 1];
        }
    }
    public bool isEmpty { get { return track.Count == 0; } }
    public GameObject trackGameObject { private set; get; }

    public Track(GameObject track)
    {
        trackGameObject = track;
    }

    public void Place(Domino d)
    {
        track.Add(d);
        d.ResetGameObject();
        if (track.Count == 1)
        {
            d.ToggleStart();
        }
        d.Freeze();
        OnDominoPlaced(d);
    }

    public Domino PlaceNext(float angle)
    {
        // TODO use raytracing for y, since the surface height could change
        if (last == null)
        {
            return null;
        }
        var pos = last.position;
        var rotation = last.rotationY + angle;

        float d = FindNextAvailablePosition(pos, last.forward, angle);

        var newPiece = new Domino(pos + last.forward * d, rotation, last.model, last.startArrowModel);
        Place(newPiece);
        return newPiece;
    }

    private float FindNextAvailablePosition(Vector3 position, Vector3 forward, float angle)
    {
        // TODO constants
        float DEFAULT_STEP = 0.025f;
        Vector3 SIZE = new Vector3(0.028f, 0.05f, 0.004f);

        // How much space it's needed behind and below the piece to consider the space free
        float MARGIN = DEFAULT_STEP * 0.3f;

        float angle_rad = angle * Mathf.PI / 180f;
        float projected_z = Mathf.Abs((Mathf.Sin(angle_rad) * SIZE.x + Mathf.Cos(angle_rad) * SIZE.z) / 2.0f);

        // The position is on the bottom, move it up to the center
        var center = position + Vector3.up * SIZE.y / 2f;

        var min_distance = SIZE.z;

        if (CheckFreeSpace(center, forward, min_distance, DEFAULT_STEP + MARGIN))
        {
            return DEFAULT_STEP;
        }
        if (CheckFreeSpace(center, forward, min_distance, DEFAULT_STEP / 2f + MARGIN))
        {
            return DEFAULT_STEP / 2f;
        }

        float step = DEFAULT_STEP * 1.5f;
        while (!CheckFreeSpace(center, forward, step - MARGIN, step + MARGIN))
        {
            step += 0.5f * DEFAULT_STEP;
        }
        return step;
    }

    private bool CheckFreeSpace(Vector3 position, Vector3 forward, float min, float max)
    {
        var center = position + forward * (min + max) / 2f;
        var half_size = new Vector3(0.035f, 0.04f, max - min) / 2f; // TODO constant with size, a bit wider but less tall to avoid collisions with the floor

        return !Physics.CheckBox(center, half_size, Quaternion.LookRotation(forward));
    }


    public void Reset()
    {
        foreach (var domino in track)
        {
            domino.ResetGameObject();
        }
        Freeze();
    }

    public void Freeze()
    {
        track.ForEach(domino => domino.Freeze());
    }

    public void Unfreeze()
    {
        track.ForEach(domino => domino.Unfreeze());
    }

    public Domino Find(GameObject obj)
    {
        foreach (var domino in track)
        {
            if (domino.MatchesGameObject(obj))
            {
                return domino;
            }
        }
        return null;
    }

    public void Remove(Domino toRemove)
    {
        toRemove.DestroyGameObject();
        track.Remove(toRemove);
    }

    public void KickOff()
    {
        Unfreeze();
        track.ForEach(d => d.KickOffIfNeeded());
    }
}
