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

    private WebSocket w;
    private string ServerHost = "ec2-52-89-222-51.us-west-2.compute.amazonaws.com";

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
            if ( legacyMode ) { animation[ clipName ].speed = value; }
        }
    }

    // Editor-defined name of the animation clip
    public string clipName = "Clip";

    // The character GameObject
    public GameObject character;

    // The number of the animation frame currently being displayed
    private int _currentFrame;

    // Helper Singleton for listening to Mira controller events
    private HandheldController _handheldController;

    // the fraction of a second that represents each frame;
    private float _secondsPerFrame;

    // onion skin alphas. High is the most opaque, Low is the least opaque
    private float _onionAlphaHigh, _onionAlphaLow;

    // onion skin hide/show
    private bool _onionSkinOn = true;

    // onion skin GameObjects
    private List<GameObject> _onionSkinGO;

    // Public getter/setter version of the above variable
    public int currentFrame {
        get {
            float currentTime;

            if ( legacyMode ) {
                // Get animation state values
                AnimationState state = animation[ clipName ];
                currentTime = state.time;
            }
            else {
                // Get current playhead (non-legacy only)
                currentTime = playheadTime;
            }

            // Calculate and return
            int calculatedFrame = (int)( currentTime * frameRate );

            // If the difference between the calculated vs. stored frame number
            // is too great (> 0.25 second) to be reasonably shown on screen,
            // just return the calculated frame number
            if ( Math.Abs( _currentFrame - calculatedFrame ) > frameRate * 0.25 ) {
                return calculatedFrame;
            }

            // Otherwise, return the stored frame number
            return _currentFrame;
        }
        set {
            // Set the new value
            this._currentFrame = value;

            // Calculate time from the new frame value
            float newTime = (float)this._currentFrame / frameRate;

            // Seek to that frame in the animation
            if ( legacyMode ) { animation[ clipName ].time = newTime; }
            else { playheadTime = newTime; }
        }
    }

    // Public getter version of the above variable
    public float frameRate {
        get {
            // Return animation clip value
            if ( legacyMode ) { return animation[ clipName ].clip.frameRate; }
            else { return targetFrameRate; }
        }
    }

    // Current playhead time, in seconds (non-legacy only)
    private float _playheadTime = 0;

    // Public getter/setter version of the above variable
    public float playheadTime {
        get {
            return _playheadTime;
        }
        set {
            // This setter is for non-legacy mode only,
            // so skip this setter if the app is in legacy mode
            if ( legacyMode ) { return; }

            // Set value
            _playheadTime = value;

            // Seek to animation frame at the new playhead time
            PlayInFixedTime();
        }
    }

    // Whether the animation is still going
    private bool _isAnimating = false;

    // original material color
    private Color _colorOn;

    // framerate multiplier spreads out the onion times for debugging
    private float _exaggerateOnion = 10;

    // Public getter/setter version of the above variable
    public bool isAnimating {
        get {
            if ( legacyMode ) {
                return speed != 0f
                             && animation[ clipName ].speed != 0f
                              && animation.enabled;
            }
            else {
                return _isAnimating;
            }
        }
        set {
            // Only settable if running in non-legacy mode
            // (see the above getter to understand why)
            if ( !legacyMode ) {
                // Set new value
                _isAnimating = value;
            }
        }
    }

    ///// LIFECYCLE /////

    void Awake() {
        // Assign singleton reference to this component object
        instance = this;

        // Initialize list of animators
        if ( !legacyMode ) { animators = new List<Animator>(); }

        // Init camera network
        w = new WebSocket(new Uri("ws://" + ServerHost + ":8080"));
    		StartCoroutine(w.Connect());
    }

    /// Initializer function
    void Start() {
        _colorOn = character.transform.Find( "MHuman" ).GetComponent<Renderer>().material.color;
        _onionAlphaHigh = 0.75f;
        _onionAlphaLow = 0.15f;

        _onionSkinGO = new List<GameObject>();

        SetSecondsPerFrame();

        if ( legacyMode ) {
            // Get animation component
            animation = character.GetComponent<Animation>();

            // Set animation to move along keyframes forward, then in reverse
            animation.wrapMode = WrapMode.PingPong;

            // Start animation
            animation.Play();
        }
        else {
            // Get animator component
            float tAlpha;
            int i;
            GameObject tNewCharacter;
            for ( i = 0; i < 5; i++ )
            {
                tNewCharacter = Instantiate( character, character.transform.position, character.transform.rotation );
                _onionSkinGO.Add( tNewCharacter );
                tAlpha = RemapIntToFloatRange( i, 0, 4, _onionAlphaLow, _onionAlphaHigh );
                Debug.Log(tAlpha);
                tNewCharacter.transform.Find( "MHuman" ).GetComponent<Renderer>().material.color = new Color( _colorOn.r, _colorOn.g, _colorOn.b, tAlpha );
                animators.Add( tNewCharacter.GetComponent<Animator>() );
            }

            //Debug.Log("human" + character.transform.Find( "MHuman" ).GetComponent<Renderer>().material );
            //character.transform.Find( "MHuman" ).GetComponent<Renderer>().material.color = new Color( _colorOn.r, _colorOn.g, _colorOn.b, 0.5f ); // RemapIntToFloatRange( i, 4, 0, _onionAlphaHigh, _onionAlphaLow ) );
            animators.Add( character.GetComponent<Animator>() );

            //for ( i = 5; i < 10; i++ )
            //{
            //    tNewCharacter = Instantiate( character, character.transform.position, character.transform.rotation );
            //    tAlpha = RemapIntToFloatRange( i, 5, 9, _onionAlphaHigh, _onionAlphaLow );
            //    Debug.Log( tAlpha ); 
            //    tNewCharacter.transform.Find( "MHuman" ).GetComponent<Renderer>().material.color = new Color( _colorOn.r, _colorOn.g, _colorOn.b, tAlpha );
            //    animators.Add( tNewCharacter.GetComponent<Animator>() );
            //}

            // Start animation
            StartPlayback();
        }

        _handheldController = HandheldController.Instance;
        _handheldController.TriggerPressed += HandleTriggerPressed;
        _handheldController.TriggerReleased += HandleTriggerReleased;
        _handheldController.TouchpadPressed += HandleTouchpadPressed;
        _handheldController.BackPressed += HandleBackPressed;
        _handheldController.StartPressed += HandleStartPressed;
    }

    // Update is called once per frame
    void Update() {
        // Use keyboard to test, out of headset
        KeyboardTest();

        // Handle new animator
        if ( !legacyMode ) { HandleNewAnimator(); }
    }

    ///// INTERNAL /////

    void KeyboardTest() {
        if ( Input.GetKeyDown( KeyCode.T ) )
        {
            // If already animating, pause
            if ( isAnimating )
            {
                Pause();
            }
            // Otherwise, start animating forward
            else
            {
                BackOneFrame();
            }
        }
        else if ( Input.GetKeyDown( KeyCode.Y ) )
        {
            // If already animating, pause
            if ( isAnimating )
            {
                Pause();
            }
            // Otherwise, start animating in reverse
            else
            {
                ForwardOneFrame();
            }
        } else if ( Input.GetKeyDown( KeyCode.E ) ) {
            // If already animating, pause
            if ( isAnimating ) {
                Pause();
            }
            // Otherwise, start animating forward
            else {
                Forward();
            }
        }
        else if ( Input.GetKeyDown( KeyCode.R ) ) {
            // If already animating, pause
            if ( isAnimating ) {
                Pause();
            }
            // Otherwise, start animating in reverse
            else {
                Reverse();
            }
        }
        else if ( Input.GetKeyDown( KeyCode.U ) )
        {
            ToggleOnionSkin();
        }
        else if ( Input.GetKeyDown( KeyCode.UpArrow ) ) {
            // Increase speed by 10%
            SetNewSpeed( speed * 1.1f );
        }
        else if ( Input.GetKeyDown( KeyCode.DownArrow ) ) {
            // Decrease speed by 10%
            SetNewSpeed( speed / 1.1f );
        }
        else if ( Input.GetKeyDown( KeyCode.LeftArrow ) ) {
            // Move back by one frame
            BackOneFrame();
        }
        else if ( Input.GetKeyDown( KeyCode.RightArrow ) ) {
            // Move forward by one frame
            ForwardOneFrame();
        }
    }

    void HandleNewAnimator() {
        // Skip if shouldn't be animating
        if ( !isAnimating ) { return; }

        // Add to playhead time
        playheadTime += _secondsPerFrame; // (1 / frameRate) * speed; // (Application.targetFrameRate)
    }

    // set StartPlayback on all the Animators
    void StartPlayback()
    {
        int i;
        int len = animators.Count;
        for ( i = 0; i < len; i++ )
        {
            animators[ i ].StartPlayback();
        }
    }

    //
    void PlayInFixedTime()
    {
        AnimatorClipInfo info;

        float tTime;
        int i;
        // Back Frames
        for ( i = 0; i < 5; i++ )
        {
            // Get the current animator clip data
            info = animators[ i ].GetCurrentAnimatorClipInfo( 0 )[ 0 ];

            tTime = _playheadTime - ( 5 - i ) * _secondsPerFrame;
            //Debug.Log( tTime );
            animators[ i ].PlayInFixedTime( info.clip.name, -1, tTime * _exaggerateOnion );
        }

        // Keyframe at position 6
        info = animators[ 5 ].GetCurrentAnimatorClipInfo( 0 )[ 0 ];
        //Debug.Log( _playheadTime );
        animators[ i ].PlayInFixedTime( info.clip.name, -1, _playheadTime );

        // Forward Frames
        //for ( i = 5; i < 10; i++ )
        //{
        //    // Get the current animator clip data
        //    info = animators[ i ].GetCurrentAnimatorClipInfo( 0 )[ 0 ];
        //    tTime = _playheadTime + ( i * _secondsPerFrame );
        //    //Debug.Log( tTime );
        //    animators[ i ].PlayInFixedTime( info.clip.name, -1, tTime * _exaggerateOnion );
        //}
    }

    // calculates the fraction of a second that represents each frame at the given frame rate
    void SetSecondsPerFrame()
    {
        _secondsPerFrame = ( 1 / frameRate ) * speed;
    }

    ///// EXPOSED /////

    public void Forward() {
        // Set speed to be positive
        speed = Math.Abs( speed );

        // Play the animation
        if ( legacyMode ) { animation.Play(); }
        else { isAnimating = true; }
    }

    public void Reverse() {
        // Set speed to be negative
        speed = -Math.Abs( speed );

        // Play the animation
        if ( legacyMode ) { animation.Play(); }
        else { isAnimating = true; }
    }

    public void Pause() {
        // Stop the animation
        if ( legacyMode ) { animation[ clipName ].speed = 0f; }
        else { isAnimating = false; }
    }

    public void Resume() {
        // Set speed to be positive
        speed = Math.Abs( speed );
    }

    public void JumpToFrame( int frameNumber ) {
        // Set new frame number, using setter method
        // (see top of this class)
        currentFrame = frameNumber;
    }

    public void SetNewSpeed( float newSpeed ) {
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

    public void ToggleOnionSkin()
    {
        _onionSkinOn = !_onionSkinOn;

        int i = 0;
        int len = _onionSkinGO.Count;
        for ( i = 0; i < len; i++ )
        {
            _onionSkinGO[ i ].transform.Find( "MHuman" ).GetComponent<Renderer>().enabled = _onionSkinOn;
        }
    }

    private void HandleTriggerPressed()
    {
        Debug.Log( "HandleTriggerPressed" );
        character.SetActive( false );
    }

    private void HandleTriggerReleased()
    {
        Debug.Log( "HandleTriggerReleased" );
        character.SetActive( true );
    }

    private void HandleTouchpadPressed()
    {
        Debug.LogFormat( "HandleTouchpadPressed at: {0}, {1}", MiraController.TouchPos[ 0 ], MiraController.TouchPos[ 1 ] );
        if ( MiraController.TouchPos[ 1 ] > 0.5f )
        {
            // frame forward
            Forward();
        }
        else if ( MiraController.TouchPos[ 1 ] <= 0.5f )
        {
            // frame back
            Reverse();
        }
    }

    private void HandleBackPressed()
    {
        Debug.Log( "HandleBackPressed" );

        w.SendString("photo");
    }

    private void HandleStartPressed()
    {
        Debug.Log( "HandleStartPressed" );

        ToggleOnionSkin();
    }
    
    // utility range map
    public static float RemapIntToFloatRange( int value, float from1, float to1, float from2, float to2 )
    {
        return ( (float)value - from1 ) / ( to1 - from1 ) * ( to2 - from2 ) + from2;
    }
    
}
