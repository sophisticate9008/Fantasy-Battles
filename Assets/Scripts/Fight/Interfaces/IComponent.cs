using MyBase;
using UnityEngine;

public interface IComponent {
    public ArmConfigBase Config { get; set; }
    public string[] Type { get; set; }
    public string ComponentName{get;set;}
    public GameObject SelfObj{get; set;}

    public void Exec(GameObject enemyObj);

    public void Init();
}