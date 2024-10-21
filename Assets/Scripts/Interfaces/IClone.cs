using UnityEngine;

public interface IClone
{
    bool IsInit { get; set;}
    void Init();
    void ReturnToPool();
}