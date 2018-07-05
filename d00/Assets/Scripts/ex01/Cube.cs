using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public int type = 0;
	public float speed = 0;

	// Use this for initialization
	void Start () {
		speed = 0.1F + ((float)Random.Range(1, 10) / 100);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, -speed, 0);
		if (transform.localPosition.y < -6) {
			Debug.Log("Failed");
			Destroy(gameObject);
		}
		if (transform.localPosition.y < -3 && transform.localPosition.y > -5) {
			float precision = (transform.localPosition.y + 4) * (-10);
			if (Input.GetKeyDown("a") && transform.localPosition.x < -1) {
				Debug.Log("Precision: " + precision);
				Destroy(gameObject);
			}
			else if (Input.GetKeyDown("s") && transform.localPosition.x == 0) {
				Debug.Log("Precision: " + precision);
				Destroy(gameObject);
			}
			else if (Input.GetKeyDown("d") && transform.localPosition.x > 1) {
				Debug.Log("Precision: " + precision);
				Destroy(gameObject);
			}
		}
	}
}
