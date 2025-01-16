using System.Collections.Generic;

using UnityEngine;
[System.Serializable]
//关卡通过情况
public class MissionPassRecordConfig : ConfigBase
{
    public override bool IsCreatePool { get; set; } = false;
    public Dictionary<int,MissionRecord> PassRecords = new();

    
}