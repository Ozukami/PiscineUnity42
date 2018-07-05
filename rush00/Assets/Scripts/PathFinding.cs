using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

	public GameObject player;

	public CheckPoint startPoint;
	public CheckPoint endPoint;
	public GameObject[] CheckPoints;

	public static PathFinding instance = null;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		// List<Transform> path = GetPathFromTo(startPoint, endPoint);
		// foreach (var transform in path) {
		// 	Debug.Log(transform);
		// }
	}

	public List<Transform> GetPathFromTo(CheckPoint startPoint, CheckPoint endPoint) {
		Debug.Log(int.Parse(player.GetComponent<Player>().currentRoom));
		endPoint = CheckPoints[int.Parse(player.GetComponent<Player>().currentRoom)].GetComponent<CheckPoint>();
		this.startPoint = startPoint;
		List<CheckPoint> queue = new List<CheckPoint>();
		queue.Add(startPoint);
		while (queue.Count > 0) {
			if (queue[0] == endPoint)
				return BuildPath(queue[0], startPoint);
			foreach (var checkPoint in queue[0].nextCheckpoint) {
				if (!checkPoint.GetComponent<CheckPoint>().visited) {
					queue.Add(checkPoint.GetComponent<CheckPoint>());
					checkPoint.GetComponent<CheckPoint>().prev = queue[0];
					checkPoint.GetComponent<CheckPoint>().visited = true;
				}
			}
			queue.RemoveAt(0);
		}
		return null;
	}

	List<Transform> BuildPath(CheckPoint startPath, CheckPoint startPoint) {
		List<Transform> path = new List<Transform>();
		CheckPoint current = startPath;
		while (current != startPoint) {
			path.Add(current.transform);
			current = current.prev;
		}
		// path.Add(this.startPoint.transform);
		path.Reverse();
		foreach (var transform in path) {
			Debug.Log(transform);
		}
		return path;
	}

	public void ResetPathFinding() {
		foreach (var checkPoint in CheckPoints) {
			checkPoint.GetComponent<CheckPoint>().visited = false;
			checkPoint.GetComponent<CheckPoint>().prev = null;
		}
	}

	// public Transform FindCheckPoints(string name) {
	// 	foreach (var checkPoint in CheckPoints) {
	// 		if (checkPoint.name == "CheckPoint" + name) {
	// 			return checkPoint.transform;
	// 		}
	// 	}
	// 	return null;
	// }
}
