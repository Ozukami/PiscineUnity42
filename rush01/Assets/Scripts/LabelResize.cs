using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelResize : MonoBehaviour {

	public Text[] labels;

	void Update () {
		int min = 100;
		foreach (var label in labels)
			if (label)
				if (label.cachedTextGenerator.fontSizeUsedForBestFit > 0)
					min = (label.cachedTextGenerator.fontSizeUsedForBestFit < min)
						? label.cachedTextGenerator.fontSizeUsedForBestFit
						: min;
		foreach (var label in labels)
			if (label)
				label.resizeTextMaxSize = min;
		
	}
}
