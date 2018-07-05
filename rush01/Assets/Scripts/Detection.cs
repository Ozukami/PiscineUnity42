using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour {


	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Maya") {
			Debug.Log("Trigger: " + other.gameObject.name);
		}
	}
}
