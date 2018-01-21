﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public enum GameMode { Build, Run, Menu };
    public delegate void GameModeChangedAction(GameMode mode);
    public static event GameModeChangedAction OnModeChanged;
    public GameObject startArrowModel;
    public GameObject dominoModel;
    public MenuController menuController;

    public static Track track
    {
        get;
        private set;
    }
    public static Game instance
    {
        get;
        private set;
    }
    private GameMode mode;
    public GameMode Mode
    {
        get
        {
            return mode;
        }
        private set
        {
            mode = value;
            if (OnModeChanged != null)
                OnModeChanged(value);
        }
    }
    public static bool isBuilding { get { return instance.mode == GameMode.Build; } }
    public bool kickOffButtonOverloaded { get; set; }
    private GameMode modeBeforeMenu;

    void Start()
    {
        instance = this;
        Mode = GameMode.Build;
        track = new Track(transform.Find("Track").gameObject);
    }

    void Update()
    {
        if (mode == GameMode.Build)
        {
            if (!kickOffButtonOverloaded && OVRInput.GetDown(OVRInput.RawButton.A))
            {
                track.KickOff();
                Mode = GameMode.Run;
            }
        }
        else if (Mode == GameMode.Run)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.A) ||
                OVRInput.GetDown(OVRInput.RawButton.B) ||
                OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) ||
                OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
            {
                track.Reset();
                Mode = GameMode.Build;
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            if (menuController.showing)
            {
                menuController.CloseMenu();
                // Maybe it's better not to set this mode and allow the game to continue normally
                // when showing the menu, so that the user can follow the instructions.
                // Not deleting the code yet to make it easier to revert if I change my mind.
              //  Mode = modeBeforeMenu;
            } else
            {
              //  modeBeforeMenu = Mode;
              //  Mode = GameMode.Menu;
                menuController.OpenMenu();
            }
        }

    }
    public void Restart()
    {
        SceneManager.LoadScene("Domino1");
    }
}
