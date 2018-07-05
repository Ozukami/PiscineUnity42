using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target {
	Player, Enemy
}

public enum Type {
	Boost, Heal, Passif, Area, Shot, Gabarit
}

public class Spell : MonoBehaviour {
	
	[SerializeField] private int _id;
	[SerializeField] private Transform _player;
	[SerializeField] public int _level;
	[SerializeField] private Target _target;
	[SerializeField] private Type _type;
	[SerializeField] private int _damage;
	[SerializeField] private int _manaCost;
	[SerializeField] private float _cooldown;
	[SerializeField] private float _range;
	[SerializeField] private ParticleSystem _visualEffect;
	[SerializeField] private float _duration;
	[SerializeField] private float _tick;
	[SerializeField] private int _requiredLevel;
	[SerializeField] private int[] _requiredSkills;
	[SerializeField][TextArea] private string _description;
	[SerializeField] private bool isContineous;
	
	private static bool isFireSwordActive = false;
	private float _cooldownGlobal = 0;

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void Cast (GameObject caster) {
	
		if (_cooldownGlobal < Time.time)
		{
			if (_type == Type.Boost)
			{
				if (_target == Target.Enemy)
				{
					caster.GetComponent<Maya>().manaPoints -= _manaCost;
					
					ParticleSystem spell = Instantiate(_visualEffect, transform);
					spell.transform.position = caster.transform.position;
					_cooldownGlobal = Time.time + _cooldown;
					spell.transform.Translate(0, 1, 0);
					StartCoroutine(FollowCaster(spell, caster));
					StartCoroutine(PassifDuration(spell.GetComponentInChildren<SphereCollider>(), spell, caster));
				}
				else
				{
					GameObject spell = GameObject.FindWithTag("FireSword");
					_cooldownGlobal = Time.time + _cooldown;				
				
					if (!isFireSwordActive)
					{
						if (caster.GetComponent<Maya>().manaPoints < _manaCost)
							return;
						var main = spell.GetComponent<ParticleSystem>().main;
						var em = spell.GetComponent<ParticleSystem>().emission;
						main.maxParticles = 1000;
						em.enabled = true;
					
						caster.GetComponent<Maya>().manaPoints -= _manaCost;
						caster.GetComponent<Maya>().manaRegenPerSecond -= 1;
						caster.GetComponent<Maya>().minDamage += (_damage + (caster.GetComponent<Maya>().intelligence / 2));
						caster.GetComponent<Maya>().maxDamage += (4 + _damage + (caster.GetComponent<Maya>().intelligence / 2));
						isFireSwordActive = true;
					} 
					else 
					{
						var main = spell.GetComponent<ParticleSystem>().main;
						var em = spell.GetComponent<ParticleSystem>().emission;
						main.maxParticles = 0;
						em.enabled = false;				

						caster.GetComponent<Maya>().manaRegenPerSecond += 1;
						caster.GetComponent<Maya>().minDamage -= _damage + caster.GetComponent<Maya>().intelligence / 2;
						caster.GetComponent<Maya>().maxDamage -= _damage + caster.GetComponent<Maya>().intelligence / 2;
						isFireSwordActive = false;
					}
				}
			}
			else
			{

				if (caster.GetComponent<Maya>().manaPoints < _manaCost)
					return;
				if (_type == Type.Gabarit) {
					caster.GetComponent<Maya>().manaPoints -= _manaCost;

					ParticleSystem spell = Instantiate(_visualEffect, transform);
					SphereCollider col = spell.GetComponentInChildren<SphereCollider>();
					transform.position = caster.transform.position;
					transform.Translate(0, 25, 0);
					spell.GetComponentInChildren<Light>().enabled = true;
					StartCoroutine(Meteor(spell, col, caster));
					_cooldownGlobal = Time.time + _cooldown;

				} 
				else
				{
					caster.GetComponent<Maya>().manaPoints -= _manaCost;
					
					ParticleSystem spell = Instantiate(_visualEffect, transform);
					spell.transform.position = caster.transform.position;
					_cooldownGlobal = Time.time + _cooldown;
					if (_type == Type.Area)
					{
						SphereCollider effectCollider = spell.GetComponentInChildren<SphereCollider>();
						ScanForItems(effectCollider, spell, caster);
					}

					if (_type == Type.Shot) 
					{
						RaycastHit hit;
										
						spell.transform.Translate(0, 1, 0);
						Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
						float before_angleX = caster.transform.eulerAngles.x;
						caster.transform.LookAt(hit.point);
						float after_angleX = caster.transform.eulerAngles.x;
						/*if (after_angleX > 1)
							after_angleX = 15;
						else if (after_angleX < -10)
							after_angleX = -10;*/
						caster.transform.eulerAngles = new Vector3(0, caster.transform.eulerAngles.y,0);
						spell.gameObject.transform.rotation = caster.transform.rotation;		
						spell.gameObject.transform.Translate(Vector3.forward * 0.2f);
						CapsuleCollider effectCollider = spell.GetComponentInChildren<CapsuleCollider>();
						StartCoroutine(FireBall(effectCollider, spell, caster));
					}
					if (_type == Type.Heal)
					{
						spell.transform.Translate(0, 1, 0);
						caster.GetComponent<Maya>().Heal(_damage);
					}
					if (_type == Type.Passif)
					{
						caster.GetComponent<Maya>().boostArmor += 20;
						caster.GetComponent<Maya>().boostDamage += 20;
						caster.GetComponent<Maya>().boostHealth += 100;
						caster.GetComponent<Maya>().boostManaRegen += 10;
						caster.GetComponent<Maya>().boostHealthRegen += 10;
						
						StartCoroutine(FollowCaster(spell, caster));
						spell.transform.Translate(0, 0.4f, 0);
					
					}
				}
			}
		}
	}
	
