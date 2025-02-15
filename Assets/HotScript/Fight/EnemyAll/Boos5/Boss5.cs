using UnityEngine;

public class Boss5:EnemyBase {
    public override bool AcceptHarm(GameObject enemy, GameObject armChild)
    {
        if(armChild.GetComponent<ArmChildBase>().Config.ComponentStrs.Contains("穿透")) {
            float rand = Random.Range(0,1);
            if(rand < 0.2f) {
                FighteManager.Instance.ShowText(gameObject, "闪避");
                return false;
            }
        }
        return true;
    }
}