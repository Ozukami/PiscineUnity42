using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public Canvas statsUI;
	public GameObject player;

	public void Retry () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void RageQuit () {
		Application.Quit();
	}

	public void CloseStats () {
		statsUI.gameObject.SetActive(false);
	}

	public void UpgradeStat (string stat) {
		player.GetComponent<Maya>().UpgradeStats(stat);
	}

	public void Upgrade () {
		statsUI.gameObject.SetActive(true);
	}
}
