using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour {

	public float lifeTime;
	public int targetLayer;

	void Awake() {
		GameObject.Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 13) {
			GameObject.Destroy(gameObject);
		} else if (other.gameObject.layer == targetLayer && other.GetType() == typeof(CapsuleCollider2D)) {
			if (targetLayer == 10) {
				GameObject.Destroy(gameObject);
				other.GetComponent<Enemy>().Die();
			}
		}
	}
}
