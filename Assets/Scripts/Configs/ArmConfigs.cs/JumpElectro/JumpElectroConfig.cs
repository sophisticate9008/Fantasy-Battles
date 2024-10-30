
namespace ArmConfigs
{
    public class JumpElectroConfig : ArmConfigBase
    {
        
        public int JumpCount {get;set;} = 5;
        public override void Init()
        {
            base.Init();
            Tlc = 1;
            RangeFire = 8;
            AttackCount = 1;
            AttackCd = 0.2f;
            OnType = "enter";
            DamageType = "electro";
            Cd = 4;
            IsLineCast = true;
            
        }
    }
}
