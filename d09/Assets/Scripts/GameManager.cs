using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager gM = null;

	public Text timeText, endText;
	
	public int enemies = 0;
	public int wave = 0;
	public float waveTime;
	public float pauseTime;

	public bool inPause = false;
	private float _time = 0;

	void Awake () {
		if (gM == null)
			gM = this;
		else if (gM != this)
			Destroy(gameObject);
	}

	void Update () {
		_time += Time.deltaTime;
		if (!inPause)
			timeText.text = "(Wave " + wave + ") Time: " + (waveTime - Mathf.RoundToInt(_time)) + "s";
		else
			timeText.text = "(Pause) Time: " + (pauseTime - Mathf.RoundToInt(_time)) + "s";
		if (_time >= waveTime && !inPause) {
			wave++;
			inPause = true;
			_time = 0;
		} else if (_time >= pauseTime && inPause) {
			inPause = false;
			_time = 0;
		}
	}

	public void Loose () {
		endText.gameObject.SetActive(true);
		endText.text = "Game Over\nYou survived " + wave + " waves !";
	}	
}
