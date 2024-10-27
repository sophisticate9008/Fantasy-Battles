using FightBases;
using UnityEngine;

public class HoldComponent : ComponentBase
{
    public HoldComponent(string componentName, string type, GameObject selfObj) : base(componentName, type, selfObj)
    {
    }

    public override void Exec(GameObject enemyObj)
    {
        throw new System.NotImplementedException();
    }
}