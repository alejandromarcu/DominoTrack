using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class GrabAndMove : MonoBehaviour
{
    public GameObject avatar;

    public delegate void GrabAndMoveStart();
    public static event GrabAndMoveStart OnGrabAndMoveStart;
    public delegate void GrabAndMoveEnd();
    public static event GrabAndMoveEnd OnGrabAndMoveEnd;

    private bool isMoving = false;

    private float prevRotationY;
    private Vector3 prevPos;
    private float minY;

    private void OnEnable()
    {
        Game.OnModeChanged += OnGameModeChanged;
    }

    private void OnDisable()
    {
        Game.OnModeChanged -= OnGameModeChanged;
    }

    private void OnGameModeChanged(Game.GameMode mode)
    {
        if (mode == Game.GameMode.Run)
        {
            if (isMoving) Release();
        }
    }

    void Update()
    {
        if (!Game.isBuilding) return;

        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            Grab();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            Release();
        }
        if (isMoving)
        {
            Move();
        }
    }

   
    void Grab()
    {
        Game.track.Freeze();
        isMoving = true;
        GetPositionAndRotationY(out prevPos, out prevRotationY);
        ComputeMinY();

        if (OnGrabAndMoveStart != null)
        {
            OnGrabAndMoveStart();
        }
    }

    private void ComputeMinY()
    {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        var lowestPoint = renderers.Min(r => r.bounds.min.y);
        minY = transform.position.y - lowestPoint;
    }

    void Move()
    {
        Vector3 currentPos;
        float currentRotationY;
        GetPositionAndRotationY(out currentPos, out currentRotationY);

        var deltaPos = currentPos - prevPos;

        // Don't allow to move below the floor
        if (transform.position.y + deltaPos.y < minY)
        {
            deltaPos.y = 0;
        }

        transform.RotateAround(currentPos, Vector3.up, currentRotationY - prevRotationY);
        transform.Translate(deltaPos, Space.World);

        prevPos = currentPos; 
        prevRotationY = currentRotationY;
    }

    void GetPositionAndRotationY(out Vector3 pos, out float rot)
    {
        pos = avatar.transform.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
        rot = (avatar.transform.rotation * OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch)).eulerAngles.y;
    }

    void Release()
    {
        isMoving = false;
        Game.track.Unfreeze();
        if (OnGrabAndMoveEnd != null)
        {
            OnGrabAndMoveEnd();
        }
        Analytics.CustomEvent("grabAndMove");
    }
}
