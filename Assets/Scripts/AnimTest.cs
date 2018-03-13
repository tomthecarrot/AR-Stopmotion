using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    public Animator characterAnimator;

    private float _animTime = 0f;

    // Use this for initialization
    void Start()
    {
        // Animator.PlayInFixedTime
        // https://docs.unity3d.com/ScriptReference/Animator.PlayInFixedTime.html

        characterAnimator.StartPlayback();
        // https://docs.unity3d.com/ScriptReference/Animator.StartPlayback.html
        // https://docs.unity3d.com/ScriptReference/Animator.PlayInFixedTime.html
    }

    // Update is called once per frame
    void Update()
    { 
        AnimatorClipInfo tClipInfo = characterAnimator.GetCurrentAnimatorClipInfo( 0 )[ 0 ];
        Debug.LogFormat( "Clip: {0}, {1}", tClipInfo.clip.name, tClipInfo.clip.length );

        if ( Input.GetAxis( "Mouse ScrollWheel" ) > 0 ) // forward
        {
            Debug.Log( "forward" );
            _animTime += 0.1f;
        }
        else if ( Input.GetAxis( "Mouse ScrollWheel" ) < 0 ) // back
        {
            Debug.Log( "back" );
            _animTime -= 0.1f;
        }

        characterAnimator.PlayInFixedTime( tClipInfo.clip.name, -1, _animTime );
    }
}
