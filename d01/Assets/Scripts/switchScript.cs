using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchScript : MonoBehaviour {

	public GameObject[] platforms;
	public GameObject[] doors;
	public GameObject[] blueDoors;
	public GameObject[] redDoors;
	public GameObject[] yellowDoors;
	private Dictionary<string, GameObject[]> playerColorDoors;
	public string targetTag;
	public string targetColor;

	// Use this for initialization
	void Start () {
		if (targetTag == "AllPlatforms") {
			platforms = GameObject.FindGameObjectsWithTag("Platform");
		} else if (targetTag == "AllDoor") {
			blueDoors = GameObject.FindGameObjectsWithTag("BlueDoor");
			redDoors = GameObject.FindGameObjectsWithTag("RedDoor");
			yellowDoors = GameObject.FindGameObjectsWithTag("YellowDoor");
			doors = new GameObject[blueDoors.Length + redDoors.Length + yellowDoors.Length];
			blueDoors.CopyTo(doors, 0);
			redDoors.CopyTo(doors, blueDoors.Length);
			yellowDoors.CopyTo(doors, blueDoors.Length + redDoors.Length);
		} else if (targetTag == "PlayerColorDoor") {
			blueDoors = GameObject.FindGameObjectsWithTag("BlueDoor");
			redDoors = GameObject.FindGameObjectsWithTag("RedDoor");
			yellowDoors = GameObject.FindGameObjectsWithTag("YellowDoor");
			playerColorDoors = new Dictionary<string, GameObject[]>();
			playerColorDoors.Add("Blue", blueDoors);
			playerColorDoors.Add("Red", redDoors);
			playerColorDoors.Add("Yellow", yellowDoors);
		} else {
			doors = GameObject.FindGameObjectsWithTag(targetTag);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void open () {
		foreach (var door in doors) {
			if (door) {
				door.GetComponent<doorScript>().open();
				// door.transform.Rotate(new Vector3(0, 0, 90));
			}
		}
	}

	public void close () {
		foreach (var door in doors) {
			if (door) {
				door.GetComponent<doorScript>().close();
				// door.transform.Rotate(new Vector3(0, 0, -90));
			}
		}
	}

	public void openColor ( string color ) {
		foreach (var door in playerColorDoors[color]) {
			if (door) {
				door.GetComponent<doorScript>().open();
				// door.transform.Rotate(new Vector3(0, 0, 90));
			}
		}
	}

	public void closeColor ( string color ) {
		foreach (var door in playerColorDoors[color]) {
			if (door) {
				door.GetComponent<doorScript>().close();
				// door.transform.Rotate(new Vector3(0, 0, -90));
			}
		}
	}
	
	public void changeColors () {
		foreach (var platform in platforms) {
			if (platform) {
				if (platform.layer == 11) {
					platform.layer = 12;
					platform.GetComponent<SpriteRenderer>().color = Color.blue;
				} else if (platform.layer == 12) {
					platform.layer = 13;
					platform.GetComponent<SpriteRenderer>().color = Color.yellow;
				} else if (platform.layer == 13) {
					platform.layer = 11;
					platform.GetComponent<SpriteRenderer>().color = Color.red;
				}
			}
		}
	}
}
