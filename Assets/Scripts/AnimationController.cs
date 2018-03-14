// AR Stopmotion project
// USC Creating Reality 2018 hackathon
// Thomas Suarez (tomthecarrot), Ryan Reede (reedery), Elliot Mebane (elliotmebane)
// 2018 March 13

﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	// Object references
	public static AnimationController instance;
	private Animation animation;

	// Editor-defined speed for each animation
	public float _speed = 1.0f;

	// Getter/setter version of the above variable
	public float speed {
		get {
			return _speed;
		}
		set {
			// Set new value to the variable
			_speed = value;

			// Set new value to the animation state
			animation[clipName].speed = value;
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
			return (int) (currentTime * frameRate);
		}
		set {
			// Set the new value
			this._currentFrame = value;

			// Calculate time from the new frame value
			float newTime = (float) this._currentFrame / frameRate;

			// Seek to that frame in the animation
			animation[clipName].time = newTime;
		}
	}

	public float frameRate {
		get {
			// Return animation clip value
			return animation[clipName].clip.frameRate;
		}
	}

	public bool isAnimating {
		get {
			return speed != 0f
						 && animation[clipName].speed != 0f
						 && animation.enabled;
		}
	}

	///// LIFECYCLE /////

	void Awake() {
		// Assign singleton reference to this component object
		instance = this;
	}

	/// Initializer function
	void Start () {
		// Initialize animation object
		animation = gameObject.GetComponent<Animation>();

		// Set animation to move along keyframes forward, then in reverse
		animation.wrapMode = WrapMode.PingPong;

		// Start animation
		animation.Play();
	}

	// Update is called once per frame
	void Update () {

	}

	///// EXPOSED FUNCTIONS /////

	public void Forward() {
		// Set speed to be positive
		speed = Math.Abs(speed);

		// Play the animation
		animation.Play();
	}

	public void Reverse() {
		// Set speed to be negative
		speed = -Math.Abs(speed);

		// Play the animation
		animation.Play();
	}

	public void Pause() {
		// Stop the animation
		animation[clipName].speed = 0f;
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
