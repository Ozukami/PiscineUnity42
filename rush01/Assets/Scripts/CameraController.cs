using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	[SerializeField]private Vector3 offset;

	void Start () {
		transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
		transform.LookAt(player.transform.position);
	}

	void Update () {
		if (player != null && player.GetComponent<Maya>().healthPoints > 0)
		{
			transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z + offset.z);
			transform.LookAt(player.transform.position);
		}
	}
}