using System;

namespace Factorys
{
    public class ActionFactory
    {
        public static Action CreateJewelAction(int id, int level)
        {
            GlobalConfig globalConfig = ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
            return id switch
            {
                1 => () => globalConfig.AttackValue += level * 10,
                2 => () => globalConfig.CritRate += level * 0.01f,
                3 => () => globalConfig.CritDamage += level * 0.1f,
                4 => () => globalConfig.AllAddition += level * 0.03f,
                5 => () => globalConfig.FireAddition += level * 0.04f,
                6 => () => globalConfig.EnergyAddition += level * 0.04f,
                7 => () => globalConfig.IceAddition += level * 0.04f,
                8 => () => globalConfig.WindAddition += level * 0.04f,
                9 => () => globalConfig.AdAddition += level * 0.04f,
                10 => () => globalConfig.ElecAddition += level * 0.04f,
                _ => throw new NotImplementedException(),
            };
        }
    }
}