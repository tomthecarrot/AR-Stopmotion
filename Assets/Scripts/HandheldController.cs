using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandheldController : MonoBehaviour {
    private static HandheldController instance;

    public Action TriggerPressed;
    public Action TriggerReleased;
    public Action TouchpadPressed;
    public Action BackPressed;
    public Action StartPressed;

    private void Awake()
    {  
        if ( instance != null && instance != this )
        {
            Destroy( gameObject );
        }
        else
        {
            instance = this;
            DontDestroyOnLoad( gameObject );
        }
    }

    public static HandheldController Instance { get { return instance; } }
  
    // Use this for initialization
    void Start () {
        // debug
        TriggerPressed += DebugTriggerPressed;
        TriggerReleased += DebugTriggerReleased;
        TouchpadPressed += DebugTouchpadPressed;
        BackPressed += DebugBackPressed;
        StartPressed += DebugStartPressed;
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

        if ( MiraController.BackButtonPressed )
        {
            if ( BackPressed != null )
            {
                BackPressed();
            }
        }

        if ( MiraController.StartButtonPressed )
        {
            if ( StartPressed != null )
            {
                StartPressed();
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

    private void DebugBackPressed()
    {
        Debug.Log( "DebugBackPressed" );
    }

    private void DebugStartPressed()
    {
        Debug.Log( "DebugStartPressed" );
    }
}
