using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoardController : MonoBehaviour {

	public float distanceToBorder = 0.05f;

	void OnEnable () {
		Track.OnDominoPlaced += OnDominoPlaced;
	}

    void OnDisable()
    {
        Track.OnDominoPlaced += OnDominoPlaced;
    }

    void OnDominoPlaced(Domino d) {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                EnsureHasTileBelow(d.position, (Vector3.forward * x + Vector3.left * y) * distanceToBorder);
            }
        }
	}

	private void EnsureHasTileBelow(Vector3 pos, Vector3 shift) {
        // Raise it up a bit, otherwise sometimes it misses the collider
        pos += Vector3.up * 0.01f;

		Vector3 testPos = pos + shift;
		if (Physics.Raycast (testPos, Vector3.down, 0.05f)) {
			// We already have a tile below.
			// TODO: make sure is a tile or something that we want to clone
			return;	
		}
        
		// Find the current tile
		RaycastHit hit;
		Ray rayFindTile = new Ray (pos, Vector3.down);
		if (!Physics.Raycast (rayFindTile, out hit, 0.05f)) {
			Debug.DrawLine (pos, pos + Vector3.down, Color.red, 20f);
			Debug.LogError ("Couldn't find the tile below the piece");
			return;
		}

		// Figure out where to put the new tile
		int dx = 0;
		int dz = 0;

		GameObject tile = hit.collider.gameObject;
		Vector3 tilePos = tile.transform.position;
		Vector3 tileSize = tile.transform.localScale;

		if (testPos.x > (tilePos.x + tileSize.x / 2.0)) {
			dx = 1;
		} else if (testPos.x < (tilePos.x - tileSize.x / 2.0)) {
			dx = -1;
		}
		if (testPos.z > tilePos.z + tileSize.z / 2) {
			dz = 1;
		} else if (testPos.z < tilePos.z - tileSize.z / 2) {
			dz = -1;
		}

		var newPos = tilePos + new Vector3 (dx * tileSize.x, 0, dz * tileSize.z);
		Instantiate (tile, newPos, tile.transform.rotation, tile.transform.parent);
	}

}
