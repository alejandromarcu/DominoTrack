using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject laser;
    public GameObject cameraObject;
    private GameObject mainMenu;
    private GameObject confirmNewTrackMenu;
    private GameObject confirmExitMenu;


    public float distanceToCamera;

    private bool showing;

    private void Awake()
    {
        mainMenu = gameObject.transform.Find("MainMenu").gameObject;
        mainMenu.transform.localPosition = Vector3.zero;
        confirmNewTrackMenu = gameObject.transform.Find("ConfirmNewTrackMenu").gameObject;
        confirmNewTrackMenu.transform.localPosition = Vector3.zero;
        confirmExitMenu = gameObject.transform.Find("ConfirmExitMenu").gameObject;
        confirmExitMenu.transform.localPosition = Vector3.zero;
    }

    void Start()
    {
        CloseMenu();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            if (showing)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        RepositionCamera();
        laser.SetActive(true);
        showing = true;
        MainMenu();
    }

    public void MainMenu()
    {
        HideMenus();
        mainMenu.gameObject.SetActive(true);
    }

    private void RepositionCamera()
    {
        var fwd = cameraObject.transform.forward;
        fwd.y = 0;
        fwd.Normalize();
        var pos = cameraObject.transform.position + fwd * distanceToCamera;
        
        transform.position = pos;
        transform.LookAt(cameraObject.transform, Vector3.up);
        transform.forward = -transform.forward;
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
    }
}
