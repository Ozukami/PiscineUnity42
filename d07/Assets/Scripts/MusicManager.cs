using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager Instance { get; private set; }
	public static MusicManager instance = null;

	public AudioSource explosion, gunShot, noHit, music1, music2;

	public GameObject[] enemies;

	void Awake () {
		if (instance != null && instance != this)
			Destroy(gameObject);
		instance = this;
	}

	void Update () {
		if (enemies.Length > 0) {
			bool flag = true;
			foreach (var enemy in enemies)
				if (enemy != null)
					if (enemy.GetComponent<Tank>().isPatrolling == false)
						flag = false;
			if (flag && !music1.isPlaying) {
				music1.Play();
				if (music2.isPlaying)
					music2.Stop();
			} else if (!flag && !music2.isPlaying) {
				music2.Play();
				if (music1.isPlaying)
					music1.Stop();
			}
		}
	}
}