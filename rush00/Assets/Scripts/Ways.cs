using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ways : MonoBehaviour {

	public static Ways instance = null;
	public GameObject[] CheckPoints;
	
	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

}
