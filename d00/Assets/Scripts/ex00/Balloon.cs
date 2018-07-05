using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour {

	public int maxBreath = 100;
	public Slider breathSlider;
	public Image fill;

	private float lifeTime;
	private int currentBreath;
	private bool breathless;

	void Start () {
		currentBreath = maxBreath;
		lifeTime = 0;
		breathless = false;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime += Time.deltaTime;
		if (Input.GetKeyDown("space") && !breathless) {
			fill.color = new Color(0.6F, 0.65F, 0.8F);
			if (currentBreath >= 15) {
				transform.localScale += new Vector3(0.3F, 0.3F, 0);
				currentBreath -= 15;
				breathSlider.value = currentBreath;
			}
		}
		if (currentBreath <= 5) {
			breathless = true;
			fill.color = Color.red;
		}
		if (breathless && currentBreath >= 30) {
			breathless = false;
		}
		if (transform.localScale.x > 4 && transform.localScale.y > 4) {
			Debug.Log("Balloon life time: " + Mathf.RoundToInt(lifeTime) + "s");
			Destroy(gameObject);
		}
		if (transform.localScale.x > 0 && transform.localScale.y > 0) {
			transform.localScale -= new Vector3(0.015F, 0.015F, 0);
			if (currentBreath < 100) {
				currentBreath += 1;
				breathSlider.value = currentBreath;
			}
		}
		else {
			Debug.Log("Balloon life time: " + Mathf.RoundToInt(lifeTime) + "s");
			Destroy(gameObject);
		}
	}
}
