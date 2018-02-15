using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenuController : MonoBehaviour {

    public GameObject prefabColorButton;
    public RectTransform parentPanel;
    public static Color32[] colors = new Color32[16]
    {
        new Color32(255, 250, 230, 255),
        new Color32(0, 0, 0, 255),
        new Color32(157, 157, 157, 255),
        new Color32(190, 38, 51, 255),
        new Color32(224, 111, 139, 255),
        new Color32(73, 60, 43, 255),
        new Color32(164, 100, 34, 255),
        new Color32(235, 137, 49, 255),
        new Color32(247, 226, 107, 255),
        new Color32(47, 72, 78, 255),
        new Color32(68, 137, 26, 255),
        new Color32(163, 206, 39, 255),
        new Color32(27, 38, 50, 255),
        new Color32(0, 87, 132, 255),
        new Color32(49, 162, 242, 255),
        new Color32(178, 220, 239, 255)
    };


    void Start()
    {
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                GameObject btn = Instantiate(prefabColorButton, parentPanel);
                btn.transform.localPosition = new Vector3(x * 5 - 7.5f, y * -3 + 5.25f, 0);

                Color32 color = colors[x + y * 4];
                ColorBlock colorBlock = new ColorBlock();
                colorBlock.normalColor = color;
                colorBlock.highlightedColor = color;
                colorBlock.pressedColor = color;
                colorBlock.colorMultiplier = 1f;
                btn.GetComponent<Button>().colors = colorBlock;

                btn.GetComponent<Button>().onClick.AddListener(() => {
                    Debug.Log("Setting color to  " + color);
                    Game.track.currentDominoColor = color;
                    Game.instance.menuController.CloseMenu();
                });
            }
        }
    }
}
