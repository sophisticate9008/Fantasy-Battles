using FightBases;
using UnityEngine;

namespace ArmsChild
{
    public class TntBoom : ArmChildBase
    {
        public override void OnTriggerEnter2D(Collider2D collider)
        {
            ApplyForce(collider, 1);
            base.OnTriggerEnter2D(collider);
        }
    }
}