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

	// Direction for next animation
	// (true = animate forward, false = animate in reverse)
	private bool forward = true;

	/// Initializer function
	void Start () {
		// Link object references
		animation = gameObject.GetComponent<Animation>();
	}

	// Update is called once per frame
	void Update () {
		// Only when the animator is not yet playing (or has already finished)
		if (!animation.isPlaying) {

			// Set speed (forward or backward)
			foreach (AnimationState state in animation) {
				state.speed = forward ? speed : -speed;
			}

			// Toggle for next time
			forward = !forward;

			// (Re)Start animation
			animation.Play();
		}
	}
}
