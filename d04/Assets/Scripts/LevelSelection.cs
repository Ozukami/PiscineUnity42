using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

	public Image selectionBox;
	public Transform[] levels;
	public Text lives, rings, bestScore, levelName;

	private int selected = 0;
	private int[] bestScores = new int[12];

	// Use this for initialization
	void Start () {
		foreach (var level in levels) {
			if (level.name != "0" && level.name != "4" && level.name != "8") {
				if (PlayerPrefs.GetInt("level_" + level.name) == 1) {
					level.Find("Disable").gameObject.SetActive(false);
				}
			}
		}
		if (PlayerPrefs.GetInt("lives") != null) {
			lives.text = "" + PlayerPrefs.GetInt("lives");
		}
		if (PlayerPrefs.GetInt("rings") != null) {
			rings.text = "" + PlayerPrefs.GetInt("rings");
		}
		for (int i=0; i<12; i++) {
			if (PlayerPrefs.GetInt("bestScoreLevel" + i) != null) {
				// bestScore.text = "BestScore : " + PlayerPrefs.GetInt("bestScore") + " Pts";
				bestScores[i] = PlayerPrefs.GetInt("bestScoreLevel" + i);
			} else { bestScores[i] = 0; }
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			if (selected - 4 < 0) { selected = selected + 12 - 4;}
			else { selected -= 4; }
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			selected = (selected + 4) % 12;
		} else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			if (selected - 1 < 0) { selected = selected + 12 - 1;}
			else { selected -= 1; }
		} else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			selected = (selected + 1) % 12;
		} else if (Input.GetKeyDown(KeyCode.Return)) {
			if (levels[selected].Find("Disable") != null && levels[selected].Find("Disable").gameObject.activeSelf) {
				Debug.Log("Not Available");
			}
			if (selected == 4) {
				SceneManager.LoadScene(2);
			} else {
				Debug.Log("Available, but no scene found");
			}
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			PlayerPrefs.SetInt("level_1", 1);
			PlayerPrefs.SetInt("level_4", 1);
			PlayerPrefs.SetInt("level_5", 1);
			PlayerPrefs.SetInt("level_9", 1);
			PlayerPrefs.SetInt("level_10", 1);
			PlayerPrefs.SetInt("lives", 42);
			PlayerPrefs.SetInt("rings", 4269);
			PlayerPrefs.SetInt("bestScoreLevel0", 42);
			PlayerPrefs.SetInt("bestScoreLevel4", 69);
			PlayerPrefs.SetInt("bestScoreLevel8", 4269);
		} else if (Input.GetKeyDown(KeyCode.R)) {
			PlayerPrefs.DeleteAll();
		}
		selectionBox.transform.SetParent(levels[selected]);
		selectionBox.transform.localPosition = Vector3.zero;
		levelName.text = levels[selected].GetComponentInChildren<Text>().text;
		bestScore.text = "BestScore : " + bestScores[selected] + " Pts";
	}
}
