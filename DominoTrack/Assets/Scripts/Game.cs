using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public enum GameMode { Build, Run };
    public delegate void GameModeChangedAction(GameMode mode);
    public static event GameModeChangedAction OnModeChanged;

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

    void Start()
    {
        track = new Track(transform.Find("Track").gameObject);
        instance = this;
        Mode = GameMode.Build;
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
    }
    public void Restart()
    {
        SceneManager.LoadScene("Domino1");
    }
}
