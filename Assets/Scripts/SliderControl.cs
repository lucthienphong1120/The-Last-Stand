using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
	private Slider slider;
	private Text txt;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	public void UpdateBar(int currentValue, int maxValue)
	{
		if (slider == null)
		{
            slider = GetComponent<Slider>();
            txt = GetComponentInChildren<Text>();
            slider.minValue = 0;
        }
		slider.maxValue = maxValue;
		slider.value = currentValue;
		txt.text = slider.value.ToString() + " / " + slider.maxValue.ToString();
	}
}
