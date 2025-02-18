
using System;
using System.Collections.Generic;

public class BoomFireBallBoomConfig : ArmConfigBase
{

    public override void Init()
    {
        base.Init();
        Duration = 0.5f;
        OnType = "enter";
        Tlc = 0.5f;
        DamageType = "fire";
        MaxForce = 150;
    }
}

