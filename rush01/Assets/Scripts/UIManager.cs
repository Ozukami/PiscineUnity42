using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager UI = null;

	public Canvas pauseUI, gameOverUI;
	public Canvas playerUI, targetUI, upgradeStatsUI, levelUpUI;
	public Canvas inventoryUI, skillTreeUI, skillHintUI, statsUI, skillBarUI;
	public Button upgradeButton;

	public Text skillNameUI, skillDescriptionUI;
	public Text levelUI, typeUI, spellTargetUI, damageUI;
	public Text manaCostUI, cooldownUI, rangeUI, durationUI;

	void Awake() {
		if (UI == null)
			UI = this;
		else if (UI != this)
			Destroy(gameObject);
		targetUI.gameObject.SetActive(false);
		gameOverUI.gameObject.SetActive(false);
		inventoryUI.gameObject.SetActive(false);
		skillTreeUI.gameObject.SetActive(false);
		statsUI.gameObject.SetActive(false);
		upgradeButton.gameObject.SetActive(false);
		levelUpUI.gameObject.SetActive(false);
	}

	void Update () {
		CloseOpenWindow();
		OpenInventory();
		OpenSkillTree();
	}

	void CloseOpenWindow () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (inventoryUI.gameObject.activeSelf || skillTreeUI.gameObject.activeSelf
				|| statsUI.gameObject.activeSelf || skillHintUI.gameObject.activeSelf) {
				inventoryUI.gameObject.SetActive(false);
				skillTreeUI.gameObject.SetActive(false);
				statsUI.gameObject.SetActive(false);
				skillHintUI.gameObject.SetActive(false);
			} else {
				if (pauseUI.gameObject.activeSelf) {
					pauseUI.gameObject.SetActive(false);
					Time.timeScale = 1;
				} else {
					pauseUI.gameObject.SetActive(true);
					Time.timeScale = 0;
				}
			}
		}
	}

	void OpenInventory () {
		if (Input.GetKeyDown(KeyCode.I) && inventoryUI.gameObject.activeSelf)
			inventoryUI.gameObject.SetActive(false);
		else if (Input.GetKeyDown(KeyCode.I) && !inventoryUI.gameObject.activeSelf)
			inventoryUI.gameObject.SetActive(true);
	}

	void OpenSkillTree () {
		if (Input.GetKeyDown(KeyCode.N) && skillTreeUI.gameObject.activeSelf) {
			skillTreeUI.gameObject.SetActive(false);
			skillHintUI.gameObject.SetActive(false);
		}
		else if (Input.GetKeyDown(KeyCode.N) && !skillTreeUI.gameObject.activeSelf)
			skillTreeUI.gameObject.SetActive(true);
	}

	public void CloseAllUI () {
		playerUI.gameObject.SetActive(false);
		targetUI.gameObject.SetActive(false);
		gameOverUI.gameObject.SetActive(false);
		upgradeStatsUI.gameObject.SetActive(false);
		inventoryUI.gameObject.SetActive(false);
		skillTreeUI.gameObject.SetActive(false);
		skillHintUI.gameObject.SetActive(false);
		statsUI.gameObject.SetActive(false);
		upgradeButton.gameObject.SetActive(false);
		levelUpUI.gameObject.SetActive(false);
		skillBarUI.gameObject.SetActive(false);
	}
}
