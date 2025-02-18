using System.Collections.Generic;

using UnityEngine;

public interface IEnemy {
    public Dictionary<string, float> BuffEndTimes{ get; set;}
    public float HardControlEndTime { get; set; }
    public float ControlEndTime { get; set; }
    public bool CanAction{get;set;}
    EnemyConfigBase Config { get;}
    void Move();
    void Attack();

    //被谁杀死 
    void Die(string owner);
    void BuffEffect();
}