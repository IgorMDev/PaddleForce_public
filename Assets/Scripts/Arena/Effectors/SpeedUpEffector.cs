using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedUpEffector : Effector{
    public float speedUp = 1, 
                angSpeedUp = 1;
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Brick"){
            GetComponent<Animator>().Play("activate");
        }
    }
    public override void ApplyEffector(){
        if(actor != null){
            actor.GetComponent<ActorMover>().speed *= speedUp;
            actor.GetComponent<ActorMover>().angSpeed *= angSpeedUp;
            actor.brick.GetComponent<Animator>().Play("onSpeedUp");
            base.ApplyEffector();
        }
    }
    public override void Disable(){
        if(actor != null){
            actor.GetComponent<ActorMover>().speed /= speedUp;
            actor.GetComponent<ActorMover>().angSpeed /= angSpeedUp;
            actor.brick.GetComponent<Animator>().Play("idle");
        }
        base.Disable();
    }
}