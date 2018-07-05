using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip[] clips;

    private AudioSource source;
    private int lastListened;

	void Start () {
        source = this.GetComponent<AudioSource>();
	}
	
	void Update () {
        if (!source.isPlaying)
            PlayTrack();
	}

    void PlayTrack() {
        if (clips.Length > 1)
        {
            int id = lastListened;
            while (lastListened == id)
                id = (int)Random.Range(0, clips.Length);
            source.PlayOneShot(clips[id], 0.5f);
            lastListened = id;
        } else {
            source.PlayOneShot(clips[0], 0.5f);
        }
    }
}
