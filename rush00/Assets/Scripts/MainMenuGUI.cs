using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuGUI : MonoBehaviour {

    public string levelName;

    private GraphicRaycaster gr;

	void Start () {
        gr = this.GetComponent<GraphicRaycaster>(); 
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach (RaycastResult elem in results)
            {
                if (elem.gameObject.name == "Zone" && elem.gameObject.transform.parent.name == "TextButtonStart")
                    SceneManager.LoadScene(levelName);
                else if (elem.gameObject.name == "Zone" && elem.gameObject.transform.parent.name == "TextButtonExit")
                    Application.Quit();
                else if (elem.gameObject.name == "Zone" && elem.gameObject.transform.parent.name == "TextButtonRestart")
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                else if (elem.gameObject.name == "Zone" && elem.gameObject.transform.parent.name == "TextButtonBackToMenu")
                    SceneManager.LoadScene("MainMenu");
            }
        }
	}
}