	void ScanForItems(SphereCollider Item, ParticleSystem spell, GameObject caster)
	{
		Vector3 center = spell.transform.position;
		float radius = Item.radius;
     
		Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
		foreach (var col in allOverlappingColliders) {
			if (col.gameObject.tag == "Zombie")
			{
				float dist = Vector3.Distance(spell.transform.position, col.gameObject.transform.position);
				StartCoroutine(ApplyEffect(dist / _visualEffect.main.startSpeed.Evaluate(0), col, caster));
			}
		}
	}
	
	void ScanForItemsDirect(SphereCollider Item, ParticleSystem spell, GameObject caster)
	{
		Vector3 center = spell.transform.position;
		float radius = Item.radius;
     
		Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
		foreach (var col in allOverlappingColliders)
		{
			if (col.gameObject.tag == "Zombie")
			{
				if (col.gameObject.GetComponent<Enemy>().TakeDamage(_damage) == false)
				{
					caster.GetComponent<Maya>().GainExp(col.GetComponent<Enemy>().exp);
				}

			}
		}
	}

	IEnumerator Meteor(ParticleSystem spell, SphereCollider col, GameObject caster)
	{
		float pos = 0;
		bool falling = false;
		bool boom = false;
		while (spell)
		{
			if (!falling)
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, -1))
				{
					transform.position = hitInfo.point;
				}				
			}
			if (Input.GetMouseButtonDown(1) && spell) {
				pos = transform.position.y;
				falling = true;
				spell.GetComponentInChildren<Light>().enabled = false;
				spell.GetComponent<ParticleSystem>().Clear();
				spell.GetComponent<ParticleSystem>().Play();
			}
			if (falling) {
				spell.transform.Translate(0, 0, -0.3f);
				if (spell.transform.position.y < pos && !boom)
				{
					Vector3 center = transform.position;
					float radius = col.radius;
				
					Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
					foreach (var collider in allOverlappingColliders) {
						if (collider.gameObject.tag == "Zombie")
						{
							if (collider.gameObject.GetComponent<Enemy>().TakeDamage(500) == false)
							{
								caster.GetComponent<Maya>().GainExp(collider.gameObject.GetComponent<Enemy>().exp);
							}
						}
					}
					Destroy(spell.gameObject, 3f);
					boom = true;
				}
			}			
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator FireBall(CapsuleCollider fireballCollider, ParticleSystem fireball, GameObject caster) {
		float time = 0f;
		bool isAlive = true;
		while (true)
		{
			Vector3 center = fireball.transform.position;
			float radius = fireballCollider.radius;

			Collider[] allOverlappingColliders = Physics.OverlapSphere(center, radius);
			foreach (var col in allOverlappingColliders)
			{
				if (col.gameObject.tag == "Zombie")
				{
					if (col.gameObject.GetComponent<Enemy>().TakeDamage(_damage) == false)
					{
						caster.GetComponent<Maya>().GainExp(col.gameObject.GetComponent<Enemy>().exp);
					}
					if (isContineous == false)
					{
						Destroy(fireball.gameObject);
						//Destroy(fireball);
						isAlive = false;
					}
				}
			}
			if (!isAlive)
				break;
			time += Time.deltaTime;
			if (time > _range)
			{
				Destroy(fireball.gameObject);
				//Destroy(fireball);
				break;
			}
			fireball.transform.Translate(Vector3.forward * Time.deltaTime * 30f);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator ApplyEffect (float time, Collider col, GameObject caster) {
		yield return new WaitForSeconds(time);
		if (col.gameObject.GetComponent<Enemy>().TakeDamage(_damage) == false)
		{								
			caster.GetComponent<Maya>().GainExp(col.gameObject.GetComponent<Enemy>().exp);
		}
	}
	
	
	IEnumerator PassifDuration (SphereCollider sphere, ParticleSystem passif, GameObject caster)
	{
		for (float i = 0.0f; i < _duration; i += _tick)
		{
			ScanForItemsDirect(sphere, passif, caster);
			yield return new WaitForSeconds(_tick);
		}
		passif.Stop();
	}
	
	
	IEnumerator FollowCaster (ParticleSystem effect, GameObject caster)
	{
		while (true)
		{
			if (effect == null)
				break;
			effect.transform.position = new Vector3(caster.transform.position.x, effect.transform.position.y, caster.transform.position.z);
			yield return new WaitForFixedUpdate();
		}
	}

	public void Upgrade()
	{
		_level++;
		_damage = (int)(_damage * 1.5f);
	}

	public void Reset()
	{
		for (int i = 1; i < _level; i++)
		{
			_damage = (int) (_damage / 1.5f);
		}
		_level = 1;
	}

	public int GetId () {
		return _id;
	}

	public int GetLevel () {
		return _level;
	}
	
	public Target GetTarget () {
		return _target;
	}
	
	public Type GetType () {
		return _type;
	}
	
	public int GetDamage () {
		return _damage;
	}
	
	public int GetManaCost () {
		return _manaCost;
	}
	
	public float GetCooldown () {
		return _cooldown;
	}
	
	public float GetRange () {
		return _range;
	}
	
	public float GetDuration () {
		return _duration;
	}

	public int GetRequiredLevel () {
		return _requiredLevel;
	}

	public int[] GetRequiredIds () {
		return _requiredSkills;
	}

	public string GetDescription () {
		Debug.Log(_description);
		return _description;
	}
}


