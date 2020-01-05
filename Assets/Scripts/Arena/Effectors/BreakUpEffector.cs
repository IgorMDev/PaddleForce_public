using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakUpEffector : Effector{
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Brick"){
            GetComponent<Animator>().Play("activate");
        }
        
    }
    public override void ApplyEffector(){
        if(actor != null){
            actor.GetComponent<ActorMover>().bounded = false;
            actor.brick.GetComponent<Animator>().Play("onBreakUp");
            base.ApplyEffector();
        }
    }
    public override void Disable(){
        if(actor != null){
            actor.GetComponent<ActorMover>().bounded = true;
            actor.brick.GetComponent<Animator>().Play("offBreakUp");
        }
        base.Disable();
    }
}