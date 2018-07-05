using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lockers : MonoBehaviour {
	
	public GameObject screen;
	public Light lightScreen;
	public Material unlockMat;
	public AudioSource soundEffect;
	public GameObject door;

	private bool isActivated = false;
	
	public void Activate () {
		if (!isActivated) {
			if (door != null) {
				door.GetComponent<Animator>().SetBool("isOpen", true);
				door.GetComponent<MeshCollider>().enabled = false;
			}
			lightScreen.color = Color.green;
			Material[] mat = screen.GetComponent<MeshRenderer>().materials;
			mat[0] = unlockMat;
			screen.GetComponent<MeshRenderer>().materials = mat;
			soundEffect.Play();
			isActivated = true;
		}
	}

}
