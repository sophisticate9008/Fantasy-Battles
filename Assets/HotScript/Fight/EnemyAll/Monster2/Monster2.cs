using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster2 : EnemyBase
{
    public override void UseBuff(BuffBase buff)
    {
        if (buff.BuffName.Contains("点燃"))
        {
            buff.Duration *= 2;
        }
        base.UseBuff(buff);
    }

    public override void Init()
    {
        base.Init();
        lastAddBloodTime = Time.time;
        void die()
        {
            List<GameObject> arroundEnemies = FindEnemyInScope();
            bool fire = isFire;
            if (fire)
            {
                FighteManager.Instance.ShowText(gameObject, "爆燃", false);
            }
            else
            {
                FighteManager.Instance.ShowText(gameObject, "治愈", false);
            }
            foreach (var enemy in arroundEnemies)
            {
                EnemyBase eb = enemy.transform.GetComponent<EnemyBase>();
                if (fire)
                {
                    eb.AddBuff("草兽点燃", null, 3f, 0.01f);
                }
                else
                {
                    eb.AddLife((int)(MaxLife * 0.05f));
                }
            }
        }
        allTypeActions["die"].Add(die);
    }
    public override void Update()
    {
        base.Update();
        if (Time.time - lastAddBloodTime > 3f)
        {
            if (isFire)
            {
                return;
            }
            lastAddBloodTime = Time.time;
            AddLife((int)(MaxLife * 0.05f));
        }


    }
}