using System.Collections.Generic;

using UnityEngine;

public interface IEnemy {
    public Dictionary<string, float> BuffEndTimes{ get; set;}
    public float HardControlEndTime { get; set; }
    public float ControlEndTime { get; set; }
    public bool CanAction{get;set;}
    EnemyConfigBase Config { get;}
    Dictionary <string, IComponent> InstalledComponents{get;}
    Queue<IBuff> Buffs{ get;}
    void Move();
    void Attack();
    void Skill();
    void CalLife(int damage, string owner);

    //被谁杀死 
    void Die(string owner);
    void BuffEffect();
    void AddBuff(string buffname, GameObject selfObj, float duration, params object[] args);
}