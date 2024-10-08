using System;
using UnityEngine;

public interface IBuff {
    GameObject EnemyObj { get; set; }
    GameObject SelfObj { get; set; }
    float Duration { get; set; }
    string BuffName { get; set;}
    void Effect();
    void EffectAndAutoRemove();

    void Remove();
    void EffectControl();
    void RemoveControl();
}