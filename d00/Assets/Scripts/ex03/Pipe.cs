using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {

	public Bird bird;

	private float speed = 1;
	private bool scored = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!bird.isDead) {
			transform.Translate(-2 * speed * Time.deltaTime, 0, 0);
			if (transform.localPosition.x < -1.40 && !scored) {
				bird.score += 5;
				scored = true;
			}
			if (transform.localPosition.x < -3.65F) {
				transform.Translate(3.65F * 2, 0, 0);
				speed += 0.2F;
				scored = false;
			}
		}
	}
}
