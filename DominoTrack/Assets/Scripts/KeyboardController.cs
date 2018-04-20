using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyboardController : MonoBehaviour {

    public Text textField;
    public GameObject keyPrefab;
    public int maxLength;

	void Start () {
        PlaceRow("1234567890", -7, 1.3f);
        PlaceRow("QWERTYUIOP", -7.5F, 0.2f);
        PlaceRow("ASDFGHJKL<", -7, -0.9f);
        PlaceRow("ZXCVBNM ", -6.5f, -2f);
    }

    private void PlaceRow(string keys, float x, float y)
    {
        int i = 0;
        foreach (char key in keys)
        {
            GameObject btn = Instantiate(keyPrefab, gameObject.transform);
            btn.transform.localPosition = new Vector3(x + i * 1.2f, y, 0f);
            string keyStr = key.ToString().ToLower();
            string label =  key.ToString();
            if (key == ' ')
            {
                label = "[space]";
                btn.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
            }
            if (key == '<')
            {
                label = "Back";
                btn.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150f);
            }
            btn.GetComponent<Button>().GetComponentInChildren<Text>().text = label;
            btn.GetComponent<Button>().onClick.AddListener(() => {
                OnKeyPressed(keyStr);
            });
            i++;
        }
    }


    public void OnKeyPressed(string key)
    {
        if (key == "<")
        {
            if (textField.text.Length > 0) textField.text = textField.text.Substring(0, textField.text.Length - 1);
        } else if (textField.text.Length < maxLength)
        {
            textField.text += key;
        }
    }
}
