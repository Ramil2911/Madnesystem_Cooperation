using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalMobileController;

public class TFPInput : MonoBehaviour
{
    public bool useFPSControllerAxisNames;
    public string jumpBtn = "Jump";
    public string crouchBtn = "Fire1";
    public string runBtn = "Fire3";
    public string unlockMouseBtn = "Cancel";
    public string xInName = "Horizontal";
    public string yInName = "Vertical";
    public string xMouseName = "Mouse X";
    public string yMouseName = "Mouse Y";
    
    public SpecialTouchPad mouseTrackpad;
    public Joystick movementJoystick;

    public float XAxis()
    {
        return movementJoystick.Horizontal;
    }

    public float YAxis()
    {
        return movementJoystick.Vertical;
    }

    public float XMouse()
    {
        //print(mouseTrackpad.GetHorizontalValue());
        return mouseTrackpad.GetHorizontalValue();
    }

    public float YMouse()
    {
        //print(mouseTrackpad.GetVerticalValue());
        return mouseTrackpad.GetVerticalValue();
    }

    public virtual void SetAxisNames(string jumpBtn, string crouchBtn, 
        string runBtn, string unlockMouseBtn, string xInName, 
        string yInName, string xMouseName, string yMouseName)
    {
    }

    public virtual bool CrouchPressed()
    {
        return false;
    }

    public virtual bool CrouchHeld()
    {
        return false;
    }

    public virtual bool RunPressed()
    {
        return false;
    }

    public virtual bool RunHeld()
    {
        return false;
    }

    public virtual bool JumpHeld()
    {
        return false;
    }

    public virtual bool JumpPressed()
    {
        return false;
    }

    public virtual bool UnlockMouseButton()
    {
        return false;
    }
}
