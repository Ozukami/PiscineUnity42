using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;

	private int directionX;
	private int directionY;
	private int scoreP1 = 0;
	private int scoreP2 = 0;

	// Use this for initialization
	void Start () {
		randomDir();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0.2F * directionX, 0.2F * directionY, 0);
		if (transform.localPosition.y > 4.2F || transform.localPosition.y < -4.2F) {
			directionY *= -1;
		}
		if (transform.localPosition.x <= player1.transform.localPosition.x + 0.5F
				&& transform.localPosition.x >= player1.transform.localPosition.x - 0.5F
				&& transform.localPosition.y < player1.transform.localPosition.y + 1.3F
				&& transform.localPosition.y > player1.transform.localPosition.y - 1.3F) {
			if (transform.localPosition.x + 0.2F < player1.transform.localPosition.x + 0.5F) {
				directionY *= -1;
			} else { directionX *= -1; }
		}
		if (transform.localPosition.x <= player2.transform.localPosition.x + 0.5F
				&& transform.localPosition.x >= player2.transform.localPosition.x - 0.5F
				&& transform.localPosition.y < player2.transform.localPosition.y + 1.3F
				&& transform.localPosition.y > player2.transform.localPosition.y - 1.3F) {
			if (transform.localPosition.x - 0.2F > player2.transform.localPosition.x - 0.5F) {
				directionY *= -1;
			} else { directionX *= -1; }
		}
		if (transform.localPosition.x > 8.5) {
			scoreP1++;
			Debug.Log("Plyer 1: " + scoreP1 + " | Player 2: " + scoreP2);
			transform.localPosition = new Vector3(0, 0, 0);
			randomDir();
		}
		else if (transform.localPosition.x < -8.5) {
			scoreP2++;
			Debug.Log("Plyer 1: " + scoreP1 + " | Player 2: " + scoreP2);
			transform.localPosition = new Vector3(0, 0, 0);
			randomDir();
		}
	}

	void randomDir () {
		if (Random.Range(0, 2) > 0) {
			directionX = 1;
		} else { directionX = -1; }
		if (Random.Range(0, 2) > 0) {
			directionY = 1;
		} else { directionY = -1; }
	}

}