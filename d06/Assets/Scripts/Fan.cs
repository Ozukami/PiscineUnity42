using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {

	public ParticleSystem smoke;
	public Canvas hint;

	void Start () {
		// hint.gameObject.SetActive(false);
	}
	
	void OnTriggerEnter (Collider other) {
		// hint.gameObject.SetActive(true);
	}
	
	void OnTriggerStay (Collider other) {
		if (Input.GetKeyDown(KeyCode.E))
			smoke.Play();
	}

	void OnTriggerExit (Collider other) {
		// hint.gameObject.SetActive(false);
	}
}
