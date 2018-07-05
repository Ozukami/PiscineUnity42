using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour {

	public GameObject player;
	/*
	private Dictionary<int, int> skillTree;
	private int skillPoints = 1;

	void Awake () {
		skillTree = new Dictionary<int, int>();
	}

	void Update () {
		ShowStats();
	}
	
	void ShowStats () {
		if (Input.GetKeyDown(KeyCode.C) && UIManager.UI.statsUI.gameObject.activeSelf)
			UIManager.UI.statsUI.gameObject.SetActive(false);
		else if (Input.GetKeyDown(KeyCode.C) && !UIManager.UI.statsUI.gameObject.activeSelf)
			UIManager.UI.statsUI.gameObject.SetActive(true);
		if (statsPoints <= 0 && skillPoints <= 0)
			UIManager.UI.upgradeStatsUI.gameObject.SetActive(false);
		else
			UIManager.UI.upgradeStatsUI.gameObject.SetActive(true);
		if ((statsPoints > 0 || skillPoints > 0) && !UIManager.UI.statsUI.gameObject.activeSelf)
			UIManager.UI.upgradeButton.gameObject.SetActive(true);
		else if ((statsPoints > 0 || skillPoints > 0) && UIManager.UI.statsUI.gameObject.activeSelf)
			UIManager.UI.upgradeButton.gameObject.SetActive(false);
	}
	
	public void ResetSkillPoints () {
		skillPoints = player.GetComponent<Maya>().level;
		skillTree.Clear();
		Debug.Log(skillTree.Count);
	}

	public int GetSkillPoints () {
		return skillPoints;
	}

	public void UpgradeSkill (int skillIndex) {
		if (skillPoints > 0) {
			if (skillTree.ContainsKey(skillIndex))
				skillTree[skillIndex] += 1;
			else
				skillTree.Add(skillIndex, 1);
			skillPoints--;
		}
	}

	public bool IsSkillAvailable (int skillIndex, int[] requiredSkills) {
		if (skillTree.ContainsKey(skillIndex))
			return true;
		foreach (var requiredSkill in requiredSkills) {
			if (!skillTree.ContainsKey(requiredSkill))
				return false;
		}
		return true;
	}

	public bool IsSkillLearned (int skillIndex) {
		return skillTree.ContainsKey(skillIndex);
	}

	public void AddSkillPoint () {
		skillPoints++;
	}*/
}
