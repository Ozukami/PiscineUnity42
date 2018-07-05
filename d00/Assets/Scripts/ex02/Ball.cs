using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public GameObject club;
	public Club clubScript;
	public GameObject hole;
	public int score = -15;

	private bool isMoving = false;
	private int strength;
	private int direction = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			transform.Translate(0, 1F * strength * direction * Time.deltaTime, 0);
			if (transform.localPosition.y > hole.transform.localPosition.y - 0.5F
				&& transform.localPosition.y < hole.transform.localPosition.y + 0.5F && strength < 5) {
				score -= 5;
				Debug.Log("Final Score: " + score);
				Destroy(gameObject);
			}
			if (transform.localPosition.y > 4.5F) {
				direction = -1;
			} else if (transform.localPosition.y < -4.5F) {
				direction = 1;
			}
			if (strength > 0) {
				strength--;
			} else {
				isMoving = false;
				if (transform.localPosition.y < hole.transform.localPosition.y) {
					direction = 1;
				} else {
					direction = -1;
				}
				Debug.Log("Score: " + score);
				clubScript.updatePos(transform.localPosition, direction);
			}
		}
	}

	public void move ( int strength ) {
		isMoving = true;
		this.strength = strength;
		score += 5;
	}
}
