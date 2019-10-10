using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayingState : GameManagerState
{

    public PlayingState(GameManager gm) : base(gm)
    {
    }   

    public override void OnStateEnter()
    {
        gm.MiniWorld.SetActive(true);
    }

    public override void Tick()
    {
 
    }
}
