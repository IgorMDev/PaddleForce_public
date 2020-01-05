using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleAnimation : MonoBehaviour{
    public UnityEvent On;
    public UnityEvent Off;
    public Animation anim;
    void Awake(){
         if(anim == null) anim = GetComponent<Animation>();
    }
    public void SetOn(){
        
        anim[anim.clip.name].normalizedTime = 0;
        anim[anim.clip.name].speed = 1;
        anim.Play(anim.clip.name);
    }
    public void SetOff(){
        
        anim[anim.clip.name].normalizedTime = 1;
        anim[anim.clip.name].speed = -1;
        anim.Play(anim.clip.name);
    }
}