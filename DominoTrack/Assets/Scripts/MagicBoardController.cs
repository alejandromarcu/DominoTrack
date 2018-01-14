using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBoardController : MonoBehaviour
{

    public float distanceToBorder = 0.05f;
    public GameObject cameraObject;
    public float initialYPosition = 1.0f;

    private bool isCameraInitialized = false;

    void OnEnable()
    {
        Track.OnDominoPlaced += OnDominoPlaced;
    }

    void OnDisable()
    {
        Track.OnDominoPlaced -= OnDominoPlaced;
    }

    private void InitPositionInFrontOfCamera()
    {
        var tile = transform.Find("Tile");
        var fwd = cameraObject.transform.forward;
        fwd.y = 0;
        fwd.Normalize();

        var cameraAtBoardHeight = cameraObject.transform.position;
        cameraAtBoardHeight.y = initialYPosition;

        transform.position = cameraAtBoardHeight + fwd * tile.localScale.x * 0.75f;
        transform.LookAt(cameraAtBoardHeight, Vector3.up);
    }

    private void AddFirstPiece()
    {
        var tile = transform.Find("Tile");
        var pos = transform.TransformPoint(0f, tile.localScale.y / 2, 0f);
        Domino d = new Domino(pos, transform.rotation.eulerAngles.y + 180);
        Game.track.Place(d);
    }

    private void Update()
    {
        // The initial position of the HMD or camera is (0,0,0) until it's moved.  So, if I try
        // to initialize things that depend on the camera position on Start or another method, they'll
        // get the wrong position.  Instead, just wait until the camera has moved and the initialize.
        if (isCameraInitialized)
        {
            return;
        }
        if (cameraObject.transform.position.magnitude > 0.001f)
        {
            InitPositionInFrontOfCamera();
            AddFirstPiece();
            isCameraInitialized = true;
        }
    }

    void OnDominoPlaced(Domino d)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                EnsureHasTileBelow(d.position, (Vector3.forward * x + Vector3.left * y) * distanceToBorder);
            }
        }
    }
   
    private void EnsureHasTileBelow(Vector3 worldPos, Vector3 shift)
    {
        // Raise it up a bit, otherwise sometimes it misses the collider
        worldPos += Vector3.up * 0.01f;

        var pos = transform.InverseTransformPoint(worldPos);

        Vector3 testPos = pos + shift;
        Vector3 testPosWorld = transform.TransformPoint(testPos);
        if (Physics.Raycast(testPosWorld, Vector3.down, 0.05f))
        {
            // We already have a tile below.
            // TODO: make sure is a tile or something that we want to clone
            return;
        }

        // Find the current tile
        RaycastHit hit;
        Ray rayFindTile = new Ray(worldPos, Vector3.down);
        if (!Physics.Raycast(rayFindTile, out hit, 0.05f))
        {
            Debug.DrawLine(worldPos, worldPos + Vector3.down, Color.red, 20f);
            Debug.LogError("Couldn't find the tile below the piece");
            return;
        }

        // Figure out where to put the new tile
        int dx = 0;
        int dz = 0;

        GameObject tile = hit.collider.gameObject;
        Vector3 tilePos = transform.InverseTransformPoint(tile.transform.position);
        Vector3 tileSize = tile.transform.localScale;

        if (testPos.x > (tilePos.x + tileSize.x / 2.0))
        {
            dx = 1;
        }
        else if (testPos.x < (tilePos.x - tileSize.x / 2.0))
        {
            dx = -1;
        }
        if (testPos.z > tilePos.z + tileSize.z / 2)
        {
            dz = 1;
        }
        else if (testPos.z < tilePos.z - tileSize.z / 2)
        {
            dz = -1;
        }

        var newPos = tilePos + new Vector3(dx * tileSize.x, 0, dz * tileSize.z);
        var newPosWorld = transform.TransformPoint(newPos);
        Instantiate(tile, newPosWorld, tile.transform.rotation, tile.transform.parent);
    }

}
