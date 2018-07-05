using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {

	public int holeIndex;
	public Transform flag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Ball" && GameManager.gM.currentHole == holeIndex) {
			Debug.Log("NextHole");
			GameManager.gM.NextHole();
		}
	}
}
