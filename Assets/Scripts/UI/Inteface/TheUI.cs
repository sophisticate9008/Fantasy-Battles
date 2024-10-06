using UnityEngine;

public interface TheUI {
    public PlayerDataConfig PlayerDataConfig{ get; set; }
    public GameObject Prefab{get;set;}
    public void Init();
}