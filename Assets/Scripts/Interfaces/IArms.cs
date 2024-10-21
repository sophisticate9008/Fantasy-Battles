using System.Collections.Generic;
using MyBase;
using UnityEngine;

public interface IArms {

    public ArmConfigBase Config {get;}
    public GameObject TargetEnemy{get;set;}
    public void FindTargetNearestOrElite();
    public List<GameObject> FindRandomTarget(int count);
    public void FisrtFindTarget();
    public void OtherFindTarget();


}