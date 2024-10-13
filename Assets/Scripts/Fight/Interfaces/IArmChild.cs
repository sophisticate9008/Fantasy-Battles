using System.Collections.Generic;
using MyBase;
using UnityEngine;


public interface IArmChild
{
    public ArmConfigBase Config { get;}
    public Dictionary<string,Queue<GameObject>> CollideObjs { get; }
    public Dictionary<string, IComponent> InstalledComponents { get; set; }
    //第一次忽略的物体，用于衍生技能排除本体
    public Queue<GameObject> FirstExceptQueue { get; set;}
    public Vector3 Direction { get; set; }
    public void CopyComponents<T>(T prefab) where T : MonoBehaviour
    {

    }
    public GameObject TargetEnemy { get; set; }
    //发出后自身的索敌,排除传入的enemy
    public GameObject FindTargetRandom(GameObject nowEnemy, bool setTargetEnemy = true);
    
}