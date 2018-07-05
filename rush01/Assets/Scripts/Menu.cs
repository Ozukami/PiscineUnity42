using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public Canvas inventoryUI, skillTreeUI, statsUI, pauseUI;
	public GameObject player;

	public void Resume () {
		Time.timeScale = 1;
		pauseUI.gameObject.SetActive(false);
	}

	public void Retry () {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void RageQuit () {
		Application.Quit();
	}

	public void CloseInventory () {
		inventoryUI.gameObject.SetActive(false);
	}

	public void CloseSkillTree () {
		skillTreeUI.gameObject.SetActive(false);
	}

	public void CloseStats () {
		statsUI.gameObject.SetActive(false);
	}

	public void UpgradeStat (string stat) {
		player.GetComponent<Maya>().UpgradeStats(stat);
	}

	public void Upgrade () {
		skillTreeUI.gameObject.SetActive(true);
		statsUI.gameObject.SetActive(true);
	}

	public void UpgradeSkill (int skillIndex) {
		player.GetComponent<Maya>().UpgradeSkill(skillIndex);
	}

	public void ResetSkillPoints () {
		player.GetComponent<Maya>().ResetSkillPoints();
	}
}
