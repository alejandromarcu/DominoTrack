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
    private GameObject[] instructionsMenu = new GameObject[4];

    public float distanceToCamera;

    public bool showing { get; private set; }

    private void Awake()
    {
        mainMenu = gameObject.transform.Find("MainMenu").gameObject;
        mainMenu.transform.localPosition = Vector3.zero;
        confirmNewTrackMenu = gameObject.transform.Find("ConfirmNewTrackMenu").gameObject;
        confirmNewTrackMenu.transform.localPosition = Vector3.zero;
        confirmExitMenu = gameObject.transform.Find("ConfirmExitMenu").gameObject;
        confirmExitMenu.transform.localPosition = Vector3.zero;

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

    public void OpenMenu()
    {
        Analytics.CustomEvent("openMenu");
        // Time.timeScale = 0;
        RepositionMenu();
        laser.SetActive(true);
        showing = true;
        MainMenu();
    }

    public void MainMenu()
    {
        HideMenus();
        mainMenu.gameObject.SetActive(true);
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
        for (int i = 1; i <= instructionsMenu.Length; i++)
        {
            instructionsMenu[i-1].SetActive(false);
        }
    }
}
