public class EasyHurtNode : DamageNodeBase
{
                                                                                                                                 
    public override bool Process(DamageContext context) {
        context.FinalDamage *= 1 + context.DefenderComponent.EasyHurt;
        return true;
    }

}   