using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpellManager : MonoBehaviour
{

	public GameObject[] spells;
	private GameObject currentSpell = null;
	
	// Use this for initialization
	void Start () {
		spells = new GameObject[4];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey(KeyCode.Alpha1) && spells[0] != null)
			currentSpell = spells[0];
		else if (Input.GetKey(KeyCode.Alpha2) && spells[1] != null)
			currentSpell = spells[1];
		else if (Input.GetKey(KeyCode.Alpha3) && spells[2] != null)
			currentSpell = spells[2];
		else if (Input.GetKey(KeyCode.Alpha4) && spells[3] != null)
			currentSpell = spells[3];
		/*else if (Input.GetKey(KeyCode.Alpha4) && spells[4] != null)
			currentSpell = spells[4];
		else if (Input.GetKey(KeyCode.Alpha4) && spells[5] != null)
			currentSpell = spells[5];*/
		else
			currentSpell = null;
		if (currentSpell != null)
		{
			GetComponent<NavMeshAgent>().ResetPath();
			currentSpell.GetComponent<Spell>().Cast(gameObject);
		}
	}
}
