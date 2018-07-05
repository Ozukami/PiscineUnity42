using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

	public int direction;
	public float speed;
	public AudioClip[] startMovingSounds;

	private Animator animator;
	private Vector3 targetPosition;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPosition = new Vector3(mousePosition.x, mousePosition.y, 1);
			var angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0, 0, angle + 90);
			audioSource.clip = startMovingSounds[Random.Range(0, startMovingSounds.Length)];
			audioSource.Play();
		}
	}

	void FixedUpdate () {
		if (transform.position != targetPosition) {
			animator.SetBool("moving", true);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
		} else {
			animator.SetBool("moving", false);
		}
	}
}
