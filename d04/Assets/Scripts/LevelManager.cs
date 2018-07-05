using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public int currentLevel;

	public void Replay() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void NextLevel() {
		
	}

	public void Home() {
		SceneManager.LoadScene(1);
	}

	public void SaveData(int score, int rings) {
		if (PlayerPrefs.GetInt("level_" + currentLevel) == null)
			PlayerPrefs.SetInt("level_" + currentLevel, 1);
		if (PlayerPrefs.GetInt("bestScoreLevel" + currentLevel) == null)
			PlayerPrefs.SetInt("bestScoreLevel" + currentLevel, score);
		else if (PlayerPrefs.GetInt("bestScoreLevel" + currentLevel) < score)
			PlayerPrefs.SetInt("bestScoreLevel" + currentLevel, score);
		if (PlayerPrefs.GetInt("rings") == null)
			PlayerPrefs.SetInt("rings", rings);
		else
			PlayerPrefs.SetInt("rings", PlayerPrefs.GetInt("rings") + rings);
	}
}
