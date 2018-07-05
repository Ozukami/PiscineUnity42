using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager gM = null;

	public int score;
	public int currentHole;
	public int shotsForCurrentHole;

	public GameObject player;
	public GameObject ball;
	public GameObject arrow;
	public GameObject[] starts;
	public GameObject[] holes;

	public Text hole, shot, club;

    public Slider powerSlider;
    public Text powerText;
    public Canvas nextLvl;

	private bool goNext = false;

	void Awake () {
		if (gM == null)
			gM = this;
		else if (gM != null)
			Destroy(gameObject);
		nextLvl.gameObject.SetActive(false);
	}

	void Update () {
		if (currentHole == starts.Length) {
			Debug.Log("You WIN !");
			Time.timeScale = 0;
		}
		if (Input.GetKeyDown(KeyCode.Return) && goNext) {
			nextLvl.gameObject.SetActive(false);
			GameManager.gM.currentHole++;
			if (currentHole == starts.Length) {
				Debug.Log("You WIN !");
				Time.timeScale = 0;
			}
			if (gM.currentHole < starts.Length) {
				shotsForCurrentHole = 0;
				ball.transform.position = starts[currentHole].transform.position;
				ball.transform.rotation = Quaternion.identity;
				player.transform.position = starts[currentHole].transform.position;
				player.transform.rotation = Quaternion.identity;
			}
		}
		hole.text = "Hole " + (currentHole + 1);
		shot.text = "Shot " + (shotsForCurrentHole + 1);
	}

	public void NextHole () {
		goNext = true;
		nextLvl.gameObject.SetActive(true);
	}

	public void KickBall () {
		shotsForCurrentHole++;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (ball.transform.position - mousePos).normalized;
		ball.GetComponent<Rigidbody>().AddForce(dir * 25 * (powerSlider.value + 1), ForceMode.Impulse);
		gM.powerSlider.value = 0;
	}

	public void Rotate () {
        Vector3 dir = (this.transform.position - holes[currentHole].transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, dir);
        this.transform.rotation = rot;
	}
}
