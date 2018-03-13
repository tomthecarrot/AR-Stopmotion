// AR Stopmotion project
// USC Creating Reality 2018 hackathon
// Thomas Suarez (tomthecarrot), Ryan Reede (reedery), Elliot Mebane (elliotmebane)
// 2018 March 13

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	// Object references
	private Animation animation;

	// Editor-defined speed for each animation
	public float speed = 1.0f;

	// Editor-defined name of the animation clip
	public string clipName = "Clip";

	// The number of the animation frame currently being displayed
	private int _currentFrame;

	// Public version of the above variable
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

	/// Initializer function
	void Start () {
		// Initialize animation object
		animation = gameObject.GetComponent<Animation>();

		// Set animation to move along keyframes forward, then in reverse
		animation.wrapMode = WrapMode.PingPong;
	}

	// Update is called once per frame
	void Update () {
		// Only when the animator is not yet playing (or has already finished)
		if (!animation.isPlaying) {
			// (Re)Start animation
			animation.Play();
		}
	}

	public void JumpToFrame(int frameNumber) {
		// Set new frame number, using setter method
		// (see top of this class)
		currentFrame = frameNumber;
	}

	public void ForwardOneFrame() {
		// Add one to current frame
		currentFrame += 1;

		// Update
		JumpToFrame(currentFrame);
	}

	public void BackOneFrame() {
		// Subtract one from current frame
		currentFrame -= 1;

		// Update
		JumpToFrame(currentFrame);
	}
}
