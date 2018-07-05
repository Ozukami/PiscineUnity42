using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager instance { get; private set; }
	public Dictionary<string, AudioClip[]> audioDic = new Dictionary<string, AudioClip[]>();
	public AudioClip[] humanAcknowledge;
	public AudioClip[] humanAnnoyed;
	public AudioClip[] humanSelected;
	public AudioClip[] humanHelp;
	public AudioClip[] humanDeath;
	public AudioClip[] orcAcknowledge;
	public AudioClip[] orcAnnoyed;
	public AudioClip[] orcSelected;
	public AudioClip[] orcHelp;
	public AudioClip[] orcDeath;

	private AudioSource source;

	void Start () {
		audioDic.Add("humanAcknowledge", humanAcknowledge);
		audioDic.Add("humanAnnoyed", humanAnnoyed);
		audioDic.Add("humanSelected", humanSelected);
		audioDic.Add("humanHelp", humanHelp);
		audioDic.Add("humanDeath", humanDeath);
		audioDic.Add("orcAcknowledge", orcAcknowledge);
		audioDic.Add("orcAnnoyed", orcAnnoyed);
		audioDic.Add("orcSelected", orcSelected);
		audioDic.Add("orcHelp", orcHelp);
		audioDic.Add("orcDeath", orcDeath);
	}

	void Awake () {
		instance = this;
		source = GetComponent<AudioSource>();
	}

	public void Play (string tag, string action) {
		if (tag == "Human") {
			source.clip = (audioDic["human" + action])[Random.Range(0, audioDic["human" + action].Length)];
		} else if (tag == "Orc") {
			source.clip = (audioDic["orc" + action])[Random.Range(0, audioDic["human" + action].Length)];
		}
		source.Play();
	}
}
