using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public GameObject pauseCanvas;
	public GameObject scoreCanvas;
	public GameObject nextLevelButton;
	public GameObject retryLevelButton;

	public GameObject canonTurret;
	public GameObject gatlingTurret;
	public GameObject rocketTurret;

	public Text lifeText, energyText;
	public Image imageCT;
	public Text fireRateCT, damageCT, rangeCT, energyCT;
	public Image imageGT;	
	public Text fireRateGT, damageGT, rangeGT, energyGT;
	public Image imageRT;	
	public Text fireRateRT, damageRT, rangeRT, energyRT;
	public Text timeScale;
	public Text victory, score, grade;

	public static GUI gui;

	private bool start = true;
	private bool end = false;
	private bool flag = false;

	void Start () {
		gui = this;
		pauseCanvas.gameObject.SetActive(false);
		if (scoreCanvas)
			scoreCanvas.gameObject.SetActive(false);
		// gameManager.gm.changeSpeed(0);
	}

	void Update () {
		if (start) {
			gameManager.gm.changeSpeed(0);
			start = false;
		}
		lifeText.text = "" + gameManager.gm.playerHp;
		energyText.text = "" + gameManager.gm.playerEnergy;
		if (gameManager.gm.playerHp > 0 && !gameManager.gm.lastWave) {			
			towerScript cT = canonTurret.GetComponent<towerScript>();
			if (cT) {
				fireRateCT.text = "" + cT.fireRate;
				damageCT.text = "" + cT.damage;
				rangeCT.text = "" + cT.range;
				energyCT.text = "" + cT.energy;
			}
			
			towerScript gT = gatlingTurret.GetComponent<towerScript>();
			if (gT) {
				fireRateGT.text = "" + gT.fireRate;
				damageGT.text = "" + gT.damage;
				rangeGT.text = "" + gT.range;
				energyGT.text = "" + gT.energy;
			}
			
			towerScript rT = rocketTurret.GetComponent<towerScript>();
			if (rT) {
				fireRateRT.text = "" + rT.fireRate;
				damageRT.text = "" + rT.damage;
				rangeRT.text = "" + rT.range;
				energyRT.text = "" + rT.energy;
			}

			if (cT && gT && rT) {
				if (gameManager.gm.playerEnergy < cT.energy) {
					imageCT.color = Color.red;
				} else { imageCT.color = Color.white; }
				if (gameManager.gm.playerEnergy < gT.energy) {
					imageGT.color = Color.red;
				} else { imageGT.color = Color.white; }
				if (gameManager.gm.playerEnergy < rT.energy) {
					imageRT.color = Color.red;
				} else { imageRT.color = Color.white; }
			}

			if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0) {
				gameManager.gm.changeSpeed(0);
				pauseCanvas.gameObject.SetActive(true);
				Debug.Log(pauseCanvas.transform.Find("DarkerBackground").Find("PausePanel"));
				pauseCanvas.transform.Find("DarkerBackground").Find("PausePanel").gameObject.SetActive(true);
				pauseCanvas.transform.Find("DarkerBackground").Find("ConfirmPanel").gameObject.SetActive(false);
			}

			if (Time.timeScale == 0) {
				timeScale.text = "Paused";
			} else {
				timeScale.text = "Speed : " + Time.timeScale + "X";
			}
		} else if (gameManager.gm.lastWave && gameManager.gm.playerHp > 0 && !end) {
			end = true;
			GameObject[] spawners = GameObject.FindGameObjectsWithTag("spawner");
			foreach (GameObject spawner in spawners) {
				if (spawner.GetComponent<ennemySpawner>().isEmpty == false || spawner.transform.childCount > 0) {
					end = false;
				}
			}
		} else if (!flag) {
			flag = true;
			gameManager.gm.changeSpeed(0);
			score.text = "" + gameManager.gm.score;
			nextLevelButton.gameObject.SetActive(true);
			retryLevelButton.gameObject.SetActive(false);
			if (gameManager.gm.playerHp == 20) { grade.text = "S+"; }
			else if (gameManager.gm.playerHp == 19) { grade.text = "S"; }
			else if (gameManager.gm.playerHp == 18) { grade.text = "A"; }
			else if (gameManager.gm.playerHp > 15) { grade.text = "B"; }
			else if (gameManager.gm.playerHp > 10) { grade.text = "C"; }
			else if (gameManager.gm.playerHp > 5) { grade.text = "D"; }
			else if (gameManager.gm.playerHp > 0) { grade.text = "E"; }
			else if (gameManager.gm.playerHp <= 0) {
				victory.text = "GameOver !";
				grade.text = "F";
				nextLevelButton.gameObject.SetActive(false);
				retryLevelButton.gameObject.SetActive(true);
			}
			scoreCanvas.SetActive(true);
		}
	}

	public void HidePauseCanvas () {
		pauseCanvas.gameObject.SetActive(false);
	}

	public void ConfirmExit () {
		pauseCanvas.transform.Find("DarkerBackground").Find("PausePanel").gameObject.SetActive(false);
		pauseCanvas.transform.Find("DarkerBackground").Find("ConfirmPanel").gameObject.SetActive(true);
	}
}
