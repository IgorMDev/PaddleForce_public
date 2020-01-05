using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GoalTrigger : Node2D{
    public GoalEvent onEnter;
    Animator anim;
    bool hided;
    void Awake(){
        anim = GetComponent<Animator>();
    }
    void Start(){
        renderer.size = new Vector2(renderer.size.x, Arena.rect.height+100);
        GetComponent<BoxCollider2D>().size = renderer.size;
        Hide();
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Ball"){
            if(!hided){
                GetComponent<AudioSource>().Play();
                anim.Play("Goal");
                onEnter.Invoke(col.GetComponent<Ball>());
            }
        }
    }
    void RoundStart(){
        Hide();
    }
    void GamePause(){
        GetComponent<Collider2D>().enabled = false;
    }
    void GameResume(){
        GetComponent<Collider2D>().enabled = true;
    }
    public void Hide(){
            GetComponent<Collider2D>().enabled = false;
            anim.Play("Hide");
            hided = true;
    }
    public void Show(){
            GetComponent<Collider2D>().enabled = true;
            anim.Play("Blink");
            hided = false;
    }
}