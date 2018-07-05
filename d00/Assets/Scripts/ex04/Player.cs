using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("w") && transform.localPosition.x == -7
				&& transform.localPosition.y < 3.4F) {
			transform.Translate(0, 0.2F, 0);
		} else if (Input.GetKey("s") && transform.localPosition.x == -7
				&& transform.localPosition.y > -3.4F) {
			transform.Translate(0, -0.2F, 0);
		}

		if (Input.GetKey("up") && transform.localPosition.x == 7
				&& transform.localPosition.y < 3.4F) {
			transform.Translate(0, 0.2F, 0);
		} else if (Input.GetKey("down") && transform.localPosition.x == 7
				&& transform.localPosition.y > -3.4F) {
			transform.Translate(0, -0.2F, 0);
		}
	}
}
