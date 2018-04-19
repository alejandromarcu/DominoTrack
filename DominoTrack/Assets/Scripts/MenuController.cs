using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class MenuController : MonoBehaviour
{
    public GameObject laser;
    public GameObject cameraObject;
    private GameObject mainMenu;
    private GameObject confirmNewTrackMenu;
    private GameObject confirmExitMenu;
    private GameObject colorMenu;
    private GameObject loadTrackMenu;
    private GameObject[] instructionsMenu = new GameObject[4];

    public float distanceToCamera;

    public bool showing { get; private set; }

    private enum State { None, Main, Color };
    private State state = State.None;

    private void Awake()
    {
        mainMenu = gameObject.transform.Find("MainMenu").gameObject;
        mainMenu.transform.localPosition = Vector3.zero;
        confirmNewTrackMenu = gameObject.transform.Find("ConfirmNewTrackMenu").gameObject;
        confirmNewTrackMenu.transform.localPosition = Vector3.zero;
        confirmExitMenu = gameObject.transform.Find("ConfirmExitMenu").gameObject;
        confirmExitMenu.transform.localPosition = Vector3.zero;
        colorMenu = gameObject.transform.Find("ColorMenu").gameObject;
        colorMenu.transform.localPosition = Vector3.zero;
        loadTrackMenu = gameObject.transform.Find("LoadTrackMenu").gameObject;
        loadTrackMenu.transform.localPosition = Vector3.zero;
        for (int i = 1; i <= instructionsMenu.Length; i++)
        {
            instructionsMenu[i - 1] = gameObject.transform.Find("Instructions" + i + "Menu").gameObject;
            instructionsMenu[i - 1].transform.localPosition = Vector3.zero;
        }
    }

    void Start()
    {
        CloseMenu();
    }

    public void ToggleMainMenu()
    {
        if (state == State.Main)
        {
            CloseMenu();
        } else
        {
            OpenMenu();
        }
    }

    public void ToggleColorMenu()
    {
        if (state == State.Color)
        {
            CloseMenu();
        }
        else
        {
            OpenColorMenu();
        }
    }

    private void OpenMenu()
    {
        Analytics.CustomEvent("openMenu");
        // Time.timeScale = 0;
        RepositionMenu();
        laser.SetActive(true);
        showing = true;
        state = State.Main;
        MainMenu();
    }

    private void MainMenu()
    {
        HideMenus();
        mainMenu.gameObject.SetActive(true);
    }

    private void OpenColorMenu()
    {
        Analytics.CustomEvent("openColorMenu");
        RepositionMenu();
        laser.SetActive(true);
        showing = true;
        state = State.Color;
        ColorMenu();
    }

    private void ColorMenu()
    {
        HideMenus();
        colorMenu.gameObject.SetActive(true);
    }

    public void LoadTrackMenu()
    {
        HideMenus();
        loadTrackMenu.gameObject.SetActive(true);
    }

    private void RepositionMenu()
    {
        var fwd = cameraObject.transform.forward;
        fwd.y = 0;
        fwd.Normalize();
        var pos = cameraObject.transform.position + fwd * distanceToCamera;

        transform.position = pos;
        transform.LookAt(cameraObject.transform, Vector3.up);
        transform.forward = -transform.forward; 
        // Move it up so it's less likely to have the board on the way
        var tmp = transform.position;
        tmp.y += 0.3f;
        transform.position = tmp;
    }

    public void Instructions(int page)
    {
        Analytics.CustomEvent("openInstructions" + page);
        HideMenus();
        instructionsMenu[page-1].SetActive(true);
    }

    public void NewTrackConfirmation()
    {
        HideMenus();
        confirmNewTrackMenu.SetActive(true);
    }

    public void ExitConfirmation()
    {
        HideMenus();
        confirmExitMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseMenu()
    {
        HideMenus();
        laser.SetActive(false);
        showing = false;
        state = State.None;
       // Time.timeScale = 1;
    }

    public void RestartTrack()
    {
        Game.instance.Restart();
    }

    private void HideMenus()
    {
        mainMenu.SetActive(false);
        confirmNewTrackMenu.SetActive(false);
        confirmExitMenu.SetActive(false);
        colorMenu.SetActive(false);
        loadTrackMenu.SetActive(false);
        for (int i = 1; i <= instructionsMenu.Length; i++)
        {
            instructionsMenu[i-1].SetActive(false);
        }
    }
}
