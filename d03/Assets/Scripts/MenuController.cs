using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

	void Start () {
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}

	public void StartGame () {
		SceneManager.LoadScene(1);
	}

	public void ExitGame () {
		Application.Quit();
	}

	public void ResumeGame () {
		gameManager.gm.pause(false);
		GUI.gui.HidePauseCanvas();
	}

	public void RageQuitGame () {
		GUI.gui.ConfirmExit();
	}

	public void HomeGame () {
		SceneManager.LoadScene(0);
	}

	public void NextLevel () {
		SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
	}

	public void RetryLevel () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
