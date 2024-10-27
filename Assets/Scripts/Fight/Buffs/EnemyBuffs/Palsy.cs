using FightBases;
using UnityEngine;

namespace TheBuffs
{
    public class Palsy : BuffBase
    {

        public Palsy(string buffName, float duration,GameObject selfObj, GameObject enemyObj) : base(buffName, duration,selfObj, enemyObj)
        {

        }
        public override void Effect()
        {
            EffectControl();
        }

        public override void Remove()
        {
            RemoveControl();
        }
    }
}