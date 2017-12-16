using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public enum GameMode { Build, Run };
    public delegate void GameModeChangedAction(GameMode mode);
    public static event GameModeChangedAction OnModeChanged;

    public static Track track {
		get;
		private set;
	}
    public static Game instance
    {
        get;
        private set;
    }
    private GameMode mode;
    public GameMode Mode {
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

    void Start () {
		track = new Track (transform.Find("Track").gameObject);
        instance = this;
        Mode = GameMode.Build;
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (mode == GameMode.Build)
            {
                track.KickOff();
                Mode = GameMode.Run;
            } else
            {
                track.Reset();
                Mode = GameMode.Build;
            }
        }
    }

}
