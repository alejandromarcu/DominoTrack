﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    public GameObject placeCursor;
    public GameObject editCursor;

    public GameObject model;
    public GameObject startArrowModel;

    public GameObject avatar;
    public float minDistanceToSurfaceForPlacing = 0.1f;

    private enum Mode { Neutral, Place, Edit, JustPlaced, NotOnSurface, Inactive }; 
    private Mode mode = Mode.Neutral;

    private Renderer placeCursorRenderer;
    private Renderer editCursorRenderer;

    private GameObject editPiece = null;

    void Start()
    {
        placeCursorRenderer = placeCursor.GetComponentsInChildren<Renderer>()[0];
        editCursorRenderer = editCursor.GetComponentsInChildren<Renderer>()[0];
        Game.OnModeChanged += gameMode => SetMode(gameMode == Game.GameMode.Run ? Mode.Inactive : Mode.Neutral);
        GrabAndMove.OnGrabAndMoveStart += () => SetMode(Mode.Inactive);
        GrabAndMove.OnGrabAndMoveEnd += () => SetMode(Mode.Neutral);
    }

    /**
     * Return true if the mode was set to the specified mode, or false if the
     * transition was rejected
     **/
    bool SetMode(Mode mode)
    {
        if (this.mode == mode)
        {
            return true;
        }
        if ((this.mode == Mode.JustPlaced && mode == Mode.Edit) ||  
            (this.mode == Mode.Inactive && mode != Mode.Neutral))
        {
            return false;
        }
   
        this.mode = mode;

        editCursorRenderer.enabled = mode == Mode.Edit;
        placeCursorRenderer.enabled = mode == Mode.Place;
        return true;
    }
   
    public void StartEditing(GameObject obj)
    {
        if (SetMode(Mode.Edit))
        {
            editCursor.transform.position = obj.transform.position;
            editPiece = obj;
        }
    }

    public void StopEditing(GameObject obj)
    {
        if (mode == Mode.Edit)
        {
            SetMode(Mode.Neutral);
        }
    }

    void Update()
    {
        if (mode == Mode.Inactive) return;

        // TODO move and use confirmation
        if (Input.GetButtonDown("Fire3"))
        {
            SceneManager.LoadScene("Domino1");
        }

        SetPlaceCursorPosition();

        switch (mode)
        {
            case Mode.NotOnSurface:
            case Mode.JustPlaced:
                break;
            case Mode.Edit:
                HandleEditMode();
                break;
            default:
                SetMode(Mode.Place);
                HandlePlaceMode();
                break;
        }
    }

    private void SetPlaceCursorPosition()
    {
        var handPosition = avatar.transform.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));

        Ray ray = new Ray(handPosition, Vector3.down);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, minDistanceToSurfaceForPlacing, Layers.DOMINO_SURFACE))
        {
            SetMode(Mode.NotOnSurface);
            return;
        }
        if (mode == Mode.NotOnSurface)
        {
            SetMode(Mode.Place);
        }

        // We need to reposition the place cursor even in edit mode, so that the collider of the cursor may exit
        // and signal the end of edit mode.
        var dominoPosition = handPosition + Vector3.down * hit.distance;
        Quaternion handRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Quaternion dominoRotation = Quaternion.Euler(0f, handRotation.eulerAngles.y, 0f);

        placeCursor.transform.SetPositionAndRotation(dominoPosition, dominoRotation);
    }


    private void HandleEditMode()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            Game.track.Remove(Game.track.Find(editPiece));
            SetMode(Mode.Neutral);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            var domino = Game.track.Find(editPiece);
            domino.ToggleStart();
        }
    }

    private void HandlePlaceMode()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            var domino = new Domino(placeCursor.transform.position, placeCursor.transform.rotation.eulerAngles.y, model, startArrowModel);
            Game.track.Place(domino);
            SetMode(Mode.JustPlaced);
        }
    }
}
