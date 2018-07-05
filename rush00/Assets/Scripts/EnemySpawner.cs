using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	public GameObject[] weapons;
	public Sprite[] heads;
	public Sprite[] bodies;

	void Awake () {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y);
        GameObject newEnemy = Instantiate(enemy, spawnPos, transform.rotation, transform);
		GameObject enemyWeapon = Instantiate(weapons[Random.Range(0, weapons.Length)], transform);
		enemyWeapon.GetComponent<WeaponScript>().targetLayer = 11;
		enemyWeapon.GetComponent<WeaponScript>().ammoCount = -1;
		newEnemy.transform.parent = null;
		newEnemy.transform.Find("Head").GetComponent<SpriteRenderer>().sprite = heads[Random.Range(0, heads.Length)];
		newEnemy.transform.Find("Body").GetComponent<SpriteRenderer>().sprite = bodies[Random.Range(0, bodies.Length)];
		newEnemy.GetComponent<Enemy>().EquipWeapon(enemyWeapon);
		enemyWeapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
	}
}
