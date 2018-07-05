using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Text title;
	public Text resetFeedback;

	private float feedback = 0;

	// Use this for initialization
	void Start () {

	}
	
	void Awake () {
		StartCoroutine(TwinkleHint(0.5f));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
			SceneManager.LoadScene(1);
		}
		if (feedback > 0) {
			feedback -= Time.deltaTime;
		} else { resetFeedback.text = ""; }
	}

	IEnumerator TwinkleHint (float time) {
		while (true) {
			if (title.gameObject.activeSelf)
				title.gameObject.SetActive(false);
			else
				title.gameObject.SetActive(true);
			yield return new WaitForSeconds(time);
		}
	}

	public void ResetPlayerPref () {
		PlayerPrefs.DeleteAll();
		resetFeedback.text = "ok!";
		feedback = 2.5f;
	}
}
