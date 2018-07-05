using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour {

	public GameObject platform;
	public float speed;
	public Transform currentPoint;
	public Transform[] points;
	public int pointIndex;

	// Use this for initialization
	void Start () {
		currentPoint = points[pointIndex];
	}
	
	// Update is called once per frame
	void Update () {
		platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, speed * Time.deltaTime);
		if (platform.transform.position == currentPoint.position) {
			pointIndex = (pointIndex + 1) % points.Length;
			currentPoint = points[pointIndex];
		}
	}
}
