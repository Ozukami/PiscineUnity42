using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

	public GameObject item;
	public GameObject player;
	private bool drag = false, isOk = false;

	private Vector3 old;
	public void OnBeginDrag (PointerEventData eventData) {
		if (eventData.pointerDrag.tag == "BagSlot" && eventData.pointerDrag.GetComponent<Image>().sprite != null)
		{
			drag = true;
			old = transform.position;
		}
		else
			drag = false;
	}

	public void OnDrag (PointerEventData eventData) {
		if (drag)
			transform.position = Input.mousePosition;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		isOk = false;
		if (drag)
		{
			PointerEventData pointerData = new PointerEventData(EventSystem.current);
			pointerData.position = Input.mousePosition;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			if (results.Count > 0)
			{
				foreach (RaycastResult result in results)
				{
					if (result.gameObject.tag.Contains("InventorySlot"))
					{
						if (result.gameObject.transform.parent.name == item.GetComponent<Item>().GetTypeItem().ToString())
						{
							isOk = true;
							transform.SetParent(result.gameObject.transform, true);
							gameObject.AddComponent<AspectRatioFitter>();
							GetComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.FitInParent;
							gameObject.tag = "Stuff";
						}
					}
				}
			}
			if (!isOk)
			{
				transform.position = old;
			}
		}
	}

	public void OnPointerEnter (PointerEventData eventData) {
		Debug.Log(4);
	}

	public void OnPointerExit (PointerEventData eventData) {
		Debug.Log(5);
	}
}
