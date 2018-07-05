using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPathCheckPoint : MonoBehaviour {

	public string gizmo;
    public Color lineColor = Color.green;
	private Transform target;

	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, gizmo + ".png", false);
		GameObject[] checkPoints = this.gameObject.GetComponent<CheckPoint>().nextCheckpoint;
		if (checkPoints != null) {
			foreach (var checkPoint in checkPoints) {
				target = checkPoint.transform;
                Gizmos.color = lineColor;
				Gizmos.DrawLine(transform.position, target.position);
			}
		}
	}
}
