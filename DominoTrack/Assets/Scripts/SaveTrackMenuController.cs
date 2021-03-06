﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveTrackMenuController : MonoBehaviour {

    public Text fileNameText;

    public void OnEnable()
    {
        if (Game.instance != null && Game.instance.fileName != null)
        {
            fileNameText.text = Game.instance.fileName;
        } else
        {
            fileNameText.text = "";
        }
    }

    public void Save()
    {
        if (fileNameText.text.Length == 0) return;
        SavedGame.Save(Application.persistentDataPath + "/" + fileNameText.text + ".json");
        Game.instance.menuController.CloseMenu();
    }
}
