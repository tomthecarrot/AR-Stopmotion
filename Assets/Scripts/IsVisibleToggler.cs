using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisibleToggler : MonoBehaviour {

	private bool isVisible = true;
	private GameObject owner;
	private Canvas parentCanvas;


	// someone call me
	public void toggleMenu(){
		if (parentCanvas != null) {
			parentCanvas.enabled = isVisible;
		}
		isVisible = !isVisible;
	}


	void Start () {
		owner = gameObject;
		if (owner != null) {

			parentCanvas = gameObject.GetComponent (typeof(Canvas)) as Canvas;
			//parentSlider = owner.GetComponent<Slider> ();
		} else {
			Debug.LogError ("owner not found...");
		}

		if (parentCanvas != null) {
			Debug.Log ("good here");
		} else {
			Debug.LogError ("no parent slider found");
		}

		HandheldController hc = HandheldController.Instance;
		hc.BackPressed += toggleMenu;

	}

	void Update () {
		if (Input.GetKeyDown ("space"))
			toggleMenu ();

	}


}
