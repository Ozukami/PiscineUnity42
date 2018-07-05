using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

	// public GameObject cube;
	public GameObject aCube;
	public GameObject sCube;
	public GameObject dCube;
	public float spawnTime;

	private float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer >= spawnTime) {
			timer = 0;
			// Vector3 newPos = new Vector3(Random.Range(-1, 2) * 3.5f, 4, 0);
			// GameObject.Instantiate(cube, newPos, Quaternion.identity);
			int type = Random.Range(0, 3);
			if (type == 0) {
				GameObject.Instantiate(aCube);
			} else if (type == 1) {
				GameObject.Instantiate(sCube);
			} else {
				GameObject.Instantiate(dCube);
			}
		}
		timer += Time.deltaTime;
	}
}
