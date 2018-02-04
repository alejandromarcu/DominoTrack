using System.Collections.Generic;
using UnityEngine;

public class MagicBoardController : MonoBehaviour
{

    public float distanceToBorder = 0.05f;
    public GameObject cameraObject;
    public float initialYPosition = 1.0f;

    private bool isCameraInitialized = false;
    private List<GameObject> tiles = new List<GameObject>(); 

    void OnEnable()
    {
        Track.OnDominoPlaced += OnDominoPlaced;
    }

    void OnDisable()
    {
        Track.OnDominoPlaced -= OnDominoPlaced;
    }

    private void Start()
    {
        // It's active by default just to make it easier in the editor
        transform.Find("Tile").gameObject.SetActive(false);
    }

    private void InitPositionInFrontOfCamera()
    {
        var tile = tiles[0].transform;
        var fwd = cameraObject.transform.forward;
        fwd.y = 0;
        fwd.Normalize();

        var cameraAtBoardHeight = cameraObject.transform.position;
        cameraAtBoardHeight.y = initialYPosition;

        transform.position = cameraAtBoardHeight + fwd * tile.localScale.x * 0.75f;
        transform.LookAt(cameraAtBoardHeight, Vector3.up);
    }

    private void AddTile(Vector3 localPosition)
    {
        var tile = transform.Find("Tile").gameObject;
        var position = transform.TransformPoint(localPosition);
        var newTile = Instantiate(tile, position, tile.transform.rotation, transform);
        newTile.SetActive(true);
        tiles.Add(newTile);
    }

    private void AddFirstTile()
    {
        AddTile(Vector3.zero);
    }

    private void AddFirstPiece()
    {
        var tile = tiles[0].transform;
        var pos = transform.TransformPoint(0f, tile.localScale.y / 2, 0f);
        Domino d = new Domino(pos, transform.rotation.eulerAngles.y + 180, Space.World);
        Game.track.Place(d);
    }

    private void Init()
    {
        if (Game.instance.Mode != Game.GameMode.Load)
        {
            AddFirstTile();
            InitPositionInFrontOfCamera();
            AddFirstPiece();
        }
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
            Init();
            isCameraInitialized = true;
        }
    }

    void OnDominoPlaced(Domino d)
    {
        if (Game.instance.Mode == Game.GameMode.Load)
        {
            // Don't run during load, since the tiles will be loaded as well.
            return;
        }

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
        AddTile(newPos);
    }

    public void Restart()
    {
        Game.track.Restart();
        tiles.ForEach(tile => Destroy(tile));
        tiles.Clear();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Init();
    }

    public SavedGame.SavedMagicBoard Save()
    {
        var smb = new SavedGame.SavedMagicBoard();
        smb.position = transform.position;
        smb.rotation = transform.rotation;
        tiles.ForEach(tile => smb.tilePositions.Add(tile.transform.localPosition));
        return smb;
    }

    public void LoadFrom(SavedGame.SavedMagicBoard smb)
    {
        transform.position = smb.position;
        transform.rotation = smb.rotation;
        smb.tilePositions.ForEach(pos => AddTile(pos));
    }
}
