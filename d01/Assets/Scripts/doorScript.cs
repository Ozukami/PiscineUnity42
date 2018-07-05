using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour {

	private bool isOpen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void open () {
		if (!isOpen) {
			transform.Rotate(new Vector3(0, 0, 90));
			isOpen = true;
		}
	}

	public void close () {
		if (isOpen) {
			transform.Rotate(new Vector3(0, 0, -90));
			isOpen = false;
		}
	}
}
