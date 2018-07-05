using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private GameManager gM;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        gM = GameManager.gM;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (rb.velocity.magnitude <= 0.4f && rb.angularVelocity.magnitude <= 0.4f) {
			Debug.Log("immobile");
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}
	}

	public bool IsStatic () {
		if (rb.velocity == Vector3.zero)
			return true;
		return false;
	}
}
