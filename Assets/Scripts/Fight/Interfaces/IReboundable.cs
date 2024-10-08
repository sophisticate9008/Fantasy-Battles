using UnityEngine;

public interface IReboundable {
    public int ReboundCount {get; set;}
    public void Rebound();
}