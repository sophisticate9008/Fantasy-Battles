



using UnityEngine;

public class MagicBullet : ArmChildBase
{
    public override ArmChildBase ProcessObj(ArmChildBase obj)
    {
        foreach(Transform child in obj.transform) {
            if(child.name == Config.DamageType) {
                child.gameObject.SetActive(true);
            }else {
                child.gameObject.SetActive(false);
            }
        }
        return base.ProcessObj(obj);
    }
}



