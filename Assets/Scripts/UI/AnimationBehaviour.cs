using UnityEngine;
[RequireComponent(typeof(Animation))]
public class AnimationBehaviour : MonoBehaviour {
    Animation anim;
    private void Awake() {
        anim = GetComponent<Animation>();
    }
    public void Play(){
        anim.Play();
    }
    public void Play(string clip = ""){
        if(clip != "") anim.Play(clip);
    }
    
}