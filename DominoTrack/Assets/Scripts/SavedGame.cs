using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedGame {
    public int version;
    public Vector3 position;
    public Quaternion rotation;
    public SavedTrack track;
    public SavedMagicBoard magicBoard;

    [System.Serializable]
    public class SavedTrack
    {
        public List<SavedDomino> track = new List<SavedDomino>();
    }

    [System.Serializable]
    public class SavedDomino
    {
        public Vector3 position;
        public float rotationY;
        public Color color;
    }

    [System.Serializable]
    public class SavedMagicBoard
    {
        public Vector3 position;
        public Quaternion rotation;
        public List<Vector3> tilePositions = new List<Vector3>();
    }

    public static void Save()
    {
        SavedGame sg = Game.instance.Save();
        string json = JsonUtility.ToJson(sg, true);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/qq.json", json);
        Debug.Log("Saved track in " + Application.persistentDataPath);
    }

    public static void Load()
    {
        string s = System.IO.File.ReadAllText(Application.persistentDataPath + "/qq.json");
        SavedGame sg = JsonUtility.FromJson<SavedGame>(s);
        Game.instance.LoadFrom(sg);
    }
}
