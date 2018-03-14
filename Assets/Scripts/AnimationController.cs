// AR Stopmotion project
// USC Creating Reality 2018 hackathon
// Thomas Suarez (tomthecarrot), Ryan Reede (reedery), Elliot Mebane (elliotmebane)
// 2018 March 13

﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	///// VARIABLES AND GETTERS/SETTERS /////

	// Object references
	public static AnimationController instance;
	private Animation animation;
	private List<Animator> animators;

	// Editor-defined flag for legacy/humanoid modes
	public bool legacyMode = false;

	// Editor-defined speed for each animation
	public float _speed = 1.0f;

	// Editor-defined target frame rate (non-legacy only)
	public int targetFrameRate = 30;

	// Getter/setter version of the above variable
	public float speed {
		get {
			return _speed;
		}
		set {
			// Set new value to the variable
			_speed = value;

			// Set new value to the animation state
			if (legacyMode) { animation[clipName].speed = value; }
			else { }
		}
	}

	// Editor-defined name of the animation clip
	public string clipName = "Clip";

	// The number of the animation frame currently being displayed
	private int _currentFrame;

	// Public getter/setter version of the above variable
	public int currentFrame {
		get {
			// Get animation state values
			AnimationState state = animation[clipName];
			float currentTime = state.time;

			// Calculate and return
			int calculatedFrame = (int) (currentTime * frameRate);

			// If the difference between the calculated vs. stored frame number
			// is too great (> 1 second) to be reasonably shown on screen,
			// just return the calculated frame number
			if (Math.Abs(_currentFrame - calculatedFrame) > frameRate) {
				return calculatedFrame;
			}

			// Otherwise, return the stored frame number
			return _currentFrame;
		}
		set {
			// Set the new value
			this._currentFrame = value;

			// Calculate time from the new frame value
			float newTime = (float) this._currentFrame / frameRate;

			// Seek to that frame in the animation
			if (legacyMode) { animation[clipName].time = newTime; }
			else { }
		}
	}

	// Public getter version of the above variable
	public float frameRate {
		get {
			// Return animation clip value
			if (legacyMode) { return animation[clipName].clip.frameRate; }
			else { return targetFrameRate; }
		}
	}

	// Current playhead time, in seconds (non-legacy only)
	private float playheadTime = 0;
	
	// Whether the animation is still going
	public bool isAnimating {
		get {
			if (legacyMode) {
				return speed != 0f
							 && animation[clipName].speed != 0f
						 	 && animation.enabled;
			}
			else {
				return true; // TODO
			}
		}
	}

	///// LIFECYCLE /////

	void Awake() {
		// Assign singleton reference to this component object
		instance = this;

		// Initialize list of animators
		if (!legacyMode) { animators = new List<Animator>(); }
	}

	/// Initializer function
	void Start () {
		if (legacyMode) {
			// Get animation component
			animation = gameObject.GetComponent<Animation>();

			// Set animation to move along keyframes forward, then in reverse
			animation.wrapMode = WrapMode.PingPong;

			// Start animation
			animation.Play();
		}
		else {
			// Get animator component
			animators.Add(gameObject.GetComponent<Animator>());
			
			// Start animation
			Forward();
		}
	}

	// Update is called once per frame
	void Update () {
		// Use keyboard to test, out of headset
		KeyboardTest();

		// TODO add comment
		HandleNonLegacy();
	}

	///// INTERNAL /////

	void KeyboardTest() {
		if (Input.GetKeyDown(KeyCode.E)) {
			// If already animating, pause
			if (isAnimating) {
				Pause();
			}
			// Otherwise, start animating forward
			else {
				Forward();
			}
		}
		else if (Input.GetKeyDown(KeyCode.R)) {
			// If already animating, pause
			if (isAnimating) {
				Pause();
			}
			// Otherwise, start animating in reverse
			else {
				Reverse();
			}
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// Increase speed by 100% (2x)
			SetNewSpeed(speed * 2f);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			// Decrease speed by 100% (0.5x)
			SetNewSpeed(speed / 2f);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			// Move back by one frame
			BackOneFrame();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			// Move forward by one frame
			ForwardOneFrame();
		}
	}

	void HandleNonLegacy() {
		playheadTime += 1 / frameRate;// / Application.targetFrameRate;
		AnimatorClipInfo info = animators[0].GetCurrentAnimatorClipInfo(0)[0];
		animators[0].PlayInFixedTime(info.clip.name, -1, playheadTime);
	}

	///// EXPOSED /////

	public void Forward() {
		// Set speed to be positive
		speed = Math.Abs(speed);

		// Play the animation
		if (legacyMode) { animation.Play(); }
		else { animators[0].StartPlayback(); }
	}

	public void Reverse() {
		// Set speed to be negative
		speed = -Math.Abs(speed);

		// Play the animation
		if (legacyMode) { animation.Play(); }
		else { }
	}

	public void Pause() {
		// Stop the animation
		if (legacyMode) { animation[clipName].speed = 0f; }
		else { }
	}

	public void Resume() {
		// Set speed to be positive
		speed = Math.Abs(speed);
	}

	public void JumpToFrame(int frameNumber) {
		// Set new frame number, using setter method
		// (see top of this class)
		currentFrame = frameNumber;
	}

	public void SetNewSpeed(float newSpeed) {
		// Set new speed, using setter method
		// (see top of this class)
		speed = newSpeed;
	}

	public void ForwardOneFrame() {
		// Add one to current frame, using setter method
		// (see top of this class)
		currentFrame += 1;
	}

	public void BackOneFrame() {
		// Subtract one from current frame, using setter method
		// (see top of this class)
		currentFrame -= 1;
	}

}
