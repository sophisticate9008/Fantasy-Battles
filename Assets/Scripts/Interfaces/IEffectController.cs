using UnityEngine;

public interface IEffectController{
    public string EffectName{get;set;}
    public bool IsPlaying { get; set;}
    public GameObject Enemy{get; set;}
    public void Init();
    public void Play();
    public void Stop();
}