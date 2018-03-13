// Copyright (c) Mira Labs, Inc., 2017. All rights reserved.
//
// Downloading and/or using this MIRA SDK is under license from MIRA,
// and subject to all terms and conditions of the Mira SDK License Agreement,
// found here: https://www.mirareality.com/Mira_SDK_License_Agreement.pdf
//
// By downloading this SDK, you agree to the Mira SDK License Agreement.
//
// This SDK may only be used in connection with the development of
// applications that are exclusively created for, and exclusively available
// for use with, MIRA hardware devices. This SDK may only be commercialized
// in the U.S. and Canada, subject to the terms of the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

public class RemoteAxisInput
{
    // Events
    public delegate void RemoteAxisInputEventHandler(RemoteAxisInput axisInput, EventArgs e);
    public event RemoteAxisInputEventHandler OnValueChanged;

    private float _value = 0;
    public float value 
    { 
        get 
        {
            return _value;
        }
        internal set 
        {
            if (value == _value)
            {
                return;
            }

            _value = value;

            if (OnValueChanged != null)
            {
                OnValueChanged(this, EventArgs.Empty);
            }
        }
    }

    internal RemoteAxisInput()
    {
    }
}