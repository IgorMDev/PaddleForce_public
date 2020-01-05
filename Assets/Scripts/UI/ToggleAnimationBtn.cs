using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleAnimationBtn : ToggleBtn{
    public Animation anim;
    public AnimationClip clip;
    new void Awake(){
         if(anim == null) anim = GetComponent<Animation>();
         if(clip != null && clip.name != anim.clip.name){
             anim.clip = clip;
         }
         base.Awake();
    }
    public override void SetOn(){
        anim.Play(anim.clip.name);
        anim[anim.clip.name].normalizedTime = 0;
        anim[anim.clip.name].speed = 1;
        base.SetOn();
    }
    public override void SetOff(){
        anim.Play(anim.clip.name);
        anim[anim.clip.name].normalizedTime = 1;
        anim[anim.clip.name].speed = -1;
        base.SetOff();
    }
}