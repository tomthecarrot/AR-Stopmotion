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

	/// Initializer function
	void Start () {
		// Initialize animation object
		animation = gameObject.GetComponent<Animation>();
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
}
