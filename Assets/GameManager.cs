using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // endpoint opacity
    float _opacity;
    int _framesForward;
    int _framesBack;
    int _framesSkip;
    int _modelScale;

    public GameObject animationControllerGO;
    AnimationController _animationController;
    // Actions to notify other classes about changes to the GameManager
    // Changes from UIManager
    Action opacityChanged;
    Action framesForwardChanged;
    Action framesBackChanged;
    Action framesSkipChanged;
    Action modelScaleChanged;

    // Changes from UIPersistentManager
    Action handleMoveFrameForward;
    public Action handleMoveFrameBack;
    Action handleShowKeyframe; 
     
    public GameObject uiManagerGO;
    //UIManager _uiManager;

    public GameObject uiPersistentManagerGO;
    // UIPersistentManager _uiPersistentManager;
     
    //public GameObject character;
    //public float animationFramerate;

    // private float _secondsPerFrame;
    //private float _animTime = 5.0f;
    //private int _onionBackCount;
    //private int _onionForwardCount;
    //private List<GameObject> _onionBack;
    //private List<GameObject> _onionForward;
    //private float _onionTimeSpace = 0.05f;

    public void testHandleMoveFrameBack()
    {
        Debug.Log("testHandleMoveFrameBack Called");
    }

    void Start ()
    {
        // handleMoveFrameBack += testHandleMoveFrameBack;

        // AnimationManager
        // _animationController =  animationControllerGO.GetComponent<AnimationController>();
        // opacityChanged += animationController.handleOpacityChanged;
        // framesForwardChanged += animationController.handleFramesForwardChanged;
        //framesBackChanged += animationController.handleFramesBackChanged;
        //framesSkipChanged += animationController.handleSkipChanged;
        //modelScaleChanged += animationController.handleModelScaleChanged;

        // Persistent UI Manager
        // uiManager = _uiManagerGO.GetComponent<UIManager>()
        // subscribe to uiManager's change notification handlers
        //uiManager.handleOpacityChanged += HandleOpacityChanged;
        //uiManager.handleFramesForwardchanged += HandleFrameForwardChanged;
        //uiManager.handleFramesBackChanged += HandleFrameBackChanged;
        //uiManager.handleFramesSkipChanged += HandleFramesSkipChanged;
        //uiManager.handleModelScaleChanged += HandleModelScaleChanged;

        // Persitent UI Manager
        // uiPersistentManager = _uiPersistentManagerGO.GetComponent<UIPersistentManager>()
        // subscribe to uiManager's change notification handlers
        // _uiPersistentManager.handleKeyframeChanged += 
        // _uiPersistentManager.handleShowKeyframe += 

        // _secondsPerFrame = 1f / animationFramerate; 
    }


    void Update () {
        handleMoveFrameBack.Invoke();
    }

    // UI Listeners
    //private void HandleOpacityChanged()
    //{
    //    Opacity = uiManager
    //};
    //uiManager.handleFramesForwardchanged += HandleFrameForwardChanged;
    //uiManager.handleFramesBackChanged += HandleFrameBackChanged;
    //uiManager.handleFramesSkipChanged += HandleFramesSkipChanged;
    //uiManager.handleModelScaleChanged += HandleModelScaleChanged;

    //private void updateStencil()
    //{
    // keyframe
    //Animator tKeyframeAnimator = character.GetComponent<Animator>();
    //AnimatorClipInfo tKeyframeClipInfo = tKeyframeAnimator.GetCurrentAnimatorClipInfo( 0 )[ 0 ];
    // Debug.LogFormat( "Clip: {0}, {1}", tKeyframeClipInfo.clip.name, tKeyframeClipInfo.clip.length );

    //if ( Input.GetAxis( "Mouse ScrollWheel" ) > 0 ) // forward
    //{ 
    //    _animTime += _secondsPerFrame;
    //    Debug.LogFormat( "Forward: {0}, {1}", tKeyframeClipInfo.clip.name, tKeyframeClipInfo.clip.length );
    //}
    //else if ( Input.GetAxis( "Mouse ScrollWheel" ) < 0 ) // back
    //{ 
    //    _animTime -= _secondsPerFrame;
    //    Debug.LogFormat( "Back: {0}, {1}", tKeyframeClipInfo.clip.name, tKeyframeClipInfo.clip.length );
    //}

   // tKeyframeAnimator.PlayInFixedTime( tKeyframeClipInfo.clip.name, -1, _animTime );
    //} 

    public int ModelScale
    {
        get
        {
            return _modelScale;
        }

        set
        {
            _modelScale = value;
            modelScaleChanged();
        }
    }

    public int FramesSkip
    {
        get
        {
            return _framesSkip;
        }

        set
        {
            _framesSkip = value;
            framesSkipChanged();
        }
    }

    public int FramesBack
    {
        get
        {
            return _framesBack;
        }

        set
        {
            _framesBack = value;
            framesBackChanged();
        }
    }

    public int FramesForward
    {
        get
        {
            return _framesForward;
        }

        set
        {
            _framesForward = value;
            framesForwardChanged();
        }
    }

    public float Opacity
    {
        get
        {
            return _opacity;
        }

        set
        {
            _opacity = value;
            opacityChanged();
        }
    }
}
