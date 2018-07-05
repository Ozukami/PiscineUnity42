using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour {
    void Start () {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }

    void OnTriggerEnter (Collider other) {
        if (gameObject.tag == "ZodSpark")
            if (other.gameObject.tag == "Zombie" && other.GetType() == typeof(CapsuleCollider))
                other.GetComponent<Enemy>().TakeDamage(20);
    }
}