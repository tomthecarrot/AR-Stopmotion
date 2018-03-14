using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandheldController : MonoBehaviour {
    public static HandheldController instance;

    public Action TriggerPressed;
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

        if ( MiraController.TriggerButtonPressed )
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

    private void DebugTouchpadPressed()
    {
        Debug.LogFormat( "DebugTouchpadPressed at: {0}, {1}", MiraController.TouchPos[0], MiraController.TouchPos[ 1 ] );
    }

}
