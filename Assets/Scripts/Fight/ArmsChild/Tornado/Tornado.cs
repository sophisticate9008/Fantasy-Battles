using MyBase;
using UnityEngine;

public class Tornado : ArmChildBase {
    public override void Init()
    {
        base.Init();
        ChangeScale();
    }
    
    void ChangeScale() {
        gameObject.transform.localScale *= Config.SelfScale;
        foreach (Transform child in gameObject.transform) {
            child.localScale *= Config.SelfScale;
        }
    }
    public override void OnCollisionStayr2D(Collision2D collision)
    {
        base.OnCollisionStayr2D(collision);
        
    }
    public override void Move()
    {
        
    }


}