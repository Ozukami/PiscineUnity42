using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuTextButton : MonoBehaviour {

    public Vector3 position;
    public float offset;
    public string text;
    public Font font;
    public int fontSize;
    public GameObject uiText;
    public Color[] instances;

    private GraphicRaycaster gr;

	void Start () {
        for (int i = 0; i < instances.Length; i++) {
            GameObject instance = Instantiate(uiText, transform.position, transform.rotation, transform);
            instance.GetComponent<Text>().text = text;
            instance.GetComponent<Text>().color = instances[i];
            instance.GetComponent<Text>().font = font;
            instance.GetComponent<Text>().fontSize = fontSize;
            instance.GetComponent<RectTransform>().localPosition = new Vector3(position.x + offset * i, position.y + offset * i);
        }
	}
	
	void Update () {
	}
}
