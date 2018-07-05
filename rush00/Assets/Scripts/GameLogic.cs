using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

    public GameObject enemySpawnerDir;
    [HideInInspector] public int enemyCount;
    [HideInInspector] public bool levelComplete = false;
    [HideInInspector] public bool levelLost = false;

    public static GameLogic gameLogic = null;

	public AudioClip winSound;
	public AudioClip loseSound;
	private AudioSource source;
    private bool soundPlayed = false;

	private void Awake() {
        gameLogic = this;
	}

	void Start () {
        source = this.GetComponent<AudioSource>();
        enemyCount = enemySpawnerDir.transform.childCount;
	}
	
	void Update () {
        CheckEndGameConditions();
        if (levelLost) {
            if (!source.isPlaying && !soundPlayed) {
                source.PlayOneShot(loseSound);
                soundPlayed = true;
            }
        }
	}

    void CheckEndGameConditions() {
        if (enemyCount <= 0) {
            levelComplete = true;
            if (!source.isPlaying && !soundPlayed) {
                source.PlayOneShot(winSound);
                soundPlayed = true;
            }
        }
    }
}
