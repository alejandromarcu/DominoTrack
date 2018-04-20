using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System.IO;

public class Game : MonoBehaviour
{
    public enum GameMode { Build, Run, Menu, Load };
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
    private MagicBoardController magicBoard;
    public string fileName { get; private set; }

    void Start()
    {
        instance = this;
        Mode = GameMode.Build;
        track = new Track(transform.Find("Track").gameObject);
        magicBoard = FindObjectOfType<MagicBoardController>();
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
            menuController.ToggleMainMenu();
            /*
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
            */
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            menuController.ToggleColorMenu();
        }

    }
    public void Restart()
    {
        Analytics.CustomEvent("restart", new Dictionary<string, object> { { "pieces", track.count } });
        if (mode != GameMode.Load)
        {
            mode = GameMode.Build;
        }
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        if (menuController.showing)
        {
            menuController.CloseMenu();
        }
        magicBoard.Restart();
        this.fileName = null;
    }

   
    public SavedGame Save(string fileName)
    {
        this.fileName = Path.GetFileNameWithoutExtension(fileName);
        var sg = new SavedGame();
        sg.version = 1;
        sg.position = gameObject.transform.position;
        sg.rotation = gameObject.transform.rotation;
        sg.track = track.Save();
        sg.magicBoard = magicBoard.Save();
        return sg;
    }

    public void LoadFrom(string fileName, SavedGame sg)
    {
        Mode = GameMode.Load;
        Restart();
        gameObject.transform.position = sg.position;
        gameObject.transform.rotation = sg.rotation;
        track.LoadFrom(sg.track);
        magicBoard.LoadFrom(sg.magicBoard);
        Mode = GameMode.Build;
        this.fileName = Path.GetFileNameWithoutExtension(fileName);
    }

    void OnApplicationQuit()
    {
        Analytics.CustomEvent("quit", new Dictionary<string, object> {
            { "pieces", track.count },
            { "time", Time.time },
            { "path", Application.persistentDataPath }
        });

        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
