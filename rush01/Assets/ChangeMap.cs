using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeMap : MonoBehaviour {


	[SerializeField] private GameObject map;
	[SerializeField] private GameObject map2;
	[SerializeField] private Vector3 pos;

	private NavMeshAgent agent;

	void OnTriggerEnter(Collider col)
	{
		map.SetActive(true);
		agent = col.GetComponent<NavMeshAgent>();
		agent.Warp(pos);
		map2.SetActive(false);
	}
}
