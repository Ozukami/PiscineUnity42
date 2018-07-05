using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	public GameObject pipe;
	public bool isDead = false;
	public int score;

	private float lifeTime;
	private int jump = 0;

	// Use this for initialization
	void Start () {
		lifeTime = 0;
	}

	// X +1.40 ; -1.40
	// Y +1.30 ; -1.70
	
	// Update is called once per frame
	void Update () {
		lifeTime += Time.deltaTime;
		if (!isDead) {
			if (jump == 0) {
				transform.Translate(0, -0.1F, 0);
			} else {
				transform.Translate(0, 0.1F, 0);
				jump--;
			}
			if (Input.GetKeyDown("space")) {
				jump = 15;
			}
			if (transform.localPosition.y <= -3.70F) {
				isDead = true;
				Debug.Log("Score: " + score + "\nTime: " + Mathf.RoundToInt(lifeTime) + "s");
			} else if ((transform.localPosition.y > 1.30F || transform.localPosition.y < -1.70F)
					&& (pipe.transform.localPosition.x > -1.40F && pipe.transform.localPosition.x < 1.40F)) {
				isDead = true;
				Debug.Log("Score: " + score + "\nTime: " + Mathf.RoundToInt(lifeTime) + "s");
			}
		}
	}
}
