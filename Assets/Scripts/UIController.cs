using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIController : MonoBehaviour {

	public Canvas mainMenu;

	public GameObject activeSettingsPanel;
	public GameObject activeCreditsPanel;
	public GameObject activeHelpPanel;

	public Slider opacitySlider;
	public Slider framesBackSlider;
	public Slider framesForwardSlider;
	public Slider frameSkipSlider;
	public Slider modelScaleSlider;

	public Button helpButton;
	public Button creditsButton;
	public Button exitBackButton;

	private float opacity;
	// lower bound on opacity too...?
	private int framesForward;
	private int framesBack;
	private int framesSkip;
	private int modelScale;

	private enum menuState {
   inactive,
   activeSettings,
   activeHelp,
	 activeCredits
 }

 private menuState currMenuState;

 // TEMPorary +_+_+_+_+_+_+_^+&_$%+*_%$^*+$%^*_+...
 private float dt = 0.0f;
 private bool t = true;
/// deletetete^^^

 // public Action opacityChanged;
 // public Action framesForwardChanged;
 // public Action framesBackChanged;
 // public Action framesSkipChanged;
 // public Action modelScaleChanged;


	void Start () {
		opacitySlider.onValueChanged.AddListener(delegate {updateOpacity(); });
		framesBackSlider.onValueChanged.AddListener(delegate {updateFramesBack(); });
		framesForwardSlider.onValueChanged.AddListener(delegate {updateFramesForward(); });
		frameSkipSlider.onValueChanged.AddListener(delegate {updateFramesSkip(); });
		modelScaleSlider.onValueChanged.AddListener(delegate {updateModelScale(); });

		helpButton.GetComponent<Button>().onClick.AddListener(delegate {helpClicked(); });
		creditsButton.GetComponent<Button>().onClick.AddListener(delegate {creditsClicked(); });
		exitBackButton.GetComponent<Button>().onClick.AddListener(delegate {exitBackClicked(); });

		mainMenu.GetComponent<Canvas>().enabled = false;

		activeSettingsPanel.SetActive(false);
		activeHelpPanel.SetActive(false);
		activeCreditsPanel.SetActive(false);
	}


	void Update(){
		/*
		dt += Time.deltaTime;
		if (dt > 4.0 && t){
			t = false;
			changeState(menuState.activeSettings);
		}
		*/
	}

	private void changeState(menuState nextState){
		Debug.Log("CHANGING STATE to... ");
		Debug.Log(nextState);
		Debug.Log("from: ");
		Debug.Log(this.currMenuState);
		switch (this.currMenuState){
			case menuState.inactive:
				mainMenu.GetComponent<Canvas>().enabled = true;
				activeSettingsPanel.SetActive(true);
				break;

			case menuState.activeSettings:
				switch (nextState){
					case menuState.activeHelp:
						setExitBack(false);
						activeHelpPanel.SetActive(true);
						activeSettingsPanel.SetActive(false);
						break;

					case menuState.activeCredits:
						setExitBack(false);
						activeCreditsPanel.SetActive(true);
						activeSettingsPanel.SetActive(false);
						break;

					case menuState.inactive:
						activeSettingsPanel.SetActive(false);
						mainMenu.GetComponent<Canvas>().enabled = false;
						break;
				}
				break;

			case menuState.activeHelp:
				setExitBack(true);
				activeHelpPanel.SetActive(false);
				activeSettingsPanel.SetActive(true);
				break;

			case menuState.activeCredits:
				setExitBack(true);
				activeCreditsPanel.SetActive(false);
				activeSettingsPanel.SetActive(true);
				break;

			}
		this.currMenuState = nextState;
	}

	public void activateMenu(){
			changeState(menuState.activeSettings);
	}

	private void helpClicked(){
		changeState(menuState.activeHelp);
		Debug.Log("help clicked");
	}

	private void creditsClicked(){
		changeState(menuState.activeCredits);
		Debug.Log("credits clicked");
	}

	private void exitBackClicked(){
		Debug.Log("credits clicked");
		switch (this.currMenuState){
			case menuState.activeHelp:
				changeState(menuState.activeSettings);
				break;

			case menuState.activeCredits:
				changeState(menuState.activeSettings);
				break;

			case menuState.activeSettings:
				changeState(menuState.inactive);
				break;
		}
	}

	private void setExitBack(bool isExit){
		string newStr = isExit ? "X" : "<";
		exitBackButton.GetComponentInChildren<Text>().text = newStr;
	}

	private void updateOpacity(){
		this.opacity = (float) opacitySlider.value;
		//opacityChanged();
		Debug.Log(this.opacity);
	}

	private void updateFramesBack(){
		this.framesBack = (int) framesBackSlider.value;
		//framesBackChanged();
		Debug.Log(this.framesBack);
	}

	private void updateFramesForward(){
		this.framesForward = (int) framesForwardSlider.value;
		//framesForwardChanged();
		Debug.Log(this.framesForward);
	}

	private void updateFramesSkip(){
		this.framesSkip = (int) frameSkipSlider.value;
		//framesSkipChanged();
		Debug.Log(this.framesSkip);
	}

	private void updateModelScale(){
		this.modelScale = (int) modelScaleSlider.value;
		//modelScaleChanged();
		Debug.Log(this.modelScale);
	}

}
