using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour {

	[SerializeField] private int nextScene;

	void OnTriggerEnter(Collider col) {
	    SceneManager.LoadScene(nextScene);
	}
}