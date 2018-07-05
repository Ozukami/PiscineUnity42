using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDestroy : MonoBehaviour {

	private ParticleSystem ps;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if (ps)
		{
			if (!ps.IsAlive())
			{
				Destroy(gameObject);
			}
		}
	}
}
