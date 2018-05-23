using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadFileList : MonoBehaviour {
    public GameObject prefabFileButton;
    public int columns;
    private bool rendered = false;

    void OnDisable()
    {
        rendered = false;
    }

    void OnGUI()
    {
        // Just render once per activation.  I don't know why if I use OnEnable the first time it doesn't work well.
        if (rendered) return;
        rendered = true;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
  
        int fileStartIdx = Application.persistentDataPath.Length + 1;

        int i = 0;
        int colWidth = (int) GetComponent<RectTransform>().rect.width / columns;
        int rowHeight = (int) prefabFileButton.GetComponent<RectTransform>().rect.height;

        foreach (string file in files)
        {
            GameObject btn = Instantiate(prefabFileButton, gameObject.transform);

            int x = (int) ((i % columns + 0.5) * colWidth);
            int y = (int) ((i / columns + 0.5) * rowHeight * -1);

            btn.transform.localPosition = new Vector3(x, y, 0);
            btn.GetComponent<Button>().GetComponentInChildren<Text>().text = file.Substring(fileStartIdx, file.Length - fileStartIdx - 5);

            btn.GetComponent<Button>().onClick.AddListener(() => {
                SavedGame.Load(file);
                Game.instance.menuController.CloseMenu();
            });
            i++;
        }
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (files.Length+1) / columns * rowHeight);
    }
}
