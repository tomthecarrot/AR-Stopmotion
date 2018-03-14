using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandheldController : MonoBehaviour {
    public static HandheldController instance;

    public Action TriggerPressed;
    public Action TriggerReleased;
    public Action TouchpadPressed;
    
    private void Awake()
    {
        if ( instance != null )
        {
            Destroy( gameObject );
        }
        else
        {
            instance = this;
            DontDestroyOnLoad( gameObject );
        }
    }
    
    // Use this for initialization
    void Start () {
        // debug
        TriggerPressed += DebugTriggerPressed;
        TriggerReleased += DebugTriggerReleased;
        TouchpadPressed += DebugTouchpadPressed;
	}
	
	// Update is called once per frame
	void Update () {
        if ( MiraController.TriggerButtonPressed )
        {
            if ( TriggerPressed != null )
            {
                TriggerPressed();
            }
        }

        if ( MiraController.TriggerButtonReleased )
        {
            if ( TriggerReleased != null )
            {
                TriggerReleased();
            }
        }

        if ( MiraController.TouchpadButtonPressed )
        {
            if ( TouchpadPressed != null )
            {
                TouchpadPressed();
            }
        } 
    }

    private void DebugTriggerPressed()
    {
        Debug.Log( "DebugTriggerPressed" );
    }

    private void DebugTriggerReleased()
    {
        Debug.Log( "DebugTriggerReleased" );
    }

    private void DebugTouchpadPressed()
    {
        Debug.LogFormat( "DebugTouchpadPressed at: {0}, {1}", MiraController.TouchPos[0], MiraController.TouchPos[ 1 ] );
    }  
}
