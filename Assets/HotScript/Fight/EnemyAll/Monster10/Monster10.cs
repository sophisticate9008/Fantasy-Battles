

public class Monster10:EnemyBase {
    public override void Init()
    {
        base.Init();
        void Die() {
            var enemies = FindEnemyInScope();
            FighteManager.Instance.ShowText(gameObject, "加速周围");
            foreach (var enemy in enemies) {
                EnemyBase eb = enemy.GetComponent<EnemyBase>();
                eb.Config.Speed *= 1.5f;
                eb.animatorManager.PlayAnim(animator, animator.speed * 1.5f);
                ToolManager.Instance.SetTimeout(() => {
                    eb.animatorManager.PlayAnim(animator, animator.speed / 1.5f);
                    eb.Config.Speed /= 1.5f;
                }, 2);
            }
        }
        allTypeActions["die"].Add(Die);
    }
}