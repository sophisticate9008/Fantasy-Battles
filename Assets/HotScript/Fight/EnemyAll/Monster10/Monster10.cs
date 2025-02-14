

public class Monster10:EnemyBase {
    public override void Init()
    {
        base.Init();
        void Die() {
            var enemies = FindEnemyInScope();
            foreach (var enemy in enemies) {
                EnemyBase eb = enemy.GetComponent<EnemyBase>();
                eb.Config.Speed *= 0.5f;
                eb.animatorManager.PlayAnim(animator, animator.speed * 1.5f);
                ToolManager.Instance.SetTimeout(() => {
                    eb.animatorManager.PlayAnim(animator, animator.speed / 1.5f);
                }, 2);
            }
        }
        allTypeActions["die"].Add(Die);
    }
}