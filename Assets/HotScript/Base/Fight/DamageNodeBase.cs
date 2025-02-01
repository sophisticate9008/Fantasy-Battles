public class DamageNodeBase
{
    
    public GlobalConfig GlobalConfig => ConfigManager.Instance.GetConfigByClassName("Global") as GlobalConfig;
    public virtual bool Process(DamageContext context)
    {
        return  true;
    }
}