using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;

	void Start () {
		offset = transform.position - player.transform.position;
	}

	void Update () {
		if (player != null && player.GetComponent<Maya>().healthPoints > 0)
			transform.position = player.transform.position + offset;
	}
}
