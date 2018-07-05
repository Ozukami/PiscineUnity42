using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] enemies;

	private float time;
	private float timer;
	
	public Transform mapCenter;

	void Start () {
		SpawnEnemy();
	}
	
	void Update () {
		time += Time.deltaTime;
		if (time >= timer && !GameManager.gM.inPause)
			if (GameManager.gM.enemies < 20)
				SpawnEnemy();
	}

	void SpawnEnemy () {
		GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform);
		GameManager.gM.enemies++;
		Enemy enemy = newEnemy.GetComponent<Enemy>();

		enemy.mapCenter = mapCenter;
		enemy.healthPoints = 80;
		enemy.atkDmg = 5 ;
		
		timer = Random.Range(3.5f, 7.5f);

		if (GameManager.gM.wave > 1) {
			float factor = 0.15f * (GameManager.gM.wave - 1);
			enemy.healthPoints += (int)(80 * factor);
			enemy.atkDmg += (int)(5 * factor);
		}

		time = 0;
	}
}
