using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

	public GameObject player;
	
	public GameObject spell;
	public GameObject finalSpell;
	public GameObject image;

	public Canvas skillHintUI, statsUI;

	private GameObject tmp;
	private bool isOk;
	private bool drag;
	private bool memWin;
	private bool updateHintUI = false;

	void Awake () {
		
	}

	void Start()
	{
		finalSpell = Instantiate(spell);
		player.GetComponent<Maya>().spells[spell.GetComponent<Spell>().GetId()] = finalSpell;
	}

	void Update ()
	{
		if (spell != null && player != null) {
			Spell s = spell.GetComponent<Spell>();
			Maya m = player.GetComponent<Maya>();
			if (s.GetRequiredLevel() > m.level || !m.IsSkillAvailable(s.GetId(), s.GetRequiredIds())) {
				transform.GetChild(1).GetComponent<Image>().color = Color.grey;
				transform.GetChild(2).gameObject.SetActive(false);
			} else if (m.IsSkillAvailable(s.GetId(), s.GetRequiredIds())) {
				if (m.IsSkillLearned(s.GetId()))
					transform.GetChild(1).GetComponent<Image>().color = Color.white;
				else
					transform.GetChild(1).GetComponent<Image>().color = Color.grey;
				if (m.GetSkillPoints() > 0 && finalSpell.GetComponent<Spell>().GetLevel() < 5)
					transform.GetChild(2).gameObject.SetActive(true);
				else
					transform.GetChild(2).gameObject.SetActive(false);
			}
		} else if (spell == null) {
			if (transform.childCount > 1) {
				transform.GetChild(1).GetComponent<Image>().color = Color.grey;
				transform.GetChild(2).gameObject.SetActive(false);
			}
		}
		if (updateHintUI)
		{
			UpdateHintUI();
		}
	}

	void UpdateHintUI ()
	{
		if (spell != null) {
			Spell s = finalSpell.GetComponent<Spell>();
			if (s.GetLevel() < 5)
			{
				UIManager.UI.levelUI.text = s.GetLevel() + " -> " + (s.GetLevel() + 1);
				UIManager.UI.damageUI.text = s.GetDamage() + " -> " + (s.GetDamage() * 1.5f);
			}
			else
			{
				UIManager.UI.levelUI.text = s.GetLevel() + "";
				UIManager.UI.damageUI.text = s.GetDamage() + "";
			}
			UIManager.UI.skillNameUI.text = s.gameObject.name.Replace("(Clone)", "").Trim();
			UIManager.UI.typeUI.text = s.GetType() + "";
			UIManager.UI.spellTargetUI.text = s.GetTarget() + "";
			UIManager.UI.manaCostUI.text = s.GetManaCost() + "";
			if (s.GetType() != Type.Passif)
			{
				UIManager.UI.cooldownUI.text = s.GetCooldown() + "";
				UIManager.UI.durationUI.text = s.GetDuration() + "";
			}
			else
			{
				UIManager.UI.cooldownUI.text = "0";
				UIManager.UI.durationUI.text = "0";
			}
			UIManager.UI.rangeUI.text = s.GetRange() + "";
			UIManager.UI.skillDescriptionUI.text = s.GetDescription();
		}
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (spell != null && player.GetComponent<Maya>().IsSkillLearned(spell.GetComponent<Spell>().GetId()) && spell.GetComponent<Spell>().GetType() != Type.Passif) {
			tmp = Instantiate(image);
			tmp.transform.SetParent(this.transform, false);
			drag = true;
		} else { drag = false; }
	}

	public void OnDrag (PointerEventData eventData) {
		if (spell != null && drag) {
			tmp.transform.position = Input.mousePosition;
		}
	}

	public void OnEndDrag (PointerEventData eventData) {
		if (spell != null && drag) {
			isOk = false;
			PointerEventData pointerData = new PointerEventData(EventSystem.current);
			pointerData.position = Input.mousePosition;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			if (results.Count > 0) {
				foreach (RaycastResult result in results) {
					if (result.gameObject.tag.Contains("Slot")) {
						isOk = true;
						SkillBar skillBar = result.gameObject.transform.parent.parent.GetComponent<SkillBar>();
						SpellManager spellManager = skillBar.player.GetComponent<SpellManager>();
						spellManager.spells[int.Parse(result.gameObject.name.Replace("Slot ", "").Trim()) - 1] = finalSpell;
						tmp.transform.SetParent(result.gameObject.transform, true);
						tmp.AddComponent<AspectRatioFitter>();
						tmp.GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.FitInParent;
						tmp.gameObject.tag = "Skill";
					}
				}
			}
			if (!isOk)
				Destroy(tmp);
		}
	}

	public void OnPointerEnter (PointerEventData eventData) {
		UpdateHintUI();
		memWin = UIManager.UI.statsUI.gameObject.activeSelf;
		UIManager.UI.statsUI.gameObject.SetActive(false);
		UIManager.UI.skillHintUI.gameObject.SetActive(true);
		updateHintUI = true;
	}

	public void OnPointerExit (PointerEventData eventData) {
		if (memWin)
			UIManager.UI.statsUI.gameObject.SetActive(true);
		UIManager.UI.skillHintUI.gameObject.SetActive(false);
		updateHintUI = false;
	}
}
