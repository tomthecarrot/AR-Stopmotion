using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextSetter : MonoBehaviour {


	/// <summary>
	///  TODO handle float vs int better... with get val method set at runtime based on isInt;
	/// </summary>
	///



	private Slider parentSlider;
	private Text label;  
	private string myName;

	public float myVal = 0.0f;
	public bool isInt;

	void Awake () {
		// gets the obj this script is on

		GameObject owner = gameObject;
		if (owner != null) {

			parentSlider = gameObject.GetComponent (typeof(Slider)) as Slider;
			//parentSlider = owner.GetComponent<Slider> ();
		} else {
			Debug.LogError ("owner not found...");
		}

		if (parentSlider != null) {
			parentSlider.onValueChanged.AddListener (delegate {
				updateVal ();
			});

		} else {
			Debug.LogError ("no parent slider found");
		}

		label = GetComponentsInChildren<Text>()[0];
		// save for later
		Debug.Log(label);
		myName = label.text;
	}

	void Update () {

		int rounder;

		if (isInt) {
			rounder = 0;
		} else {
			rounder = 3;
		}
		// trim fat
		double temp = System.Math.Round(myVal, rounder);
		label.text = myName  + ": " +  temp.ToString();
	}

	void updateVal(){
		myVal = (float) parentSlider.value;
	}

}


