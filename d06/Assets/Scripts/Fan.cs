using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {
    public ParticleSystem smoke;
    public Canvas hint;

    void OnTriggerStay (Collider other) {
        if (Input.GetKeyDown(KeyCode.E))
            smoke.Play();
    }
}