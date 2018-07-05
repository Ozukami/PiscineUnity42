using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour {

    public GameObject[] Weapons;

	void Start () {
        int id = (int)Random.Range(0, Weapons.Length);
        GameObject instance = Instantiate(Weapons[id], this.transform.position, this.transform.rotation);
	}
	
}
