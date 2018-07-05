using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour {

	public GameObject ball;
	public Ball ballScript;
	public SpriteRenderer clubSR;
	public GameObject hole;

	private bool hold = false;
	private int strength = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("space")) {
			hold = true;
			strength++;
			if (transform.localPosition.y < hole.transform.localPosition.y) {
				clubSR.flipY = false;
				transform.Translate(0, -0.1F, 0);
			} else {
				clubSR.flipY = true;
				transform.Translate(0, 0.1F, 0);
			}
		} else if (hold) {
			hold = false;
			ballScript.move(strength);
			strength = 0;
		}
	}

	public void updatePos ( Vector3 newPos, int direction ) {
		transform.localPosition = newPos;
		transform.Translate(-0.2F, 0.2F * direction, 0);
		if (transform.localPosition.y < hole.transform.localPosition.y) {
			clubSR.flipY = false;
		} else {
			clubSR.flipY = true;
		}
	}
}
