using System;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour {

	[Serializable]
	public struct Loots
	{
		public Item item;
		public float percent;
		

	}
	public List<Loots> loots;
	
	//public Dictionary<Item, int> test = new Dictionary<Item, int>();
	
	void Start ()
	{
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.R))
			lootItem();
	}

	public Item lootItem()
	{
		float random = UnityEngine.Random.Range(0.0f, 100.0f);

		Debug.Log(random);

		float start = 0.0f;
		foreach (Loots loot in loots)
		{
			start += loot.percent;
			if (start >= random)
			{
				Debug.Log(loot.item + " " + loot.percent + " has been choose");
				return loot.item;
				break;
			}
		}
		return null;
	}
}
