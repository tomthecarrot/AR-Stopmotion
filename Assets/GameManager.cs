using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject character;
    public float animationFramerate;

    private float _secondsPerFrame;
    private float _animTime = 5.0f;
    private int _onionBackCount;
    private int _onionForwardCount;
    private List<GameObject> _onionBack;
    private List<GameObject> _onionForward;
    private float _onionTimeSpace = 0.05f;
    
	void Start () {
        _secondsPerFrame = 1f / animationFramerate;
        _onionBack = new List<GameObject>();
        _onionForward = new List<GameObject>();
        
        // test 1 back and forward
        _onionBack.Add( Instantiate<GameObject>(character, character.transform.position, character.transform.rotation ) );
        // _onionForward.Add( Instantiate<GameObject>( character, character.transform.position, character.transform.rotation ) );
    }

    void Update () {
        updateStencil();
    }

    private void updateStencil()
    {
        // keyframe
        Animator tKeyframeAnimator = character.GetComponent<Animator>();
        AnimatorClipInfo tKeyframeClipInfo = tKeyframeAnimator.GetCurrentAnimatorClipInfo( 0 )[ 0 ];
        // Debug.LogFormat( "Clip: {0}, {1}", tKeyframeClipInfo.clip.name, tKeyframeClipInfo.clip.length );

        if ( Input.GetAxis( "Mouse ScrollWheel" ) > 0 ) // forward
        { 
            _animTime += _secondsPerFrame;
            Debug.LogFormat( "Forward: {0}, {1}", tKeyframeClipInfo.clip.name, tKeyframeClipInfo.clip.length );
        }
        else if ( Input.GetAxis( "Mouse ScrollWheel" ) < 0 ) // back
        { 
            _animTime -= _secondsPerFrame;
            Debug.LogFormat( "Back: {0}, {1}", tKeyframeClipInfo.clip.name, tKeyframeClipInfo.clip.length );
        }

        tKeyframeAnimator.PlayInFixedTime( tKeyframeClipInfo.clip.name, -1, _animTime );
    } 
}
