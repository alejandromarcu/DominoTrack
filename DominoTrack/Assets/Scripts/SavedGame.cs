using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

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
        public Color32 color;
        public string startMode;
    }

    [System.Serializable]
    public class SavedMagicBoard
    {
        public Vector3 position;
        public Quaternion rotation;
        public List<Vector3> tilePositions = new List<Vector3>();
    }

    public static void Save(string fileName)
    {
        SavedGame sg = Game.instance.Save(fileName);
        string json = JsonUtility.ToJson(sg, true);

        System.IO.File.WriteAllText(fileName, json);
    }

    public static void Load(string fileName)
    {
        string s = System.IO.File.ReadAllText(fileName);
        SavedGame sg = JsonUtility.FromJson<SavedGame>(s);
        Game.instance.LoadFrom(fileName, sg);
    }
}
