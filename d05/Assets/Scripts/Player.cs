using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	
	public float mouseSensitivity;
    public float speed;

    // public Slider powerSlider;
    // public Text powerText;
 
    private float rotationX = 0;
    private float rotationY = 0;
    private float maximumY = 90;
    private float minimumY = -90;

    private GameManager gM;
    private Rigidbody rb;

    private Vector3 tmp;
    private Vector3 pos;

    private bool focus = true;
    private bool power = false;
    private float time;

    private IEnumerator coroutine;

    void Start () {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        coroutine = PowerSlide();
        gM = GameManager.gM;
        transform.position = gM.ball.transform.position;
        transform.rotation = Quaternion.identity;
    }

    void FocusOn () {
        // if (gM.ball.GetComponent<Ball>().IsStatic()
        if (gM.ball.GetComponent<Rigidbody>().velocity.magnitude <= 0.4f && gM.ball.GetComponent<Rigidbody>().angularVelocity.magnitude <= 0.4f) {
            focus = true;
            transform.position = gM.ball.transform.position;
            transform.rotation = Quaternion.identity;
        }
    }

    void FocusOff () {
        focus = true;
        if (power) {
            power = false;
            focus = false;
            StopCoroutine(coroutine);
            gM.KickBall();
        } else {
            power = true;
            StartCoroutine(coroutine);
        }
    }

    void Update () {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))
            focus = false;
        else if (Input.GetKeyDown(KeyCode.Space) && !focus)
            FocusOn();
        else if (Input.GetKeyDown(KeyCode.Space) && focus)
            FocusOff();
        if (!focus) {
            gM.powerSlider.gameObject.SetActive(false);
            gM.powerText.gameObject.SetActive(false);
            gM.arrow.gameObject.SetActive(false);
        } else {
            gM.powerSlider.gameObject.SetActive(true);
            gM.powerText.gameObject.SetActive(true);
            gM.arrow.gameObject.SetActive(true);
        }
    }
 
    void FixedUpdate () {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Cursor.lockState = (focus) ? CursorLockMode.Confined : CursorLockMode.Locked;

        // Mouse
        if (!focus) {
            rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            tmp.x = -rotationY;
            tmp.y = rotationX;
            tmp.z = 0;
            transform.localEulerAngles = tmp;
        }
     
        // Keyboard
        if (!focus) {
            pos = getDirection();
            pos *= speed * Time.deltaTime;
            transform.Translate(pos);
        } else
            transform.Rotate(0.0f, Input.GetAxis ("Horizontal") * speed * 0.5f, 0.0f);
    }
 
    private Vector3 getDirection () {
        tmp.x = Input.GetAxis("Horizontal");
        tmp.y = 0;
        tmp.z = Input.GetAxis("Vertical");
		if (Input.GetKey(KeyCode.Q))
            tmp.y += -1;
		if (Input.GetKey(KeyCode.E))
            tmp.y += 1;
        return tmp;
    }

    IEnumerator PowerSlide () {
        time = 0;
        while (true) {
            gM.powerSlider.value = Mathf.Sin(time);
            yield return null;
        }
    }
}
