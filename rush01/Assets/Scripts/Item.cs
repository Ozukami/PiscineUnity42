using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeItem {
	Weapon, Shield, Armor, Helmet, Amulet, Shoulder
}

public enum Rarity {
	Common, Rare, Epic, Legendary
}
	

public class Item : MonoBehaviour
{
	[SerializeField] private TypeItem _type;
	[SerializeField] private Rarity _rarity;
	public Sprite sprite;
	

	// Use this for initialization
	void Start ()
	{
		if (_rarity == Rarity.Common)
			GetComponent<ParticleSystem>().startColor = Color.white;
		else if (_rarity == Rarity.Rare)
			GetComponent<ParticleSystem>().startColor = Color.blue;
		else if (_rarity == Rarity.Epic)
			GetComponent<ParticleSystem>().startColor = new Color32(153, 51, 153,255);
		else if (_rarity == Rarity.Legendary)
			GetComponent<ParticleSystem>().startColor = new Color32(255, 215, 0, 255);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Maya")
		{
			other.gameObject.GetComponent<Maya>().AddToBag(gameObject);
			gameObject.GetComponent<ParticleSystem>().Stop();
			gameObject.GetComponent<SphereCollider>().enabled = false;
			Debug.Log("you found an item !");
//			Destroy(gameObject);
		}
	}

	public TypeItem GetTypeItem () {
		return _type;
	}
}
