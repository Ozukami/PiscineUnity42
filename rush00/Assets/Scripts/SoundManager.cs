using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clips;
    public static SoundManager soundManager = null;

    private AudioSource source;
    private AudioClip currentClip = null;

	private void Awake()
	{
        soundManager = this;
	}

	void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    public void PlayAudioClip(AudioClip audioClip, float volume) {
        currentClip = audioClip;
        if (currentClip != null)
            source.PlayOneShot(currentClip, volume);
    } 

    public void PlayDelayedAudioClip(AudioClip audioClip, float delay) {
        currentClip = audioClip;
        if (currentClip != null) {
            source.clip = currentClip;
            source.PlayDelayed(delay);
        }
    }
}
