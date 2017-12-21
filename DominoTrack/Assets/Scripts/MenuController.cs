using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject laser;
    public GameObject cameraObject;
    public float distanceToCamera;

    private bool showing;

	// Use this for initialization
	void Start () {
        ExitMenu();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            if (showing)
            {
                ExitMenu();
            } else
            {
                MainMenu();
            }
        }
	}

    public void MainMenu()
    {
        RepositionCamera();
        gameObject.transform.Find("MainMenu").transform.gameObject.SetActive(true);
        laser.SetActive(true);
        showing = true;
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

    public void ExitMenu()
    {
        gameObject.transform.Find("MainMenu").transform.gameObject.SetActive(false);
        laser.SetActive(false);
        showing = false;
    }

    // TODO, maybe it should be in a class specific for this menu
    public void RestartTrack()
    {
        Game.instance.Restart();
    }
}
