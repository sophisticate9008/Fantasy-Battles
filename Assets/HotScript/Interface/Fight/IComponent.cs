
using UnityEngine;

public interface IComponent {
    public ArmConfigBase Config { get; }
    public string[] Types { get; set; }
    public string ComponentName{get;set;}
    public GameObject SelfObj{get; set;}

    public void Exec(GameObject enemyObj);

    public void Init();
}