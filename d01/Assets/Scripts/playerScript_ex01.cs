using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScript_ex01 : MonoBehaviour {

	public camera mainCamera;
	public float velocity;
	public float jumpVelocity;
	public string activeCharTag;

	private Rigidbody2D rb2d;
	private BoxCollider2D bc2d;
	private static int exitActive = 0;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		bc2d = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("1")) {
			activeCharTag = "Red";
			updateMass();
			updateCamera();
		}
		if (Input.GetKey("2")) {
			activeCharTag = "Yellow";
			updateMass();
			updateCamera();
		}
		if (Input.GetKey("3")) {
			activeCharTag = "Blue";
			updateMass();
			updateCamera();
		}
	}

	void FixedUpdate () {
		if (Input.GetKey("left")) {
			if (this.tag == activeCharTag) {
				transform.Translate(-1 * velocity * Time.deltaTime, 0, 0);
			}
		}
		if (Input.GetKey("right")) {
			if (this.tag == activeCharTag) {
				transform.Translate(velocity * Time.deltaTime, 0, 0);
			}
		}
		if (Input.GetKeyDown("space") && this.tag == activeCharTag && isGrounded()) {
			rb2d.AddForce(new Vector2(0, jumpVelocity), ForceMode2D.Impulse);
		}
	}

	void updateMass () {
		if (this.tag == activeCharTag) {
			rb2d.mass = 1;
		} else {
			rb2d.mass = 2000;
		}
	}

	void updateCamera () {
		if (this.tag == activeCharTag) {
			mainCamera.player = gameObject;
		}
	}

	bool isGrounded () {
		Vector2 pos = new Vector2(transform.position.x, transform.position.y - bc2d.bounds.extents.y - 0.02f);

		if (activeCharTag == "Yellow") {
			if (Input.GetKey("left")) {
				pos += new Vector2(bc2d.bounds.extents.x, 0);
			} else if (Input.GetKey("right")) {
				pos += new Vector2(-bc2d.bounds.extents.x, 0);
			}
			Debug.DrawRay(pos, Vector2.down, Color.green);
			return Physics2D.Raycast(pos, Vector2.down, 0.02f).collider;
		}

		float dist = bc2d.bounds.extents.x - 0.05f;

		Debug.DrawRay(pos, Vector2.left, Color.green);
		Debug.DrawRay(pos, Vector2.right, Color.green);

		RaycastHit2D groundL = Physics2D.Raycast(pos, Vector2.left, dist);
		RaycastHit2D groundR = Physics2D.Raycast(pos, Vector2.right, dist);

		return (groundL.collider || groundR.collider);
	}

	void OnTriggerEnter2D ( Collider2D collider ) {
		if (this.tag + "Exit" == collider.tag) {
			exitActive++;
			Debug.Log("Triggered ! " + collider.tag + "(" + exitActive + ")");
			if (exitActive == 6) {
				Debug.Log("YOU WIN !!!");
				exitActive = 0;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}

	void OnTriggerExit2D ( Collider2D collider ) {
		if (this.tag + "Exit" == collider.tag) {
			exitActive--;
			Debug.Log("unTriggered ! " + collider.tag + "(" + exitActive + ")");
		}
	}
}
