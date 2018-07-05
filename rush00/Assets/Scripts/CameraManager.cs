using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Transform PlayerTransform;
    private Camera cam;

	void Start() {
        cam = this.GetComponent<Camera>();
	}

	void Update () {
        if (PlayerTransform != null)
        {
            Vector3 pos = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, this.transform.position.z);
            this.transform.position = pos;
        }

        Color color = new Color(
            (Mathf.Sin(Time.time)+1f)*0.5f,
            0.1f+(Mathf.Sin(Time.time*0.33f)+1f)*0.5f,
            0.2f+(Mathf.Cos(Time.time*0.15f)+1f)*0.5f
        );
        cam.backgroundColor = color;
	}
}
